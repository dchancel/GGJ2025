using UnityEngine;

[CreateAssetMenu(fileName = "SO_OrderTemplate", menuName = "Scriptable Objects/Drinks/Order Template")]
public class SO_OrderTemplate : ScriptableObject
{
    public MixerSpriteMapping[] mixerSprites;
    public SolidsSpriteMapping[] solidsSprites;
    public IceSpriteMapping[] iceSprites;

    public Sprite GetMixerSprite(MixerType mixer)
    {
        foreach (var mapping in mixerSprites)
        {
            if (mapping.mixerType == mixer)
                return mapping.sprite;
        }
        return null;
    }

    public Sprite GetSolidsSprite(SolidsType solids)
    {
        foreach (var mapping in solidsSprites)
        {
            if (mapping.solidsType == solids)
                return mapping.sprite;
        }
        return null;
    }

    public Sprite GetIceSprite(IceOption ice)
    {
        foreach (var mapping in iceSprites)
        {
            if (mapping.iceOption == ice)
                return mapping.sprite;
        }
        return null;
    }
}
