using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSelectManager
{
    public static List<MusicData> mdata;
	public static List<MusicData> mdata_new;
	public static int CardNum;
    public static void InitCardSelector()
    {
        mdata = MusicLoader.ConvertMusicFoldersToMusicData( MusicLoader.LoadSongDataFromDisk());
		mdata_new = new List<MusicData>();
		CardNum = mdata.Count;
		DestroyCardList();
		CreateCardList();
	}

    public static void DestroyCardList()
    {
		GameObject songlist = GameObject.FindGameObjectWithTag("CardList");
		for (int i = 0; i < songlist.transform.childCount; i++)
		{
			Transform child = songlist.transform.GetChild(i);
			GameObject.Destroy(child.gameObject);
		}
		return;
	}
	public static void CreateCardList()
	{
		GameObject cardList = GameObject.Find("CardList");
		int index = 0;
		foreach (MusicData md in mdata)
		{
			GameObject cardSelectItem = new GameObject("CardWheelItem" + string.Format("{0}", index));

			CardSelectItem temp = cardSelectItem.AddComponent<CardSelectItem>();


			cardSelectItem.transform.parent = cardList.transform;
			
			GameObject textContent = new GameObject();

			textContent.transform.parent = cardSelectItem.transform;
			
			Text textContentText = textContent.AddComponent<Text>();
			textContentText.text = LanguageManager.UTF8String(md.packname+"-" + md.NO);
			textContentText.font = (Font)Resources.Load("chaoshijicufanghei");
			textContentText.alignment = TextAnchor.UpperCenter;
			textContentText.color = Color.white;
			textContentText.transform.localPosition = new Vector3(0f, -1.6f, -10f);
			textContentText.fontSize = 24;
			textContent.GetComponent<RectTransform>().sizeDelta = new Vector2(160, 100);
			textContent.GetComponent<RectTransform>().localScale = new Vector3(0.01f, 0.01f, 0.01f);

			
			temp.rImage = cardSelectItem.AddComponent<RawImage>();
			byte[] bytes = FileManager.ReadBytesWWW(MusicLoader.SongList[index].image);

			//创建Texture
			Texture2D texture = new Texture2D(0, 0);
			texture.LoadImage(bytes);
			int area = texture.width * texture.height;
			float max_area = 5.7f * 8.8f *0.04f;
			float ratio = (float)(System.Math.Sqrt(max_area * 1.0f / area));
			temp.rImage.texture = texture;
			temp.rImage.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.width * ratio, texture.height * ratio);

			temp.m_Button = cardSelectItem.AddComponent<Button>();
			temp.m_Button.targetGraphic = temp.rImage;
			temp.m_Button.onClick.RemoveAllListeners();
			temp.m_Button.onClick.AddListener(temp.ButtonOnClickEvent);

			ColorBlock cb = new ColorBlock();
			cb = temp.m_Button.colors;
			cb.normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
			cb.highlightedColor = new Color(0 / 255f, 255 / 255f, 255 / 255f, 225 / 255f);
			cb.pressedColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
			cb.selectedColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
			cb.fadeDuration = 0.4f;
			temp.m_Button.colors = cb;
			/*
			temp.button_obj = new GameObject();
			temp.button_obj.transform.parent = cardSelectItem.transform;
			temp.button_obj.transform.localPosition = new Vector3(20f, 0, 0f);
			temp.button = temp.button_obj.AddComponent<Button>();

			//background image for item;
			temp.image = temp.button_obj.AddComponent<Image>();
			temp.image.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 6f);
			temp.image.color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 125 / 255f);

			//temp.button.GetComponent<RectTransform>().sizeDelta = new Vector2(40f, 6f);
			temp.button.targetGraphic = temp.image;
			temp.button.onClick.RemoveAllListeners();
			temp.button.onClick.AddListener(temp.ButtonClicked);

			ColorBlock cb = new ColorBlock();

			cb = temp.button.colors;
			cb.normalColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 120 / 255f);
			cb.highlightedColor = new Color(0 / 255f, 255 / 255f, 255 / 255f, 20 / 255f);
			cb.pressedColor = new Color(0 / 255f, 255 / 255f, 255 / 255f, 20 / 255f);
			cb.selectedColor = new Color(0 / 255f, 255 / 255f, 255 / 255f, 10 / 255f);
			cb.fadeDuration = 0.5f;
			temp.button.colors = cb;

			temp.text_title = new GameObject();
			temp.text_title.transform.parent = cardSelectItem.transform;
			temp.text_title_tm = temp.text_title.AddComponent<TextMesh>();

			temp.text_artist = new GameObject();
			temp.text_artist.transform.parent = cardSelectItem.transform;
			temp.text_artist_tm = temp.text_artist.AddComponent<TextMesh>();

			temp.text_animation = new GameObject();
			temp.text_animation.transform.parent = cardSelectItem.transform;
			temp.text_animation_tm = temp.text_animation.AddComponent<TextMesh>();

			temp.text_translated = new GameObject();
			temp.text_translated.transform.parent = cardSelectItem.transform;
			temp.text_translated_tm = temp.text_translated.AddComponent<TextMesh>();

			temp.transform.localScale = new Vector3(8f, 8f, 8f);
			temp.transform.localPosition = new Vector3(-160, -20 + index * 50, -1);
			temp.text_title_tm.text = LanguageManager.UTF8String(md.title);
			temp.text_artist_tm.text = LanguageManager.UTF8String(md.artist);
			temp.text_animation_tm.text = LanguageManager.UTF8String(md.animation);
			temp.text_translated_tm.text = LanguageManager.UTF8String(md.translated);

			temp.text_title.transform.localPosition = new Vector3(0, 2.8f, 0);
			temp.text_title_tm.fontSize = 24;

			temp.text_artist.transform.localPosition = new Vector3(0, -1f, 0);
			temp.text_artist_tm.fontSize = 12;

			temp.text_animation.transform.localPosition = new Vector3(38, -0.8f, 0);
			temp.text_animation_tm.fontSize = 16;
			temp.text_animation_tm.anchor = TextAnchor.UpperRight;

			temp.text_translated.transform.localPosition = new Vector3(38, 3, 0);
			temp.text_translated_tm.fontSize = 20;
			temp.text_translated_tm.anchor = TextAnchor.UpperRight;
			
			cardSelectItem.layer = LayerMask.NameToLayer("UI");
			foreach (Transform tran in cardSelectItem.GetComponentsInChildren<Transform>())
			{//遍历当前物体及其所有子物体
				tran.gameObject.layer = LayerMask.NameToLayer("UI");//更改物体的Layer层
			}
			//temp.button_obj.transform.gameObject.layer = LayerMask.NameToLayer("Background");
			*/
			temp.index = index;
			index++;
		}
	}
    public static void AddCard(MusicData md)
    {
        for(int i = 0; i < mdata_new.Count; i++)
        {
            if (mdata_new[i].title == md.title) return;
        }
		mdata_new.Add(md);
    }
    public static void RemoveCard(MusicData md)
    {
        for (int i = 0; i < mdata_new.Count; i++)
        {
            if (mdata_new[i].title == md.title)
            {
				mdata_new.RemoveAt(i);
                return;
            }
        }
    }
}

public class CardWheelBase
{
	private static float WheelPos = 0;
	private static int ItemCount;
	public static void UpdateWheelPos(float f)
	{
		WheelPos = f;
	}
	public static float GetWheelPos()
	{
		return WheelPos;
	}
	public static void UpdateWheelItemCount(int i)
	{
		ItemCount = i;
	}
	public static int GetWheelItemCount()
	{
		return ItemCount;
	}
}