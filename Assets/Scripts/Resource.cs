using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Resource : Clickable
{
    public int resourcesAvailable = 0;
    public int maxResources = 10;
    public float refreshTime = 2f;

    [SerializeField] private Image refreshWheel;
    [SerializeField] private Image resourceDisplay;

    [SerializeField] public AudioClip soundEffect;

    private Color baseColor;
    private Coroutine refreshRoutine;
    private SpriteRenderer sr;

    private bool hasInitialized = false;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;
        FillResources();
    }

    public void SpendResource()
    {
        resourcesAvailable--;
        UpdateResourceDisplay();
    }

    private void UpdateResourceDisplay()
    {
        resourceDisplay.fillAmount = (float)resourcesAvailable / (float)maxResources;
    }

    private void FillResources()
    {
        resourcesAvailable = maxResources;
        refreshWheel.fillAmount = 0f;
        sr.color = baseColor;
        if (hasInitialized)
        {
            SoundManager.instance.PlaySound(soundEffect);
        }
        hasInitialized = true;
        UpdateResourceDisplay();
    }

    public void RefreshResource()
    {
        if(refreshRoutine == null)
        {
            refreshRoutine = StartCoroutine(RefreshRoutine());
        }
    }

    private IEnumerator RefreshRoutine()
    {
        float t = 0f;
        sr.color = Color.gray;
        while(t < refreshTime)
        {
            if (GameManager.instance.isPlaying)
            {
                t += Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();

            refreshWheel.fillAmount = t / refreshTime;

        }
        FillResources();


        refreshRoutine = null;
    }
}
