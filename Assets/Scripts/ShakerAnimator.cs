
using UnityEngine;

public class ShakerAnimator : MonoBehaviour
{
    public static ShakerAnimator instance;

    private Animator anim;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        anim = GetComponent<Animator>();
    }

    public void StartShake()
    {
        anim.SetTrigger("DoAnimation");
    }

    public void EndShake()
    {
        anim.ResetTrigger("DoAnimation");
    }
}
