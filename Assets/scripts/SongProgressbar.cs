using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongProgressbar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (MusicPlayer.GetIsPlaying())
        {
            m_Scrollbar.value = MusicPlayer.GetAudioPos();
        }
        else
        {

        }
            
    }
    private Scrollbar m_Scrollbar;
    void Start()
    {
        m_Scrollbar = GameObject.Find("SongProgressBar").GetComponent<Scrollbar>();
        //使用监听事件需要先持有组件的引用
        m_Scrollbar.onValueChanged.AddListener(OnValueChangedPrivate);
    }
    //方法
    private void OnValueChangedPrivate(float T)
    {
        MusicPlayer.PlayAudioAtPos(T);
        if (!MusicPlayer.GetIsPlaying())
        {
            GameObject.Find("SongTimeText").GetComponent<Text>().text = LanguageManager.ConvToTime(T * MusicPlayer.GetSongLength()) +
                "/" + LanguageManager.ConvToTime(MusicPlayer.GetSongLength());
        }
        //print("代码控制" + T);
    }

}
