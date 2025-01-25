using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timer;
    public float maxTimer;

    public TextMeshProUGUI moneyDisplay;

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

    public Image timerClock;

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
        dayTimer = StartCoroutine(DayTimer());
    }

    private void EndDay()
    {
        //do end of day stuff here, like upgrades menu
        endOfDay.SetActive(true);
    }

    public void GameFailed()
    {
        //call this if an order gets unfulfilled for long enough that you run out of time

        if(dayTimer != null)
        {
            StopCoroutine(dayTimer);
        }
    }

    public void DeliverOrder(BobaController bc)
    {
        //if bc == requested order, make money
        money += orderValue;
        //else if bc != requested order, lose money
        //money -= (int)Mathf.Floor((float)orderValue / 2f);
        UpdateMoney();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlaying = !isPlaying;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartNewDay();
        }
#endif
    }

    private IEnumerator DayTimer()
    {
        timer = maxTimer;
        while (timer > 0f)
        {
            if (isPlaying)
            {
                timer -= Time.deltaTime;
                yield return new WaitForEndOfFrame();

                timerClock.fillAmount = timer / maxTimer;

            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }
        EndDay();
        dayTimer = null;
    }
}
