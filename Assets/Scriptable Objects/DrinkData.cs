using JetBrains.Annotations;
using UnityEngine;

public enum MixerType
{
    None,
    Milk,
    Juice
}

public enum SolidsType
{
    None,
    Boba,
    Fruit,
    Both
}

[System.Serializable]
public class CupSpriteMapping
{
    public Sprite sprite;
}

[System.Serializable]
public class MixerSpriteMapping
{
    public MixerType mixerType;
    public Sprite sprite;
}

[System.Serializable]
public class SolidsSpriteMapping
{
    public SolidsType solidsType;
    public Sprite sprite;
}

[System.Serializable]
public class IceSpriteMapping
{
    public Sprite sprite;
}
