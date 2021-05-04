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
    public float max_time = 30f;
    public float fade_time = 5f;
    bool fading = false;
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
                if (timer > max_time || MusicPlayer.GetSongLength() - MusicPlayer.GetAudioPosSec() < 0.02f)
                {
                    List<int> itemlist = new List<int>();
                    for (int i = 0; i < MusicWheelBase.GetWheelItemCount(); i++)
                    {
                        MusicWheelItem item_t = GameObject.Find("WheelItem" + string.Format("{0}", i)).GetComponent<MusicWheelItem>();
                        if (!item_t.played)
                        {
                            itemlist.Add(i);
                        }
                    }
                    if (itemlist.Count == 0)
                    {
                        //放完了
                        timer = 0f;
                        fading = false;
                        MusicPlayer.SetVolume(GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>().value);
                        FullAuto = !FullAuto;
                        GameObject.Find("PlayButton").GetComponent<PlayButton>().ButtonOnClickEvent();
                    }
                    else
                    {
                        timer = 0f;
                        fading = false;
                        GameObject.Find("RandomNextButton").GetComponent<RandomNext>().ButtonOnClickEvent();
                        //MusicPlayer.SetVolume(volume);
                        MusicPlayer.SetVolume(GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>().value);

                        //GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>().value = volume;
                        GameObject.Find("PlayButton").GetComponent<PlayButton>().ButtonOnClickEvent();
                    }

                }
                if (!fading)
                {
                    //volume = MusicPlayer.GetVolume();
                }
                if (timer > max_time- fade_time)
                {

                    fading = true;

                    //MusicPlayer.SetVolume(volume * (max_time - timer) / (max_time-fade_time));
                    MusicPlayer.SetVolume(GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>().value * (max_time - timer) / (max_time - fade_time));
                    // GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>().value = volume * (max_time - timer) / (max_time - fade_time);
                }
                /*
                else
                {
                    volume = MusicPlayer.GetVolume();
                }*/
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
