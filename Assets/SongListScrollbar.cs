using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongListScrollbar : MonoBehaviour
{
    // Update is called once per frame
    private bool HasFocus;
    void Update()
    {
        if(HasFocus && Input.GetAxis("Mouse ScrollWheel") > 0 && m_Scrollbar.value<1)
        {
            m_Scrollbar.value += 0.02f;
        }
        if(HasFocus && Input.GetAxis("Mouse ScrollWheel") < 0 && m_Scrollbar.value>0)
        {
            m_Scrollbar.value -= 0.02f;
        }
    }
    private Scrollbar m_Scrollbar;
    void Start()
    {
        m_Scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        //使用监听事件需要先持有组件的引用
        m_Scrollbar.onValueChanged.AddListener(OnValueChangedPrivate);
    }
    //方法
    public void OnValueChanged(float T)
    {
        //print("公开拖拽" + T);
    }
    private void OnValueChangedPrivate(float T)
    {
        MusicWheelBase.UpdateWheelPos(T);
        //print("代码控制" + T);
    }
    void OnApplicationFocus(bool focus)
    {
        //Debug.Log(string.Format("OnApplicationFocus:{0}", focus));
        HasFocus = focus;
    }
}
