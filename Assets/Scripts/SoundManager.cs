using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioClip newOrder;
    public AudioClip addCup;
    public AudioClip addIce;
    public AudioClip addMilk;
    public AudioClip addTea;
    public AudioClip addJuice;
    public AudioClip addBoba;
    public AudioClip addFruit;
    public AudioClip shake;
    public AudioClip serve;

    private AudioSource aud;

    private void OnEnable()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        aud = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip ac)
    {
        aud.pitch = Random.Range(0.9f, 1.4f);
        aud.PlayOneShot(ac);
    }

    public void SetVolume(float f)
    {
        aud.volume = f;
    }
}
