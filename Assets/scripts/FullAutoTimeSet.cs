using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullAutoTimeSet : MonoBehaviour
{
    private Button m_Button;
    private float[] max_times =new float[]{30f,25f,20f,15f,10f,8f,5f };
    private float[] fade_times = new float[]{ 5f, 5f, 5f, 5f, 5f,4f,2f };
    private int cur_state = 0;
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }
    // Update is called once per frame
    void Update()
    {
        GameObject.Find("FullAutoTimerText").GetComponent<Text>().text = string.Format("{0:F0}-sec\n{1:F0}-fade", max_times[cur_state], fade_times[cur_state]);
    }
    public void ButtonOnClickEvent()
    {
        cur_state++;
        if (cur_state >= max_times.Length) cur_state = 0;
        FullAutoButton faButton = GameObject.Find("FullAutoButton").GetComponent<FullAutoButton>();
        faButton.max_time = max_times[cur_state];
        faButton.fade_time = fade_times[cur_state];
        //GameObject.Find("FullAutoTimerText").GetComponent<Text>().text = string.Format("{0:F0}sec", max_times[cur_state]);
    }
}
