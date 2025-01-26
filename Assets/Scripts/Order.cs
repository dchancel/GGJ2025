using UnityEngine;
using UnityEngine.UI;

public class Order : MonoBehaviour
{
    private SO_BobaOrder soBobaOrder; // for comparison when completing orders

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
    public SO_BobaOrder GetBobaOrderSO() { return soBobaOrder; }

    private void Update()
    {
        SliderColor();
    }

    public void Initialize(SO_BobaOrder bo)
    {
        soBobaOrder = bo;
    }

    private void SliderColor()
    {
        if (!GameManager.instance.isPlaying)
            return;

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
            GameManager.instance.GameFailed();
            GameManager.instance.RemoveOrder(this);
        }
    }
}
