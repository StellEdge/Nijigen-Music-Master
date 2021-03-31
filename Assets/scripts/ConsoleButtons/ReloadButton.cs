using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReloadButton : MonoBehaviour
{
    public Button m_Button;
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }
    public void ButtonOnClickEvent()
    {
        MusicLoader.InitMusicLoader();
    }
}