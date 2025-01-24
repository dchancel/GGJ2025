using UnityEngine;

[CreateAssetMenu(fileName = "SO_OrderTemplate", menuName = "Scriptable Objects/Drinks/Order Template")]
public class SO_OrderTemplate : ScriptableObject
{
    public CupSpriteMapping cupSprite;
    public MixerSpriteMapping[] mixerSprites;
    public SolidsSpriteMapping[] solidsSprites;
    public IceSpriteMapping iceSprite;

    public Sprite GetCupSprite()
    {
        return cupSprite.sprite;
    }

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

    public Sprite GetIceSprite()
    {
        return iceSprite.sprite;
    }
}
