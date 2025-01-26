using System.Collections;
using TMPro;
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
    [SerializeField] private RectTransform shakeable;
    [SerializeField] private float shakeAmount;
    [SerializeField] private float shakeHighAmount;
    private Vector3 textStartPos;
    private float timeElapsed;

    public GameObject GetSpriteSlot() { return spritesSlot; }
    public SO_BobaOrder GetBobaOrderSO() { return soBobaOrder; }

    private void Awake()
    {
        textStartPos = shakeable.localPosition;
    }

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

        if (timeAllowed - timeElapsed < 10)
        {
            if (timeAllowed - timeElapsed < 5)
            {
                shakeAmount = shakeHighAmount;
            }
            StartCoroutine(Shake());
        }

        if (timeElapsed > timeAllowed)
        {
            GameManager.instance.GameFailed();
            GameManager.instance.RemoveOrder(this);
        }
    }

    private IEnumerator Shake()
    {
        while (true)
        {
            // Calculate random shake offset
            float shakeX = Mathf.Sin(Time.time * 20f) * shakeAmount;
            float shakeY = Mathf.Cos(Time.time * 20f) * shakeAmount;

            // Apply the shake to the text's position
            shakeable.localPosition = new Vector3(textStartPos.x + shakeX, textStartPos.y + shakeY, textStartPos.z);

            //timeElapsed += Time.deltaTime;

            // Wait until next frame
            yield return null;
        }

        // Reset position after shaking
        //uiText.rectTransform.localPosition = originalPosition;
    }
}
