using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSelectManager
{
    public static List<MusicData> mdata;
	public static List<MusicData> mdata_new;
	public static List<MusicFolder> mfdata;
	public static List<string> filters;
	public static int CardNum;
    public static void InitCardSelector()
    {
		mfdata = MusicLoader.LoadSongDataFromDisk();
		mdata = MusicLoader.ConvertMusicFoldersToMusicData(mfdata);
		mdata_new = new List<MusicData>();
		filters = new List<string>();
		CardNum = mdata.Count;
		GameObject.Find("TotalCards").GetComponent<Text>().text = "CurrentSelected:" + mdata_new.Count;
		DestroyCardList();
		CreateCardList();
		DestroyToogleList();
		CreateToggleList();
		//InitCardListToCurrentList();
	}
	public static void InitCardListToCurrentList()
	{
		GameObject songlist = GameObject.FindGameObjectWithTag("CardList");
		List<MusicData> cur_list = MusicLoader.SongList;
        if (true)//(cur_list.Count < mdata.Count * 0.8)
        {
			for (int i = 0; i < cur_list.Count; i++)
			{

				int cur_ind = -1;
				for (int j = 0; j < mdata.Count; j++)
                {
					if (cur_list[i] == mdata[j])
                    {
						cur_ind = j;
						break;
					}
                }
				if (cur_ind != -1)
				{
					Debug.Log("Find a pre listed item: "+mdata[cur_ind].title);
					GameObject card_item = GameObject.Find("CardWheelItem" + string.Format("{0}", cur_ind));

					Debug.Log("Add :" + CardSelectManager.mdata[cur_ind].title);
					AddCard(CardSelectManager.mdata[cur_ind]);
					card_item.GetComponent<CardSelectItem>().rImage.color = Color.cyan;
					foreach (Transform child in card_item.transform)
					{
						child.gameObject.GetComponent<Text>().color = Color.cyan;
					}
					card_item.GetComponent<CardSelectItem>().state = true;
				}
			}
		}

		return;
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
			temp.state = false;

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
			byte[] bytes = FileManager.ReadBytesWWW(FileManager.GetThumbnailPath(md.image));

			//创建Texture
			Texture2D texture = new Texture2D(0, 0);
			texture.LoadImage(bytes);
			int area = texture.width * texture.height;
			float max_area = 6.75f * 10.5f *0.035f;
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
			temp.index = index;
			index++;
		}
	}

	public static void DestroyToogleList()
	{
		GameObject tglist = GameObject.Find("CSToggleList");
		for (int i = 0; i < tglist.transform.childCount; i++)
		{
			Transform child = tglist.transform.GetChild(i);
			GameObject.Destroy(child.gameObject);
		}
		return;
	}
	public static void CreateToggleList()
    {
		GameObject tglist = GameObject.Find("CSToggleList");
		int index = 0;
		foreach (MusicFolder mf in mfdata)
        {
			GameObject tgItem = new GameObject("CStoggleItem" + string.Format("{0}", index));

			SongFolderFilterToogle temp = tgItem.AddComponent<SongFolderFilterToogle>();
			Toggle tg = tgItem.AddComponent<Toggle>();

			temp.toggle1 = tg;
			temp.setname = mf.Name;

			tgItem.transform.SetParent(tglist.transform);

			tgItem.transform.localPosition = new Vector3(400f- 120f * (index / 2), 265f-22f*(index%2), 0f);
			tgItem.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
			tgItem.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			GameObject textItem = new GameObject("CStoggleItem" + string.Format("{0}", index) + "text");
			textItem.transform.SetParent(tgItem.transform);
			textItem.transform.localPosition = new Vector3(0f, -1f, 0f);
			Text textItem_text = textItem.AddComponent<Text>();
			textItem_text.text =LanguageManager.UTF8String (mf.Name);
			textItem_text.font = (Font)Resources.Load("chaoshijicufanghei");
			textItem_text.alignment = TextAnchor.MiddleLeft;
			textItem.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 20);
			textItem.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			GameObject bgItem = new GameObject("CStoggleItem" + string.Format("{0}", index)+"bg");
			bgItem.transform.SetParent(tgItem.transform);
			bgItem.transform.localPosition = new Vector3(-60f, 0f, 0f);
			Image bgimg = bgItem.AddComponent<Image>();
			bgimg.sprite = Resources.Load<Sprite>("bg");
			bgItem.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
			bgItem.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			GameObject checkmark = new GameObject("CStoggleItem" + string.Format("{0}", index) + "cm");
			checkmark.transform.SetParent(bgItem.transform);
			Image cmimg = checkmark.AddComponent<Image>();
			cmimg.sprite = Resources.Load<Sprite>("cm");
			checkmark.transform.localPosition = new Vector3(0f, 0f, 0f);
			checkmark.GetComponent<RectTransform>().sizeDelta = new Vector2(20, 20);
			checkmark.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);

			temp.toggle1.targetGraphic = bgimg;
			temp.toggle1.graphic = cmimg;

			temp.toggle1.onValueChanged.AddListener(delegate
			{
				temp.ToggleCallValueChanged(temp.toggle1);
			});
			index++;
			//mf.Name;
		}
	}
    public static void AddCard(MusicData md)
    {
        for(int i = 0; i < mdata_new.Count; i++)
        {
            if (mdata_new[i].title == md.title) return;
        }
		mdata_new.Add(md);
		GameObject.Find("TotalCards").GetComponent<Text>().text = "CurrentSelected:" + mdata_new.Count;
    }
    public static void RemoveCard(MusicData md)
    {
        for (int i = 0; i < mdata_new.Count; i++)
        {
            if (mdata_new[i].title == md.title)
            {
				mdata_new.RemoveAt(i);
				GameObject.Find("TotalCards").GetComponent<Text>().text = "CurrentSelected:" + mdata_new.Count;
				return;
            }
        }
		
	}
	public static void RemoveAllCard()
	{
		mdata_new = new List<MusicData>();
		GameObject.Find("TotalCards").GetComponent<Text>().text = "CurrentSelected:" + mdata_new.Count;
		GameObject songlist = GameObject.FindGameObjectWithTag("CardList");
		for (int i = 0; i < songlist.transform.childCount; i++)
		{
			Transform child = songlist.transform.GetChild(i);
			if (child.gameObject.GetComponent<CardSelectItem>().state)
			{
				child.gameObject.GetComponent<CardSelectItem>().ButtonOnClickEvent();
			}
		}
		return;
	}

	public static void AddAllCardOfFolder(string foldername)
    {
		for(int i = 0; i < mdata.Count; i++)
        {
			if(mdata[i].packname == foldername)
            {
				CardSelectItem card_item = GameObject.Find("CardWheelItem" + string.Format("{0}", i)).GetComponent<CardSelectItem>();
                if (!card_item.state)
                {
					card_item.ButtonOnClickEvent();
                }
			}
		}
	}
	public static void RemoveAllCardOfFolder(string foldername)
	{
		for (int i = 0; i < mdata.Count; i++)
		{
			if (mdata[i].packname == foldername)
			{
				CardSelectItem card_item = GameObject.Find("CardWheelItem" + string.Format("{0}", i)).GetComponent<CardSelectItem>();
				if (card_item.state)
				{
					card_item.ButtonOnClickEvent();
				}
			}
		}
	}
	public static void AddFilter(string name)
    {
		for (int i = 0; i < filters.Count; i++)
		{
			if (filters[i] == name) return;
		}
		filters.Add(name);
	}

	public static void RemoveFilter(string name)
	{
		for (int i = 0; i < filters.Count; i++)
		{
			if (filters[i] == name)
			{
				filters.RemoveAt(i);
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