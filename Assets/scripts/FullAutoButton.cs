using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullAutoButton : MonoBehaviour
{
    private Button m_Button;
    public bool FullAuto;
    private Sprite selected, normal;
    private float timer;
    private float volume;
    void Start()
    {
        FullAuto = false;
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        selected = Resources.Load<Sprite>("fullauto_selected");
        normal = Resources.Load<Sprite>("fullauto_normal");
    }
    // Update is called once per frame
    void Update()
    {
        if (FullAuto)
        {
            m_Button.GetComponent<Image>().sprite = selected;
            if (MusicPlayer.GetIsPlaying())
            {
                timer += Time.deltaTime;
                if (timer > 30f || MusicPlayer.GetSongLength() - MusicPlayer.GetAudioPosSec() < 0.02f)
                {
                    timer = 0f;
                    GameObject.Find("RandomNextButton").GetComponent<RandomNext>().ButtonOnClickEvent();
                    MusicPlayer.SetVolume(volume);
                    GameObject.Find("PlayButton").GetComponent<PlayButton>().ButtonOnClickEvent();
                }

                if (timer > 25f)
                {
                    MusicPlayer.SetVolume(volume * (30f - timer) / 5);
                }
                else
                {
                    volume = MusicPlayer.GetVolume();
                }
            }
            else
            {
                timer = 0f;
            }
        }
        else
        {
            m_Button.GetComponent<Image>().sprite = normal;
        }
    }
    public void ButtonOnClickEvent()
    {
        FullAuto = !FullAuto;
        timer = 0;
        volume = MusicPlayer.GetVolume();
    }
}
