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

    public void ReceiveIce()
    {
        ice = true;
    }

    public void ReceiveTea()
    {
        baseType = "Tea";
    }

    public void ReceiveMilk()
    {
        if (mixer != MixerType.None)
        {
            return;
        }
        mixer = MixerType.Milk;
    }

    public void ReceiveJuice()
    {
        if(mixer != MixerType.None)
        {
            return;
        }
        mixer = MixerType.Juice;
    }

    public void ReceiveTapioca()
    {
        if(solids != SolidsType.None)
        {
            solids = SolidsType.Boba;
        }
        if(solids == SolidsType.Fruit)
        {
            solids = SolidsType.Both;
        }
    }

    public void ReceiveFruit()
    {
        if (solids != SolidsType.None)
        {
            solids = SolidsType.Fruit;
        }
        if (solids == SolidsType.Boba)
        {
            solids = SolidsType.Both;
        }
    }

    public void ReceiveShake()
    {
        shaken = true;
        lid = true;
    }
}
