using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private AudioSource aSource;
    void Awake()
    {
        //Get and store a reference to the following attached components:  
        //AudioSource  
        this.aSource = GetComponent<AudioSource>();
        MusicPlayer.InitMusicPlayer(aSource);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
