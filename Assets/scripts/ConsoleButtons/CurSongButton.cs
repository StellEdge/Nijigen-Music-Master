using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurSongButton : MonoBehaviour
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

        int index = MusicWheelBase.GetCurSongIndex();
        MusicWheelBase.SetWheelPosIndex(index);
    }
}
