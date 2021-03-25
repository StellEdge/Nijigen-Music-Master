using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;

//Sub module of Music manager
public static class MusicLoader
{
	private static string MUSICPATH = MusicDataPath.MUSICPATH;
	private static DirectoryInfo musicdir;
	private static List<MusicFolder> musicfolders;
	private const string cLocalPath = "file://";
	//build audio list:
	private static Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
	private static Dictionary<string, byte[]> audioWavDic = new Dictionary<string, byte[]>();

	public static List<MusicData> SongList;

	private static object audioWavDic_lock = new object();
	private static string[] FolderFilter = { "Dark","CoreSet" };

	public static void InitMusicLoader()
	{
		/*
		if (!Directory.Exists(MUSICPATH))
		{
			MUSICPATH = MusicDataPath.MUSICPATH_DEBUG;
		}
		if (!Directory.Exists(MUSICPATH))
		{
			MUSICPATH = MusicDataPath.MUSICPATH_ANDROID;
		}*/
		MUSICPATH = FileManager.GetMusicDataPath()+"/";
		debugtext.Add("MyDebug", "MUSICPATH " + MUSICPATH);
		Debug.Log("MUSICPATH " + MUSICPATH);
		musicfolders = new List<MusicFolder>();
		MusicFolder temp_musicfolder;
		MusicData temp_music = new MusicData();
		//load music files
		if (Directory.Exists(MUSICPATH)){  
			musicdir = new DirectoryInfo(MUSICPATH);  
			DirectoryInfo[] musicsets = musicdir.GetDirectories();

			for(int i=0;i<musicsets.Length;i++){
				if(File.Exists(MUSICPATH+musicsets[i].Name+"/data.csv")){
					// good pack?
					Debug.Log( "Found pack csv:" + musicsets[i].Name ); 
					//temp_musicfolder.musicdata.Clear();
					try{
						csvController.GetInstance().loadFile(MUSICPATH+musicsets[i].Name,"data.csv");
						List<string[]> musiclist=csvController.GetInstance().getData();
						musiclist.RemoveAt(0);
						temp_musicfolder = new MusicFolder();
						temp_musicfolder.Name = musicsets[i].Name;
						temp_musicfolder.musicdata.Clear();
						foreach(string[] info in musiclist){
							//temp_music.SetMusicData(info);
							info[6] = MUSICPATH + musicsets[i].Name + "//" + info[6];
							info[7] = MUSICPATH + musicsets[i].Name + "//" + info[7];
							temp_musicfolder.musicdata.Add(new MusicData(info));
						}	
						Debug.Log( "Loaded pack csv:" + musicsets[i].Name ); 
						musicfolders.Add(temp_musicfolder);
					}
					catch{
						Debug.Log( musicsets[i].Name+ "load failed!");
					}
				}
			}
		}

		//csv load complete
		MusicDirFilter(musicfolders, FolderFilter);

		UpdateSongList(musicfolders);
		//UpdateMusicManagerAudioDic(audioDic);

	}
	public static string[] GetFolderFilter()
    {
		return FolderFilter;
    }
	private static void UpdateMusicManagerAudioDic(Dictionary<string,AudioClip> d)
    {
		MusicPlayer.UpdateAudioDic(d);

    }
	public static void UpdateSongList(List<MusicFolder> mfolders)
	{
		//Get music folders from MusicLoader
		//List<Thread> threads = new List<Thread>();
		//Thread t;
		SongList = new List<MusicData>();

		
		foreach (MusicFolder mf in mfolders)
		{
			ReadAndLoadMusicFolderToAudioDic(mf);
			foreach (MusicData md in mf.musicdata){
				SongList.Add(md);
			}
		}

		/*
		foreach (MusicFolder mf in mfolders)
		{
            t = new Thread(ReadAndLoadMusicFolder);
			t.Start(mf);
			foreach (MusicData md in mf.musicdata){
				SongList.Add(md);
			}
		}*/
		SongList = Shuffle<MusicData>(SongList);
		CreateWheelItems(SongList);
	}
	private static void ReadAndLoadMusicFolderAsync(object obj)
	{
		MusicFolder mf = obj as MusicFolder;
		Debug.Log("Start loading " + mf.Name);
		foreach (MusicData md in mf.musicdata)
		{
			//string musicfilepath = cLocalPath + md.music;
			FileStream fileStream = new FileStream(md.music, FileMode.Open, FileAccess.Read);
			fileStream.Seek(0, SeekOrigin.Begin);
			//创建文件长度缓冲区
			byte[] bytes = new byte[fileStream.Length];
			//读取文件
			fileStream.Read(bytes, 0, (int)fileStream.Length);
			//释放文件读取流
			fileStream.Close();
			fileStream.Dispose();
			fileStream = null;

			//Convert Music to AudioClip Can only be deployed in the main thread.
			//So audioWavDic act as buffer

			lock (audioWavDic_lock)
			{
				//audioDic.Add(md.title, clip);
				audioWavDic.Add(md.title, bytes);
				Debug.Log("Add Music:" + md.title);
			}
		}
		return;
	}
	private static void ReadAndLoadMusicFolderToAudioDic(object obj)
    {
		return;
    }
	private static void ReadAndLoadMusicFolder(object obj)
	{
		//deprecated
		MusicFolder mf = obj as MusicFolder;
		Debug.Log("Start loading "+mf.Name);
		AudioClip clip;
		foreach (MusicData md in mf.musicdata)
		{
			//string musicfilepath = cLocalPath + md.music;
			FileStream fileStream = new FileStream(md.music, FileMode.Open, FileAccess.Read);
			fileStream.Seek(0, SeekOrigin.Begin);
			//创建文件长度缓冲区
			byte[] bytes = new byte[fileStream.Length];
			//读取文件
			fileStream.Read(bytes, 0, (int)fileStream.Length);
			//释放文件读取流
			fileStream.Close();
			fileStream.Dispose();
			fileStream = null;
			if (bytes.Length!=0)
            {
				//WWW www = new WWW(musicfilepath);
				clip = NAudioPlayer.FromMp3Data(bytes);
			}
            else
            {
				clip = null;
            }
			Debug.Log("Load " + md.music);
			//Read musicbytes Here:
			//TODO:

			//Convert Music to AudioClip Can only be deployed in the main thread.
			//So audioWavDic act as buffer

			lock (audioWavDic_lock)
			{
				audioDic.Add(md.title, clip);
				//audioWavDic.Add(md.title, musicbytes);
				Debug.Log("Add Music:"+ md.title);
			}
		}
		return;
	}
	public static List<MusicFolder> MusicDirFilter(List<MusicFolder> mfolders,string[] fltr)
    {
		if (fltr.Length == 0) return mfolders;
		//lazy code
		List<string> l_fltr = new List<string>(fltr);
		for (int i = mfolders.Count - 1; i >= 0; i--)
        {
            if (!l_fltr.Exists(t => t == mfolders[i].Name))
            {
				mfolders.RemoveAt(i);
            }
        }
		return mfolders;
    }
	public static long GetMusicDirLength(){
		if (musicdir.Exists)
			return 0;
		long len = 0;
		foreach (FileInfo fi in musicdir.GetFiles())
		{
			len++;
		}
		return len;
	}
	private static void CreateWheelItems(List<MusicData> mdata)
	{
		GameObject songlist = GameObject.FindGameObjectWithTag("SongList");
		int index = 0;
		foreach (MusicData md in mdata)
		{
			GameObject musicwheelitem = new GameObject("WheelItem"+string.Format("{0}",index));
				
			MusicWheelItem temp = musicwheelitem.AddComponent<MusicWheelItem>();
				

			musicwheelitem.transform.parent = songlist.transform;

			temp.button_obj = new GameObject();
			temp.button_obj.transform.parent = musicwheelitem.transform;
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
			cb.highlightedColor = new Color(255 / 255f, 255 / 255f, 255 / 255f, 0 / 255f);
			cb.pressedColor = new Color(125 / 255f, 255 / 255f, 255 / 255f, 20 / 255f);
			cb.selectedColor = new Color(125 / 255f, 255 / 255f, 255 / 255f, 10 / 255f);
			temp.button.colors = cb;

			temp.text_title = new GameObject();
			temp.text_title.transform.parent = musicwheelitem.transform;
			temp.text_title_tm = temp.text_title.AddComponent<TextMesh>();

			temp.text_artist = new GameObject();
			temp.text_artist.transform.parent = musicwheelitem.transform;
			temp.text_artist_tm = temp.text_artist.AddComponent<TextMesh>();

			temp.text_animation = new GameObject();
			temp.text_animation.transform.parent = musicwheelitem.transform;
			temp.text_animation_tm = temp.text_animation.AddComponent<TextMesh>();

			temp.text_translated = new GameObject();
			temp.text_translated.transform.parent = musicwheelitem.transform;
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

			musicwheelitem.layer = LayerMask.NameToLayer("UI");
			foreach (Transform tran in musicwheelitem.GetComponentsInChildren<Transform>())
			{//遍历当前物体及其所有子物体
				tran.gameObject.layer = LayerMask.NameToLayer("UI");//更改物体的Layer层
			}
			//temp.button_obj.transform.gameObject.layer = LayerMask.NameToLayer("Background");

			temp.index = index;
			index++;
		}
		
		MusicWheelBase.UpdateWheelItemCount(index);

	}
	private static void DeleteWheelItems()
    {
		GameObject songlist = GameObject.FindGameObjectWithTag("SongList");
		for(int i = 0; i < songlist.transform.childCount; i++)
        {
			Transform child = songlist.transform.GetChild(i);
			GameObject.Destroy(child.gameObject);
        }
	}
	public static void ReshuffleSongList()
    {
		DeleteWheelItems();
		SongList = Shuffle<MusicData>(SongList);
		CreateWheelItems(SongList);
	}
	public static List<T> Shuffle<T>(List<T> original)
	{
		System.Random randomNum = new System.Random();
		int index = 0;
		T temp;
		for (int i = 0; i < original.Count; i++)
		{
			index = randomNum.Next(0, original.Count - 1);
			if (index != i)
			{
				temp = original[i];
				original[i] = original[index];
				original[index] = temp;
			}
		}
		return original;
	}
}