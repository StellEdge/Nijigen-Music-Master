using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GuessTimer : MonoBehaviour
{
    // Start is called before the first frame update
    private float sec;
    void Start()
    {
        sec = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            sec = sec + Time.deltaTime;
            GameObject.Find("GuessTimerText").GetComponent<Text>().text = LanguageManager.ConvToTimeMili(sec);
        }
        else
        {
            sec = 0f;
        }
    }
}
