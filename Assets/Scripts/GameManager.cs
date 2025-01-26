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
    public SO_BobaOrder[] orderOptions;
    public GameObject ordersBar;
    public float orderInterval;
    public float orderTime;
    [HideInInspector] public bool orderIsInfinite;
    public GameObject orderPrefab;
    public GameObject orderEmptySprite;
    public List<Order> activeOrders;

    [Header("UI")]
    public TextMeshProUGUI moneyDisplay;
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

        orderTime *= 0.9f;
        orderTime = Mathf.Clamp(orderTime,30f, 120f);

        orderInterval *= 0.8f;
        orderInterval = Mathf.Clamp(orderInterval, 5f, 100f);

        for(int i = 0; i < dayUpgrades.Count; i++)
        {
            if(day == dayUpgrades[i].dayNumber)
            {
                dayUpgrades[i].newEffects.Invoke();
            }
        }
        dayTimer = StartCoroutine(DayTimer());

        dayTimeDisplay.text = "Time left in day " + day + ":";
    }

    private void EndDay()
    {
        //do end of day stuff here, like upgrades menu
        endOfDay.SetActive(true);
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
        }
        else
        {
            money -= (int)Mathf.Floor((float)orderValue / 2f);
        }
        
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
        int r = Random.Range(0, orderOptions.Length);
        CreateOrder(orderOptions[r]);
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
        temp2.GetComponent<Image>().sprite = bo.GetMixerSprite();

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
#endregion
}

[System.Serializable]
public class DayData
{
    public int dayNumber;
    public UnityEvent newEffects;
}
