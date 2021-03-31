using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PrevButton : MonoBehaviour
{
    private Button m_Button;
    // Start is called before the first frame update
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonOnClickEvent()
    {

        int index = MusicWheelBase.GetCurSongIndex() - 1;
        if (index <0 ) index = MusicWheelBase.GetWheelItemCount() -1;
        MusicWheelItem item = GameObject.Find("WheelItem" + string.Format("{0}", index)).GetComponent<MusicWheelItem>();

        MusicWheelBase.SetWheelPosIndex(index);
        item.ButtonClicked();
        GameObject.Find("SongProgressBar").GetComponent<SongProgressbar>().SetProgress(0f);
        GameObject.Find("PlayButton").GetComponent<PlayButton>().ButtonOnClickEvent();
    }
}
