using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Float Controls")]
    [SerializeField] private float hoverSpeed = 2.0f;
    [SerializeField] private float hoverAmp = 0.1f;
    private float startY;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float drag = 2.0f;
    [SerializeField] private float maxVelocity = 10.0f;
    [SerializeField] private Vector2 velocity;
    private Vector2 targetPosition;
    private bool hasTarget = false;

    [Header("Animation")]
    [SerializeField] private Animator animController;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        startY = transform.position.y;
        return;
    }

    private void Update()
    {
        MouseInput();
        MoveTo();
        Floating();
        FlipSprite();
    }

    private void MouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Get the mouse position in screen space and convert to world space
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Use only the X and Y components for 2D movement
            targetPosition = new Vector2(mouseWorldPosition.x, mouseWorldPosition.y);

            hasTarget = true;
        }
    }

    private void MoveTo()
    {
        if (hasTarget)
        {
            Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;

            // Apply force in the direction of the target
            velocity += direction * moveSpeed * Time.deltaTime;

            // Apply drag to simulate water resistance
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);

            velocity = Vector2.ClampMagnitude(velocity, maxVelocity);

            transform.position = new Vector2(
                transform.position.x + velocity.x * Time.deltaTime,
                transform.position.y + velocity.y * Time.deltaTime
            );

            // Check if the player is close to the target and stop if close enough
            if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
            {
                startY = transform.position.y;
                hasTarget = false;
                velocity = Vector2.zero;
            }
        }
        else
        {
            // Apply drag to stop completely when no target
            velocity = Vector2.Lerp(velocity, Vector2.zero, drag * Time.deltaTime);
        }
    }

    protected void Floating()
    {
        if (!hasTarget)
        {
            float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmp;
            transform.position = new Vector2(transform.position.x, newY);
        }
    }

    protected void FlipSprite()
    {
        if (velocity.x > 0)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;
    }
}
