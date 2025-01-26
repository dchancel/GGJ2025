using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class EndOfDay : MonoBehaviour
{
    public List<Upgrade> possibleUpgrades = new List<Upgrade>();

    private int firstIndex = -1;
    private int secondIndex = -1;
    private int thirdIndex = -1;

    public List<Button> upgradeButton = new List<Button>();
    public List<TextMeshProUGUI> upgradeText = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> upgradeCost = new List<TextMeshProUGUI>();

    private void OnEnable()
    {
        GameManager.instance.isPlaying = false;
    }

    private void OnDisable()
    {
        GameManager.instance.isPlaying = true;
    }

    public void ResetOptions()
    {
        DetermineAvailableUpgrades();
    }

    private void DetermineAvailableUpgrades()
    {
        firstIndex = -1;
        upgradeButton[0].interactable = true;
        secondIndex = -1;
        upgradeButton[1].interactable = true;
        thirdIndex = -1;
        upgradeButton[2].interactable = true;
        for (int i = 0; i < 3; i++)
        {
            int temp = Random.Range(0,possibleUpgrades.Count);
            while(temp == firstIndex || temp == secondIndex || possibleUpgrades[temp].firstDayAvailable > GameManager.instance.day)
            {
                temp = Random.Range(0, possibleUpgrades.Count);
            }
            switch (i)
            {
                case (0):
                    firstIndex = temp;
                    break;
                case (1):
                    secondIndex = temp;
                    break;
                case (2):
                    thirdIndex = temp;
                    break;
            }
        }

        //option 1 = firstindex
        upgradeText[0].text = possibleUpgrades[firstIndex].description;
        upgradeCost[0].text = "$" + possibleUpgrades[firstIndex].cost;
        if(possibleUpgrades[firstIndex].cost > GameManager.instance.money)
        {
            upgradeButton[0].interactable = false;
        }
        //option 2 = secondindex
        upgradeText[1].text = possibleUpgrades[secondIndex].description;
        upgradeCost[1].text = "$" + possibleUpgrades[secondIndex].cost;
        if (possibleUpgrades[secondIndex].cost > GameManager.instance.money)
        {
            upgradeButton[1].interactable = false;
        }
        //option 3 = thirdindex
        upgradeText[2].text = possibleUpgrades[thirdIndex].description;
        upgradeCost[2].text = "$" + possibleUpgrades[thirdIndex].cost;
        if (possibleUpgrades[thirdIndex].cost > GameManager.instance.money)
        {
            upgradeButton[2].interactable = false;
        }
    }

    public void SelectOption(int index)
    {
        //index 3 = skip upgrades
        if(index > 2)
        {
            //skipping
        }
        else
        {
            if(index == 0)
            {
                index = firstIndex;
            } else if(index == 1)
            {
                index = secondIndex;
            } else if(index == 2)
            {
                index = secondIndex;
            }


            switch (possibleUpgrades[index].identifier)
            {
                case ("ConveyorSpeed"):
                    GameManager.instance.conveyorMoveSpeed *= 0.9f;
                    break;
                case ("CupAmount"):
                    GameManager.instance.rsc_cup.maxResources += 3;
                    break;
                case ("CupTime"):
                    GameManager.instance.rsc_cup.refreshTime *= 0.75f;
                    break;
                case ("IceAmount"):
                    GameManager.instance.rsc_ice.maxResources += 3;
                    break;
                case ("IceTime"):
                    GameManager.instance.rsc_ice.refreshTime *= 0.75f;
                    break;
                case ("TeaAmount"):
                    GameManager.instance.rsc_tea.maxResources += 6;
                    break;
                case ("TeaTime"):
                    GameManager.instance.rsc_tea.refreshTime *= .8f;
                    break;
                case ("MilkAmount"):
                    GameManager.instance.rsc_milk.maxResources += 5;
                    break;
                case ("MilkTime"):
                    GameManager.instance.rsc_milk.refreshTime *= .8f;
                    break;
                case ("JuiceAmount"):
                    GameManager.instance.rsc_juice.maxResources += 5;
                    break;
                case ("JuiceTime"):
                    GameManager.instance.rsc_juice.refreshTime *= .8f;
                    break;
                case ("BobaAmount"):
                    GameManager.instance.rsc_tapioca.maxResources += 2;
                    break;
                case ("BobaTime"):
                    GameManager.instance.rsc_tapioca.refreshTime *= .75f;
                    break;
                case ("FruitAmount"):
                    GameManager.instance.rsc_fruit.maxResources += 1;
                    break;
                case ("FruitTime"):
                    GameManager.instance.rsc_fruit.refreshTime *= .5f;
                    break;
                case ("ShakeSpeed"):
                    GameManager.instance.shakeTime *= 0.95f;
                    break;
                case ("OrderValue"):
                    GameManager.instance.orderValue += 1;
                    break;
                case ("LongerDays"):
                    GameManager.instance.maxTimer *= 1.25f;
                    break;
            }
            GameManager.instance.money -= possibleUpgrades[index].cost;
            Debug.Log("Spending " + possibleUpgrades[index].cost);
            possibleUpgrades[index].cost += 5;
        }
        gameObject.SetActive(false);
        GameManager.instance.StartNewDay();
    }
}

[System.Serializable]
public class Upgrade
{
    public Sprite icon;
    public string description;
    public int cost;
    public int firstDayAvailable;
    public string identifier;
}
