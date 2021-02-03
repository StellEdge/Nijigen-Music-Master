using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicWheelItem : MonoBehaviour
{
    private GameObject wheelObject; //self
    private GameObject text_title,text_artist,text_animation,text_translated;
    private TextMesh text_title_tm, text_artist_tm, text_animation_tm, text_translated_tm;

    private string _title, _artist, _animation, _translated;
    bool update_request;
    // Start is called before the first frame update
    void Start()
    {
        update_request = false; // not now
        wheelObject = gameObject;

        text_title = new GameObject();
        text_title.transform.parent = this.transform;
        text_title_tm = text_title.AddComponent<TextMesh>();

        text_artist = new GameObject();
        text_artist.transform.parent = this.transform;
        text_artist_tm = text_artist.AddComponent<TextMesh>();

        text_animation = new GameObject();
        text_animation.transform.parent = this.transform;
        text_animation_tm = text_animation.AddComponent<TextMesh>();

        text_translated = new GameObject();
        text_translated.transform.parent = this.transform;
        text_translated_tm = text_translated.AddComponent<TextMesh>();

        wheelObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

        SetTextColor(Color.white);
        SetText("Title", "Artist someone", "Animation", "Translation");
        text_title.transform.localPosition = new Vector3(0, 3, 0);
        text_title_tm.fontSize = 24;

        text_artist.transform.localPosition = new Vector3(0, 0, 0);
        text_artist_tm.fontSize = 16;

        text_animation.transform.localPosition = new Vector3(20, 0, 0);
        text_animation_tm.fontSize = 16;
        text_animation_tm.alignment=TextAlignment.Right;

        text_translated.transform.localPosition = new Vector3(0, 5, 0);
        text_translated_tm.fontSize = 1;        //not using now;

    }

    // Update is called once per frame
    void Update()
    {
        if (update_request)
        // only update when needed.
        {
            update_request = false;
        }
    }
    public void SetTextColor(Color c)
    {
        text_title_tm.color = c;
        text_artist_tm.color = c;
        text_animation_tm.color = c;
        text_translated_tm.color = c;
        update_request = true;
    }
    public void SetText(string title,string artist,string anime,string trans)
    {
        _title = title;
        _artist = artist;
        _animation = anime;
        _translated = trans;
        text_title_tm.text = _title;
        text_artist_tm.text = _artist;
        text_animation_tm.text = _animation;
        text_translated_tm.text = _translated;
        update_request = true;
    }
    public void Settitle(string s)
    {
        _title = s;
        update_request = true;
    }
}
