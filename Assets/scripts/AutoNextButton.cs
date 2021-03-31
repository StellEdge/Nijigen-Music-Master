using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AutoNextButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button m_Button;
    public bool AutoNext;
    private Sprite selected, normal;
    void Start()
    {
        AutoNext = false;
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        selected = Resources.Load<Sprite>("autonext_selected");
        normal = Resources.Load<Sprite>("autonext_normal");
    }
    // Update is called once per frame
    void Update()
    {
        if (AutoNext)
        {
            m_Button.GetComponent<Image>().sprite = selected;
            if (MusicPlayer.GetSongLength()-MusicPlayer.GetAudioPosSec()<0.02f)
            {
                int index = MusicWheelBase.GetCurSongIndex() +1;
                if (index >= MusicWheelBase.GetWheelItemCount()) index = 0;
                MusicWheelItem item = GameObject.Find("WheelItem" + string.Format("{0}", index)).GetComponent<MusicWheelItem>();

                MusicWheelBase.SetWheelPosIndex(index);
                item.ButtonClicked();
                GameObject.Find("SongProgressBar").GetComponent<SongProgressbar>().SetProgress(0f);
                GameObject.Find("PlayButton").GetComponent<PlayButton>().ButtonOnClickEvent();
            }
        }
        else
        {
            m_Button.GetComponent<Image>().sprite = normal;
        }
    }
    public void ButtonOnClickEvent()
    {
        AutoNext = !AutoNext;
    }
}
