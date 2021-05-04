using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RandomNext : MonoBehaviour
{
    // Start is called before the first frame update
    private Button m_Button;
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ButtonOnClickEvent();
        }
    }
    public void ButtonOnClickEvent()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            Debug.Log("Paused");
            MusicPlayer.PauseAudio();
        }
        List<int> itemlist = new List<int>();
        for(int i = 0;i< MusicWheelBase.GetWheelItemCount(); i++)
        {
            MusicWheelItem item_t = GameObject.Find("WheelItem" + string.Format("{0}", i)).GetComponent<MusicWheelItem>();
            if (!item_t.played)
            {
                itemlist.Add(i);
            }
        }
        if (itemlist.Count == 0) return;
        //Random.InitState(1);
        int index = Random.Range(0,itemlist.Count);
        GameObject.Find("RedoRandomButton").GetComponent<RedoRandom>().AddRandom(itemlist[index]);
        MusicWheelItem item = GameObject.Find("WheelItem"+string.Format("{0}",itemlist[index])).GetComponent<MusicWheelItem>();

        float t = itemlist[index] * 1.0f / (MusicWheelBase.GetWheelItemCount() - 1);
        MusicWheelBase.UpdateWheelPos(t);
        Scrollbar m_Scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        m_Scrollbar.value = t;
        item.ButtonClicked();

        float pos = Random.Range(0f, 0.8f);
        if (GameObject.Find("LimTVSizeButton").GetComponent<LimitTVSize>().LimitTvSize)
        {
            float songlen = MusicPlayer.GetSongLength();
            float sec = Random.Range(0f, 80f);
            sec = Mathf.Min(songlen - 15f, sec);
            pos = sec / songlen;
        }
        
        GameObject.Find("SongProgressBar").GetComponent<SongProgressbar>().SetProgress(pos);
    }
}