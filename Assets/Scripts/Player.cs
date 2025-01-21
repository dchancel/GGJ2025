using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Float Controls")]
    [SerializeField] private float hoverSpeed = 2.0f;
    [SerializeField] private float hoverAmp = 0.5f;
    private float startY;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float drag = 2.0f;
    [SerializeField] private float maxVelocity = 10.0f;
    private Vector3 velocity;
    private Vector3 inputDir;
    private bool isInputing = false;

    [Header("Animation")]
    [SerializeField] private Animator animController;

    private void Awake()
    {
        startY = transform.position.y;
        return;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        inputDir= new Vector3(h, v, 0).normalized;

        if (inputDir.magnitude > 0)
        {
            velocity += inputDir * moveSpeed * Time.deltaTime;

            isInputing = true;
            animController.speed = 2.0f;
        }
        else
        {
            // TODO: Add method for waiting until drag is over to prevent stutter
            if (isInputing)
            {
                startY = transform.position.y;
            }
            isInputing = false;
            Floating();
            animController.speed = 1.0f;
        }

        // Apply drag for water resistance
        velocity = Vector3.Lerp(velocity, Vector3.zero, drag * Time.deltaTime);

        // Clamp velocity
        velocity = Vector3.ClampMagnitude(velocity, maxVelocity);

        // Move
        transform.position += velocity * Time.deltaTime;
    }

    protected void Floating()
    {
        float newY = startY + Mathf.Sin(Time.time * hoverSpeed) * hoverAmp;
        transform.position = new Vector2(transform.position.x, newY);
    }
}
