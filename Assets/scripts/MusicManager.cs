using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio;
using NAudio.Wave;

public class MusicManager : MonoBehaviour
{

    private void Awake()
    {

    }
    //private AudioSource bgAudioSource;

    void Start()
    {
        Screen.fullScreen = false;
        MusicLoader.InitMusicLoader();
        //bgAudioSource = gameObject.AddComponent<AudioSource>();
        //bgAudioSource.playOnAwake = false;
        //bgAudioSource.hideFlags = HideFlags.HideInHierarchy;
        //bgAudioSource.transform.SetParent(this.transform);

    }
    void Update()
    {

    }
}