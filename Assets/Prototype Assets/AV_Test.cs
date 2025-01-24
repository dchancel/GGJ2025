using UnityEngine;
using UnityEngine.UI;

public class AV_Test : MonoBehaviour
{
    [SerializeField] private SO_BobaOrder orderA;
    [SerializeField] private SO_BobaOrder orderB;
    [SerializeField] private Image rend;
    public Sprite mixerSpriteA;
    public Sprite mixerSpriteB;

    void Awake()
    {
        mixerSpriteA = orderA.GetMixerSprite();
        mixerSpriteB = orderB.GetMixerSprite();
    }

    void Start()
    {
        rend.sprite = mixerSpriteB;
    }
}
