using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button m_Button;
    private Sprite play, pause;
    //private bool keystate;
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        play = Resources.Load<Sprite>("play");
        pause = Resources.Load<Sprite>("pause");
    }
    void Update()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            m_Button.GetComponent<Image>().sprite = pause;
        }
        else
        {
            m_Button.GetComponent<Image>().sprite = play;
        }
        //bool curkey = Input.GetKey(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.C))//!keystate && curkey)
        {
            ButtonOnClickEvent();
        }
        //keystate = curkey;
    }
    public void ButtonOnClickEvent()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            Debug.Log("Paused");
            MusicPlayer.PauseAudio();
        }
        else
        {
            Debug.Log("UnPaused");
            //MusicPlayer.ResumeAudio();
            MusicPlayer.PlayAudio(MusicPlayer.GetCurAudio());
        }
        //
    }
}
