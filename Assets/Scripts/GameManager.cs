using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int day = 0;
    public List<DayData> dayUpgrades = new List<DayData>();

    public float timer;
    public float maxTimer;

    [Header("Order Settings")]
    public List<SO_BobaOrder> orderOptionsDefault;
    public List<SO_BobaOrder> orderOptionsBonus1;
    public List<SO_BobaOrder> orderOptionsBonus2;
    public GameObject ordersBar;
    public float orderInterval;
    public float orderTime;
    [HideInInspector] public bool orderIsInfinite;
    public GameObject orderPrefab;
    public GameObject orderEmptySprite;
    public List<Order> activeOrders;

    [Header("UI")]
    public TextMeshProUGUI moneyDisplay;
    public TextMeshProUGUI fallingMoneyText;
    public Animator fallingMoneyAnimator;
    public Image timerClock;
    public TextMeshProUGUI dayTimeDisplay;
    public GameObject pauseScreen;
    public GameObject lossScreen;

    [Header("Not Order Settings")]
    public GameObject endOfDay;

    public int money;
    public int orderValue;

    public float conveyorMoveSpeed;
    public bool isPlaying;

    public Resource rsc_cup;
    public Resource rsc_ice;
    public Resource rsc_milk;
    public Resource rsc_tea;
    public Resource rsc_juice;
    public Resource rsc_tapioca;
    public Resource rsc_fruit;
    public float shakeTime;


    private Coroutine dayTimer;
    private List<BobaController> activeBoba = new List<BobaController>();

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        UpdateMoney();
    }

    private void UpdateMoney()
    {
        if(moneyDisplay == null)
        {
            return;
        }
        moneyDisplay.text = "$" + money;
    }

    public void StartNewDay()
    {
        UpdateMoney();

        if(dayTimer != null)
        {
            StopCoroutine(dayTimer);
        }
        day++;
        if(day == 1)
        {
            orderIsInfinite = true;
        }
        else
        {
            orderIsInfinite = false;
        }

        orderTime *= 0.95f;
        orderTime = Mathf.Clamp(orderTime,20f, 120f);

        orderInterval *= 0.95f;
        orderInterval = Mathf.Clamp(orderInterval, 3f, 100f);

        for(int i = 0; i < dayUpgrades.Count; i++)
        {
            if(day == dayUpgrades[i].dayNumber)
            {
                dayUpgrades[i].newEffects.Invoke();
            }
        }
        dayTimer = StartCoroutine(DayTimer());

        dayTimeDisplay.text = "Time left in day " + day + ":";
        if (day > 1)
        {
            RefreshForDay();
        }
    }

    public void AddBobaController(BobaController bc)
    {
        activeBoba.Add(bc);
    }

    private void RefreshForDay()
    {
        rsc_cup.FillResourcesSilently();
        rsc_fruit.FillResourcesSilently();
        rsc_ice.FillResourcesSilently();
        rsc_juice.FillResourcesSilently();
        rsc_milk.FillResourcesSilently();
        rsc_tapioca.FillResourcesSilently();
        rsc_tea.FillResourcesSilently();

        for(int i = 0; i < activeBoba.Count; i++)
        {
            if (activeBoba[i] != null)
            {
                Destroy(activeBoba[i].gameObject);
            }
        }
        activeBoba.Clear();
    }

    private void EndDay()
    {
        //do end of day stuff here, like upgrades menu
        endOfDay.SetActive(true);
        endOfDay.GetComponent<EndOfDay>().ResetOptions();
        ResetOrders();

        // Add additional orders if specific days (sorry but in a rush)
        if (day == 2)
        {
            for (int i = 0; i < orderOptionsBonus1.Count; i++)
            {
                orderOptionsDefault.Add(orderOptionsBonus1[i]);
            }
        }
        if (day == 4)
        {
            for (int i = 0; i < orderOptionsBonus2.Count; i++)
            {
                orderOptionsDefault.Add(orderOptionsBonus2[i]);
            }
        }
    }

    public void GameFailed()
    {
        //call this if an order gets unfulfilled for long enough that you run out of time
        lossScreen.SetActive(true);

        if(dayTimer != null)
        {
            isPlaying= false;
            StopCoroutine(dayTimer);
        }
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeliverOrder(BobaController bc)
    {
        if (CompareOrders(bc))
        {
            money += orderValue;
            fallingMoneyText.text = "+" + orderValue.ToString();
            fallingMoneyText.color = Color.green;
            SoundManager.instance.PlaySound(SoundManager.instance.serve);
        }
        else
        {
            int temp = (int)Mathf.Floor((float)orderValue / 2f);
            money -= temp;
            fallingMoneyText.text = "-" + temp.ToString();
            fallingMoneyText.color = Color.red;
            SoundManager.instance.PlaySound(SoundManager.instance.wrongOrder);
        }

        fallingMoneyAnimator.Play("Money Anim");

        UpdateMoney();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PauseAndResume();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartNewDay();
        }
#endif
    }

    public void PauseAndResume()
    {
        isPlaying = !isPlaying;
        pauseScreen.SetActive(!pauseScreen.activeInHierarchy);
    }

    private IEnumerator DayTimer()
    {
        timer = maxTimer;
        AddOrder();
        while (timer > 0f)
        {
            if (isPlaying)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();

                timerClock.fillAmount = timer / maxTimer;

                float timeLeftInCurrentInterval = timer % orderInterval;
                if (timeLeftInCurrentInterval > 0f && timeLeftInCurrentInterval < Time.deltaTime)
                    AddOrder();

            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        EndDay();
        dayTimer = null;
    }

#region Order Functions
    private void AddOrder()
    {
        int r = Random.Range(0, orderOptionsDefault.Count);
        CreateOrder(orderOptionsDefault[r]);
    }

    private void CreateOrder(SO_BobaOrder bo)
    {
        // create a game object and grab the slot for sprites to be added to
        Order go = Instantiate(orderPrefab).GetComponent<Order>();
        go.Initialize(bo);
        GameObject slot = go.GetComponent<Order>().GetSpriteSlot();

        // create another go to hold image component for each sprite
        GameObject temp1 = Instantiate(orderEmptySprite);
        temp1.GetComponent<Image>().sprite = bo.GetCupSprite();

        GameObject temp2 = Instantiate(orderEmptySprite);
        if (bo.baseType == "Tea" && bo.mixer == MixerType.Juice)
        {
            temp2.GetComponent<Image>().sprite = bo.GetTeaJuiceSprite();
        }
        else
        {
            temp2.GetComponent<Image>().sprite = bo.GetMixerSprite();
        }

        GameObject temp3 = Instantiate(orderEmptySprite);
        temp3.GetComponent<Image>().sprite = bo.GetSolidsSprite();

        GameObject temp4 = Instantiate(orderEmptySprite);
        temp4.GetComponent<Image>().sprite = bo.GetIceSprite();

        // add go's with images to slot
        // if template images are left blank dont add the GO
        if (bo.GetCupSprite() != null)
            temp1.transform.parent = slot.transform;
        if (bo.GetMixerSprite() != null)
            temp2.transform.parent = slot.transform;
        if (bo.GetSolidsSprite() != null)
            temp3.transform.parent = slot.transform;
        if (bo.GetIceSprite() != null)
            temp4.transform.parent = slot.transform;

        // add completed order to bar
        go.transform.parent = ordersBar.transform;
        
        activeOrders.Add(go);
    }

    private bool CompareOrders(BobaController bc)
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            SO_BobaOrder tmp = activeOrders[i].GetBobaOrderSO();
            if (tmp.mixer == bc.mixer && 
                tmp.solids == bc.solids &&
                tmp.ice == bc.ice &&
                tmp.baseType == bc.baseType)
            {
                RemoveOrder(activeOrders[i]);
                return true;
            }
        }
        
        return false;
    }

    public void RemoveOrder(Order o)
    {
        if (activeOrders.Contains(o))
        {
            activeOrders.Remove(o);
            Destroy(o.gameObject);
        }
    }

    private void ResetOrders()
    {
        for (int i = 0; i < activeOrders.Count; i++)
        {
            Destroy(activeOrders[i].gameObject);
        }
        activeOrders.Clear();
    }
#endregion
}

[System.Serializable]
public class DayData
{
    public int dayNumber;
    public UnityEvent newEffects;
}
