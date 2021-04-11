using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSelectButton : MonoBehaviour
{
    private Button m_Button;
    private bool cardSelectState = false;
    private GameObject cl;
    // Start is called before the first frame update
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        cl = GameObject.Find("CardListContainer");
        cl.SetActive(cardSelectState);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonOnClickEvent()
    {
        // Application.LoadLevel("songselect");
        cardSelectState = !cardSelectState;
        cl.SetActive(cardSelectState);
        if (cardSelectState)
        {
            CardSelectManager.InitCardSelector();
            GameObject.Find("Scrollbar").GetComponent<SongListScrollbar>().blocked = true;
        }
        else
        {
            MusicLoader.UpdateSongList(CardSelectManager.mdata_new);
            GameObject.Find("Scrollbar").GetComponent<SongListScrollbar>().blocked = false;
            MusicLoader.FolderFilter = CardSelectManager.filters.ToArray();
        }
    }
}