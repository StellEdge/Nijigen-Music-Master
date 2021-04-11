using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RedoRandom : MonoBehaviour
{
    // Start is called before the first frame update
    private Stack st;
    public void AddRandom(int i)
    {
        st.Push(i);
    }
    private Button m_Button;
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        st = new Stack();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ButtonOnClickEvent();
        }
    }

    public void ButtonOnClickEvent()
    {
        if (st.Count <= 1) return;
        //redo now
        int index = (int)st.Peek();
        MusicWheelItem item_t = GameObject.Find("WheelItem" + string.Format("{0}", index)).GetComponent<MusicWheelItem>();
        item_t.side_image_obj.SetActive(false);
        item_t.played = false;
        st.Pop();

        index = (int)st.Peek();
        MusicWheelItem item = GameObject.Find("WheelItem" + string.Format("{0}", index)).GetComponent<MusicWheelItem>();

        float t = index * 1.0f / (MusicWheelBase.GetWheelItemCount() - 1);
        MusicWheelBase.UpdateWheelPos(t);
        Scrollbar m_Scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
        m_Scrollbar.value = t;
        item.ButtonClicked();
        /*
        float pos = Random.Range(0f, 0.8f);
        if (GameObject.Find("LimTVSizeButton").GetComponent<LimitTVSize>().LimitTvSize)
        {
            float songlen = MusicPlayer.GetSongLength();
            float sec = Random.Range(0f, 80f);
            pos = sec / songlen;
        }*/

        GameObject.Find("SongProgressBar").GetComponent<SongProgressbar>().SetProgress(0f);
    }
}
