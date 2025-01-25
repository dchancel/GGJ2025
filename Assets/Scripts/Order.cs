using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject spritesSlot;
    [SerializeField] private Slider slider;
    [SerializeField] private Image fillImage;

    [Header("Settings")]
    [SerializeField] private float timeAllowed;
    [SerializeField] private Color fullColor;
    [SerializeField] private Color middleColor;
    [SerializeField] private Color emptyColor;
    private float timeElapsed;

    public GameObject GetSpriteSlot() { return spritesSlot; }

    private void Update()
    {
        SliderColor();
    }

    private void SliderColor()
    {
        timeElapsed += Time.deltaTime;
        float normalizedTime = (timeElapsed % timeAllowed) / timeAllowed;

        // Calculate the segment (first or second half)
        Color lerpedColor;
        if (normalizedTime <= 0.5f)
        {
            float t = normalizedTime / 0.5f; // Normalize time to 0-1
            lerpedColor = Color.Lerp(fullColor, middleColor, t);
        }
        else
        {
            float t = (normalizedTime - 0.5f) / 0.5f; // Normalize time to 0-1
            lerpedColor = Color.Lerp(middleColor, emptyColor, t);
        }

        // Apply the interpolated color and value
        float p = 1 - timeElapsed / timeAllowed;
        slider.value = p;
        fillImage.color = lerpedColor;

        if (timeElapsed > timeAllowed)
        {
            Debug.LogError("hello, the order expired please add code in Order.cs");
        }
    }
}
