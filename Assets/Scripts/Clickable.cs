using UnityEngine;
using UnityEngine.Events;

public class Clickable : MonoBehaviour
{
    [SerializeField] public UnityEvent OnClick;

    public void TakeClick()
    {
        OnClick.Invoke();
    }

    public void PrintMessage(string s)
    {
        Debug.Log(s);
    }
}
