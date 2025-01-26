using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> song = new List<AudioClip>();
    int index = -1;

    private AudioSource aud;

    private void Start()
    {
        aud = GetComponent<AudioSource>();
        PickSong();
    }
    private void PickSong()
    {
        int temp = index;
        while(temp == index)
        {
            temp = Random.Range(0, song.Count);
        }
        index = temp;
        aud.PlayOneShot(song[index]);
    }

    private void Update()
    {
        if (!aud.isPlaying)
        {
            PickSong();
        }
    }
}
