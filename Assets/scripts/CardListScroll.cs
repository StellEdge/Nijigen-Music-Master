using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardListScroll : MonoBehaviour
{
    // Update is called once per frame
    private bool HasFocus;
    private Vector2 m_screenpos = new Vector2();
    void Update()
    {
        if (HasFocus && Input.GetAxis("Mouse ScrollWheel") > 0 && m_Scrollbar.value > 0)
        {
            m_Scrollbar.value -= 0.02f;
        }
        if (HasFocus && Input.GetAxis("Mouse ScrollWheel") < 0 && m_Scrollbar.value < 1)
        {
            m_Scrollbar.value += 0.02f;
        }

        if (HasFocus && Input.touchCount == 1)
        {

            if (Input.touches[0].phase == TouchPhase.Began)
            {
                // 记录手指触屏的位置  
                m_screenpos = Input.touches[0].position;

            }
            // 手指移动  
            else if (Input.touches[0].phase == TouchPhase.Moved)
            {
                m_Scrollbar.value += Input.touches[0].deltaPosition.y / 50 * Time.deltaTime;
            }
        }
    }
    private Scrollbar m_Scrollbar;
    void Start()
    {
        m_Scrollbar = GameObject.Find("CardListScroll").GetComponent<Scrollbar>();
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

    }
    void OnApplicationFocus(bool focus)
    {
        //Debug.Log(string.Format("OnApplicationFocus:{0}", focus));
        HasFocus = focus;
    }
}
