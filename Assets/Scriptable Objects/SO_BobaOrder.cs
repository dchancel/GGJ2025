using UnityEngine;

[CreateAssetMenu(fileName = "SO_BobaOrder", menuName = "Scriptable Objects/Drinks/Boba Order")]
public class SO_BobaOrder : ScriptableObject
{
    public SO_OrderTemplate baseTemplate;

    public bool cup = true;
    public string baseType = "Tea";

    public MixerType mixer;
    public SolidsType solids;

    public bool lid = true;
    public bool ice = true;
    public bool shaken = true;

    public Sprite GetCupSprite()
    {
        return baseTemplate?.GetCupSprite();
    }

    public Sprite GetMixerSprite()
    {
        return baseTemplate?.GetMixerSprite(mixer);
    }

    public Sprite GetSolidsSprite()
    {
        return baseTemplate?.GetSolidsSprite(solids);
    }

    public Sprite GetIceSprite()
    {
        return baseTemplate?.GetIceSprite();
    }

    public Sprite GetTeaJuiceSprite()
    {
        return baseTemplate.teaJuiceSprite;
    }
}