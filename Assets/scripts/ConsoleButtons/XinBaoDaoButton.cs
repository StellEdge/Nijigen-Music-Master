using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class XinBaoDaoButton : MonoBehaviour
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
        GameObject.Find("SongTitleText").GetComponent<Text>().text = LanguageManager.UTF8String("新宝岛");
        GameObject.Find("SongAnimeText").GetComponent<Text>().text = LanguageManager.UTF8String("等待中");
        MusicPlayer.PlayXinBaoDao();
    }
}