using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongVolumebar : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

    }
    private Scrollbar m_Scrollbar;
    void Start()
    {
        m_Scrollbar = GameObject.Find("SongVolumeBar").GetComponent<Scrollbar>();
        //使用监听事件需要先持有组件的引用
        m_Scrollbar.onValueChanged.AddListener(OnValueChangedPrivate);
        MusicPlayer.SetVolume(0.6f);
        m_Scrollbar.value = 0.6f;
    }
    //方法
    private void OnValueChangedPrivate(float T)
    {
        MusicPlayer.SetVolume(T);
        //print("代码控制" + T);
    }

}