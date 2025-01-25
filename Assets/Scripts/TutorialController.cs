using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private SO_TutorialData data;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI informationText;

    private int index = 0;

    private void OnEnable()
    {
        GameManager.instance.isPlaying = false;
    }

    public void NewTutorial(SO_TutorialData inputData)
    {
        data = inputData;
        index = 0;
        gameObject.SetActive(true);
        WriteData(index);
    }

    private void OnDisable()
    {
        GameManager.instance.isPlaying = true;
    }

    public void ContinueButton()
    {
        index++;
        if(index > data.d.Count - 1)
        {
            EndTutorial();
        }
        else
        {
            WriteData(index);
        }
    }

    private void WriteData(int i)
    {
        informationText.text = data.d[i].information;
        if(data.d[i].icon != null)
        {
            iconImage.sprite = data.d[i].icon;
            iconImage.gameObject.SetActive(true);
        }
        else
        {
            iconImage.gameObject.SetActive(false);
        }
    }

    private void EndTutorial()
    {
        gameObject.SetActive(false);
    }
}

[System.Serializable]
public class TutData
{
    public Sprite icon;
    public string information;
}
