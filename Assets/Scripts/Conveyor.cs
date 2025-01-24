using UnityEngine;
using System.Collections;

public class Conveyor : MonoBehaviour
{
    public Conveyor nextConveyor;
    public BobaController held;
    public bool isServingWindow;

    private bool holding = false;
    private Coroutine sendRoutine;

    private Coroutine tickRoutine;

    private void Start()
    {
        if (tickRoutine == null)
        {
            tickRoutine = StartCoroutine(Tick());
        }
    }

    public bool IsAvailable()
    {
        return !holding;
    }

    public bool DoServe()
    {
        return false;
        //check bobacontroller contents against active orders

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
                if (!isServingWindow)
                {
                    if (nextConveyor.IsAvailable() && held != null && sendRoutine == null && held.transform.position == transform.position)
                    {
                        sendRoutine = StartCoroutine(SendObject());
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator SendObject()
    {
        yield return null;
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
                temp.transform.position = Vector3.Lerp(startPos,endPos,t/GameManager.instance.conveyorMoveSpeed);

                if(t >= GameManager.instance.conveyorMoveSpeed / 2f)
                {
                    if(held == temp)
                    {
                        holding = false;
                        held = null;
                        nextConveyor.AddHeld(temp);
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

    public void AddCup(GameObject bc)
    {
        if (held == null)
        {
            GameObject go = Instantiate(bc);
            go.transform.position = transform.position;
            AddHeld(go.GetComponent<BobaController>());
        }
    }
}
