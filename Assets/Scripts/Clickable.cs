using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Clickable : MonoBehaviour
{
    [SerializeField] public UnityEvent OnClick;
    [SerializeField] public Animator anim;

    public void TakeClick()
    {
        OnClick.Invoke();
    }

    public void PrintMessage(string s)
    {
        Debug.Log(s);
    }

    public void DoAnimation()
    {
        if(anim == null)
        {
            return;
        }
        StartCoroutine(AnimRoutine());
    }

    private IEnumerator AnimRoutine()
    {
        anim.SetTrigger("DoAnim");
        yield return new WaitForEndOfFrame();
        anim.ResetTrigger("DoAnim");
    }
}
