using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioClip[] music;       //0->InitialDialog|1->Player Turn|2->Enemy Turn|3->Battle|4->Victory|5->Defeat
    public AudioSource mSource;

    public void playMusic(int option)
    {
        mSource.clip = music[option];
        mSource.Play();
    }
}
