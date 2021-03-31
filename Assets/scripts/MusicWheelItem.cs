using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class MusicWheelItem : MonoBehaviour
{
    private GameObject wheelObject; //self
    public GameObject text_title,text_artist,text_animation,text_translated, button_obj,image_obj,side_image_obj;
    public TextMesh text_title_tm, text_artist_tm, text_animation_tm, text_translated_tm;
    public int index;
    public string _title, _artist, _animation, _translated;
    public Button button;
    public Image image;
    public Image side_image;
    public bool played;
    // Start is called before the first frame update
    void Start()
    {
        SetTextColor(Color.black);
        text_translated_tm.color = new Color(49 / 255f, 49 / 255f, 49 / 255f, 125 / 255f);

        side_image_obj = new GameObject();
        side_image_obj.transform.parent = this.transform;
        side_image = side_image_obj.AddComponent<Image>();
        Sprite TempImg = Resources.Load("Graphics/_glider(stretch)", typeof(Sprite)) as Sprite;
        //改变图片
        side_image.sprite = TempImg;
        side_image.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.8f);
        side_image.transform.localPosition = new Vector3(-1.2f, 0, 0);
        side_image.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 125 / 255f);
        side_image_obj.SetActive(false);
        played = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(-160, -index * 50 + MusicWheelBase.GetWheelPos()*(MusicWheelBase.GetWheelItemCount()-1)*50, -1);
    }
    public void SetTextColor(Color c)
    {
        text_title_tm.color = c;
        text_artist_tm.color = c;
        text_animation_tm.color = c;
        text_translated_tm.color = c;
    }
    public void SetText(string title,string artist,string anime,string trans)
    {
        _title = title;
        _artist = artist;
        _animation = anime;
        _translated = trans;
        text_title_tm.text = LanguageManager.UTF8String(_title);
        text_artist_tm.text = LanguageManager.UTF8String(_artist);
        text_animation_tm.text = LanguageManager.UTF8String(_animation);
        text_translated_tm.text = LanguageManager.UTF8String(_translated);
    }
    public void Settitle(string s)
    {
        _title = s;
    }
    public void ButtonClicked()
    {
        print(string.Format("{0} clicked. Play {1}", index,text_title_tm.text));
        side_image_obj.SetActive(true);
        played = true;
        GameObject.Find("SongTitleText").GetComponent<Text>().text = LanguageManager.UTF8String(MusicLoader.SongList[index].title);
        GameObject.Find("SongAnimeText").GetComponent<Text>().text = LanguageManager.UTF8String(MusicLoader.SongList[index].animation);
        //text_title_tm.text = LanguageManager.UTF8String(MusicLoader.SongList[index].title);
        //image.color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 125 / 255f);
        MusicPlayer.LoadAudio(MusicLoader.SongList[index].title, MusicLoader.SongList[index].music);
        MusicPlayer.PlayAudio(MusicLoader.SongList[index].title);

        RawImage img = GameObject.Find("SongImage").GetComponent<RawImage>();

        byte[] bytes = FileManager.ReadBytesWWW(MusicLoader.SongList[index].image);

        //创建Texture
        //int width = 570;
        //int height = 880;
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        Destroy(img.texture);
        //创建Sprite
        int area = texture.width * texture.height;
        int max_area = 570 * 880;
        float ratio = (float)(System.Math.Sqrt(max_area * 1.0f / area));
        img.texture = texture;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width * ratio, texture.height * ratio);
    }
    private IEnumerator LoadImage(string imgUrl)
    {
        #if UNITY_EDITOR || UNITY_IOS
            imgUrl = "file://" + imgUrl;
        #endif
        // 根据连接下载
        Image img = GameObject.Find("SongImage").GetComponent<Image>();
        WWW www = new WWW(imgUrl);
        // 等待WWW代码执行完毕之后后面的代码才会执行。
        yield return www;
        //创建Texture
        //int width = 570;
        //int height = 880;
        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(www.bytes);

        //创建Sprite
        int area = texture.width * texture.height;
        int max_area = 570 * 880;
        float ratio = (float)(System.Math.Sqrt(max_area * 1.0f / area));
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        img.sprite = sprite;
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width * ratio, texture.height * ratio);
    }
}
