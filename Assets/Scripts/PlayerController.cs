using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float clickCooldown = 0.05f;
    [SerializeField] private GameObject instantiator;

    private Vector2 clickPosition;
    private Coroutine clickCooldownRoutine;

    public void OnClick(InputValue c)
    {
        if (c.isPressed)
        {
            DoClick();
        }
    }

    public void OnClickPosition(InputValue c)
    {
        clickPosition = c.Get<Vector2>();
    }

    private void DoClick()
    {
        if (clickCooldownRoutine == null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector3.forward);

            if (hit)
            {
                if (hit.transform.GetComponent<Clickable>())
                {
                    hit.transform.GetComponent<Clickable>().TakeClick();
                }
            }

            //GameObject go = Instantiate(instantiator);
            //go.transform.position = worldPosition;
            clickCooldownRoutine = StartCoroutine(ClickCooldown());
        }
    }

    private IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(clickCooldown);
        clickCooldownRoutine = null;
    }
}
