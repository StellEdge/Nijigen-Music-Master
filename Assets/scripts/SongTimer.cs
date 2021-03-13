using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            GameObject.Find("SongTimeText").GetComponent<Text>().text = LanguageManager.ConvToTime(MusicPlayer.GetAudioPos() * MusicPlayer.GetSongLength()) +
                "/" + LanguageManager.ConvToTime(MusicPlayer.GetSongLength());
        }
    }
}
