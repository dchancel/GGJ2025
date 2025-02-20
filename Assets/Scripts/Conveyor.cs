using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Conveyor : MonoBehaviour
{
    public Conveyor nextConveyor;
    public BobaController held;
    public bool isServingWindow;
    public bool isStopGap;

    public List<Transform> wheels = new List<Transform>();
    public bool movingLeft;

    public SpriteRenderer conveyorArt;
    public List<Sprite> animSprite = new List<Sprite>();

    private bool holding = false;
    private Coroutine sendRoutine;

    private Coroutine tickRoutine;
    private Coroutine shakeRoutine;

    private float wheelSpeed = -200f;

    private int animationIndex = 0;
    private int framesRemaining = 0;
    private int animationFrames = 60;

    private void Start()
    {
        if (tickRoutine == null)
        {
            tickRoutine = StartCoroutine(Tick());
        }
        if (movingLeft)
        {
            wheelSpeed = -wheelSpeed;
        }
    }

    public bool IsAvailable()
    {
        return !holding;
    }

    public void DoServe()
    {
        //check bobacontroller contents against active orders
        GameManager.instance.DeliverOrder(held);
        if(sendRoutine != null)
        {
            StopCoroutine(sendRoutine);
        }
        Destroy(held.gameObject);
        held = null;
        holding = false;
    }

    public void AddHeld(BobaController bc)
    {
        held = bc;
        holding = true;
        //held.transform.position = transform.position;
        if (isServingWindow)
        {
            DoServe();
        }
    }

    private IEnumerator Tick()
    {
        while (true)
        {
            if (GameManager.instance.isPlaying)
            {
                if(held == null)
                {
                    holding = false;
                }

                if (!isServingWindow && !isStopGap)
                {
                    if (nextConveyor.IsAvailable() && held != null && sendRoutine == null && held.transform.position == transform.position)
                    {
                        sendRoutine = StartCoroutine(SendObject());
                    }
                    if(nextConveyor.IsAvailable() || sendRoutine != null)
                    {
                        AnimateConveyor();
                    }
                }
                else if (!isStopGap)
                {
                    AnimateConveyor();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SendObject()
    {
        BobaController temp = held;
        holding = true;
        float t = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = nextConveyor.transform.position;

        while(t < GameManager.instance.conveyorMoveSpeed)
        {
            if (GameManager.instance.isPlaying)
            {
                //Do the actual conveyor stuff
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
                if (temp == null)
                {
                    t = GameManager.instance.conveyorMoveSpeed;
                }
                else
                {

                    temp.transform.position = Vector3.Lerp(startPos, endPos, t / GameManager.instance.conveyorMoveSpeed);

                    if (t >= GameManager.instance.conveyorMoveSpeed / 2f)
                    {
                        if (held == temp)
                        {
                            holding = false;
                            held = null;
                            nextConveyor.AddHeld(temp);
                        }
                    }
                }
            }
            else
            {
                //game is paused. just loop until it isn't
                yield return new WaitForEndOfFrame();
            }
        }

        sendRoutine = null;
    }

    private void AnimateConveyor()
    {
        //occurs every frame
        for(int i = 0; i < wheels.Count; i++)
        {
            wheels[i].Rotate(Vector3.forward, wheelSpeed * (1f/GameManager.instance.conveyorMoveSpeed) * Time.deltaTime);
        }

        framesRemaining--;

        if(framesRemaining < 0)
        {
            animationIndex++;
            if(animationIndex > animSprite.Count - 1)
            {
                animationIndex = 0;
            }

            conveyorArt.sprite = animSprite[animationIndex];

            framesRemaining = Mathf.FloorToInt(animationFrames * (1f / GameManager.instance.conveyorMoveSpeed));
        }
    }

    public void AddCup(GameObject bc)
    {
        if (held == null && GameManager.instance.rsc_cup.resourcesAvailable > 0)
        {
            GameObject go = Instantiate(bc);
            go.transform.position = transform.position;
            AddHeld(go.GetComponent<BobaController>());

            GameManager.instance.rsc_cup.SpendResource();
        }
    }

    public void AddIce()
    {
        if(held != null && GameManager.instance.rsc_ice.resourcesAvailable > 0)
        {
            //Check if the container already has ice AND that ice is available

            //If not, add ice to it
            held.ReceiveIce();
            GameManager.instance.rsc_ice.SpendResource();
        }
    }

    public void AddTea()
    {
        if (held != null && GameManager.instance.rsc_tea.resourcesAvailable > 0)
        {
            //Check if the container already has tea AND that tea is available

            //If not, add tea to it
            held.ReceiveTea();
            GameManager.instance.rsc_tea.SpendResource();
        }
    }

    public void AddMilk()
    {
        if (held != null && GameManager.instance.rsc_milk.resourcesAvailable > 0)
        {
            //Check if the container already has milk AND that milk is available

            //If not, add milk to it
            held.ReceiveMilk();
            GameManager.instance.rsc_milk.SpendResource();
        }
    }

    public void AddJuice()
    {
        if (held != null && GameManager.instance.rsc_juice.resourcesAvailable > 0)
        {
            //Check if the container already has juice AND that juice is available

            //If not, add juice to it
            held.ReceiveJuice();
            GameManager.instance.rsc_juice.SpendResource();
        }
    }

    public void AddTapioca()
    {
        if (held != null && GameManager.instance.rsc_tapioca.resourcesAvailable > 0)
        {
            //Check if the container already has tapioca AND that tapioca is available

            //If not, add tapioca to it
            held.ReceiveTapioca();
            GameManager.instance.rsc_tapioca.SpendResource();
        }
    }

    public void AddFruit()
    {
        if (held != null && GameManager.instance.rsc_fruit.resourcesAvailable > 0)
        {
            //Check if the container already has fruit AND that fruit is available

            //If not, add fruit to it
            held.ReceiveFruit();
            GameManager.instance.rsc_fruit.SpendResource();
        }
    }

    public void DoShake(Conveyor outplace)
    {
        if (held != null)
        {
            //Take the container, shake it, and deposit it onto another conveyor belt upon completion
            if(shakeRoutine == null)
            {
                held.transform.position = outplace.transform.position;
                shakeRoutine = StartCoroutine(ShakeRoutine(outplace,held));
                held = null;
                holding = false;
            }
        }
    }

    private IEnumerator ShakeRoutine(Conveyor outplace, BobaController bc)
    {
        float t = 0f;
        bc.gameObject.SetActive(false);
        ShakerAnimator.instance.StartShake();
        SoundManager.instance.PlaySound(SoundManager.instance.shake);
        while (t < GameManager.instance.shakeTime)
        {
            if (GameManager.instance.isPlaying)
            {
                t += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
        if (bc != null)
        {
            bc.transform.position = outplace.transform.position;
            bc.gameObject.SetActive(true);
            bc.ReceiveShake();
            outplace.AddHeld(bc);
        }
        shakeRoutine = null;
        ShakerAnimator.instance.EndShake();
    }

    public void DoTrash()
    {
        if(held != null)
        {
            //put the whole thing into the trash
            Destroy(held.gameObject);
            held = null;
            holding = false;
        }
    }
}
