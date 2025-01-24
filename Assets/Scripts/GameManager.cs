using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timer;
    public float maxTimer;

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
    }

    public void GameFailed()
    {
        //call this if an order gets unfulfilled for long enough that you run out of time

        if(dayTimer != null)
        {
            StopCoroutine(dayTimer);
        }
    }

    public void DeliverOrder()
    {
        //you delivered an order that somebody was asking for
        money += orderValue;
    }

    public void WrongOrder()
    {
        //you have delivered an order that nobody was asking for
        money -= orderValue;
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
