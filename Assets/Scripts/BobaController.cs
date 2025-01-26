using UnityEngine;

public class BobaController : MonoBehaviour
{
    [SerializeField] private Sprite baseLayer;

    public bool cup = true;
    public string baseType;

    public MixerType mixer;
    public SolidsType solids;

    public bool lid = false;
    public bool ice = false;
    public bool shaken = false;

    public GameObject iceLayer;
    public GameObject teaLayer;
    public GameObject milkLayer;
    public GameObject bobaLayer;
    public GameObject fruitLayer;
    public GameObject juiceLayer;
    public GameObject fruitmilkLayer;
    public GameObject milkteaLayer;
    public GameObject teajuiceLayer;
    public GameObject lidLayer;

    private void Start()
    {
        SoundManager.instance.PlaySound(SoundManager.instance.addCup);
        GameManager.instance.AddBobaController(this);
    }

    public void ReceiveIce()
    {
        ice = true;
        iceLayer.SetActive(true);

        SoundManager.instance.PlaySound(SoundManager.instance.addIce);
    }

    public void ReceiveTea()
    {
        baseType = "Tea";
        if (juiceLayer.activeInHierarchy)
        {
            juiceLayer.SetActive(false);
            teajuiceLayer.SetActive(true);
        }
        else if (milkLayer.activeInHierarchy)
        {
            milkLayer.SetActive(false);
            milkteaLayer.SetActive(true);
        }
        else
        {
            teaLayer.SetActive(true);
        }

        SoundManager.instance.PlaySound(SoundManager.instance.addTea);

    }

    public void ReceiveMilk()
    {
        if (mixer != MixerType.None)
        {
            return;
        }
        mixer = MixerType.Milk;
        if (teaLayer.activeInHierarchy)
        {
            teaLayer.SetActive(false);
            milkteaLayer.SetActive(true);
        }
        else if (fruitLayer.activeInHierarchy)
        {
            fruitLayer.SetActive(false);
            fruitmilkLayer.SetActive(true);
        }
        else {
            milkLayer.SetActive(true);
        }

        SoundManager.instance.PlaySound(SoundManager.instance.addMilk);
    }

    public void ReceiveJuice()
    {
        if(mixer != MixerType.None)
        {
            return;
        }
        mixer = MixerType.Juice;
        if (teaLayer.activeInHierarchy)
        {
            teaLayer.SetActive(false);
            teajuiceLayer.SetActive(true);
        }
        else
        {
            juiceLayer.SetActive(true);
        }

        SoundManager.instance.PlaySound(SoundManager.instance.addJuice);
    }

    public void ReceiveTapioca()
    {
        if(solids == SolidsType.None)
        {
            solids = SolidsType.Boba;
        }
        if(solids == SolidsType.Fruit)
        {
            solids = SolidsType.Both;
        }

        bobaLayer.SetActive(true);
        SoundManager.instance.PlaySound(SoundManager.instance.addBoba);
    }

    public void ReceiveFruit()
    {
        if (solids == SolidsType.None)
        {
            solids = SolidsType.Fruit;
        }
        if (solids == SolidsType.Boba)
        {
            solids = SolidsType.Both;
        }

        fruitLayer.SetActive(true);
        SoundManager.instance.PlaySound(SoundManager.instance.addFruit);
    }

    public void ReceiveShake()
    {
        shaken = true;
        lid = true;
        SoundManager.instance.PlaySound(SoundManager.instance.shake);
        lidLayer.SetActive(true);
    }
}
