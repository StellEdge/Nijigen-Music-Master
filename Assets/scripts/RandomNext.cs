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

        int index = Random.Range(0,itemlist.Count);
        MusicWheelItem item = GameObject.Find("WheelItem"+string.Format("{0}",itemlist[index])).GetComponent<MusicWheelItem>();
        item.ButtonClicked();

        float pos = Random.Range(0f, 0.8f);
        GameObject.Find("SongProgressBar").GetComponent<SongProgressbar>().SetProgress(pos);
    }
}