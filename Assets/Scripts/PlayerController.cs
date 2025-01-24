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
            GameObject go = Instantiate(instantiator);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(clickPosition);
            worldPosition.z = 0f;
            go.transform.position = worldPosition;
            clickCooldownRoutine = StartCoroutine(ClickCooldown());
        }
    }

    private IEnumerator ClickCooldown()
    {
        yield return new WaitForSeconds(clickCooldown);
        clickCooldownRoutine = null;
    }
}
