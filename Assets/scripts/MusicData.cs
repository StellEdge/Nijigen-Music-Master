using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.IO;
public class LanguageManager
{
	public static string UTF8String(string input)
	{
		UTF8Encoding utf8 = new UTF8Encoding();
		return utf8.GetString(utf8.GetBytes(input));
	}
	public static string ConvToTime(float s)
	{
		int t = Mathf.FloorToInt(s);
		int th = t / 3600;
		int tm = (t - th * 3600) / 60;
		int ts = t - th * 3600 - tm * 60;
		string res = string.Format("{0:D2}:{1:D2}:{2:D2}", th, tm, ts);
		return res;
	}
}

public class MusicWheelBase
{
	private static float WheelPos=0;
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
	public static int GetCurSongIndex()
    {
		int index = 0;
		for (int i = 0; i < MusicWheelBase.GetWheelItemCount(); i++)
		{
			if (MusicLoader.SongList[i].title == MusicPlayer.GetCurAudio())
			{
				index = i;
				break;
			}
		}
		return index;
	}
	public static void SetWheelPosIndex(int index)
    {
		float t = index * 1.0f / (MusicWheelBase.GetWheelItemCount() - 1);
		MusicWheelBase.UpdateWheelPos(t);
		Scrollbar m_Scrollbar = GameObject.Find("Scrollbar").GetComponent<Scrollbar>();
		m_Scrollbar.value = t;
	}
}
public class MusicDataPath
{
	public static string MUSICPATH_DEBUG = "D:/Unitykit/MusicData/";
	public static string MUSICPATH_ANDROID = Application.persistentDataPath+ "/MusicData/";
	//public static string MUSICPATH_ANDROID = "/storage/emulated/0/NijigenMusicMaster/MusicData/";
	public static string MUSICPATH = System.Environment.CurrentDirectory+"/MusicData/";
	//result: X:\xxx\xxx(.exe文件所在的目录)
}
public class MusicData{
	public int NO;
	public string title,subtitle,artist,translated,animation,music,image;
	public string packname;
	public MusicData()
    {

    }
	public MusicData(string[] info)
    {
		SetMusicData(info);
    }
	public void SetMusicData(string[] info)
    {
		NO = int.Parse(info[0]);
		title = info[1];
		subtitle = info[2];
		artist = info[3];
		translated = info[4];
		animation = info[5];
		music = info[6];
		image = info[7];
		//packname = info[8];
	}
}
public class MusicFolder{
	public List<MusicData> musicdata;
	public int Length;
	public string Name;
	public MusicFolder(){
		musicdata = new List<MusicData>();
	}
}
public static class FileManager
{
	public static string GetAndroidMusicDataFilesDir()
	{
		string[] potentialDirectories = new string[]
		{
			"/storage",
			"/sdcard",
			"/storage/emulated/0",
			"/mnt/sdcard",
			"/storage/sdcard0",
			"/storage/sdcard1"
		};

		if (Application.platform == RuntimePlatform.Android)
		{
			for (int i = 0; i < potentialDirectories.Length; i++)
			{
				if (Directory.Exists(potentialDirectories[i]+ "/NijigenMusicMaster/MusicData"))
				{
					return potentialDirectories[i] + "/NijigenMusicMaster/MusicData";
				}
			}
		}
		return "";
	}

	public static string GetMusicDataPath()
    {
		string[] MUSICPATHS_WIN = new string[]
		{
			System.Environment.CurrentDirectory + "/MusicData",
			"D:/Unitykit/MusicData"	//DEBUG
		};
		string[] MUSICPATHS_ANDROID = new string[]
		{
			Application.persistentDataPath + "/MusicData"
		};
		if (Application.platform == RuntimePlatform.Android)
        {
			for (int i = 0; i < MUSICPATHS_ANDROID.Length; i++)
			{
				if (Directory.Exists(MUSICPATHS_ANDROID[i]))
				{
					return MUSICPATHS_ANDROID[i];
				}
			}
			return GetAndroidMusicDataFilesDir();
		}
		else if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
			for (int i = 0; i < MUSICPATHS_WIN.Length; i++)
			{
				if (Directory.Exists(MUSICPATHS_WIN[i]))
				{
					return MUSICPATHS_WIN[i];
				}
			}
		}
			
		return "";
	}

	public static byte[] ReadBytesSystemIO(string path)
    {
		//在这里做文件读取
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		fileStream.Seek(0, SeekOrigin.Begin);
		//创建文件长度缓冲区
		byte[] bytes = new byte[fileStream.Length];
		//读取文件
		fileStream.Read(bytes, 0, (int)fileStream.Length);
		//释放文件读取流
		fileStream.Close();
		fileStream.Dispose();
		fileStream = null;
		return bytes;
	}
	public static byte[] ReadBytesWWW(string _path)
	{
		#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
			_path = "file:///" + _path;
		#endif
		Debug.Log("FileManager:Reading "+ _path);
		WWW www = new WWW(_path);
		//YieldToStop(www);
		while (!www.isDone);
		return www.bytes;
	}
	private static void YieldToStop(WWW www)
	{
		var @enum = DownloadEnumerator(www);
		while (@enum.MoveNext()) ;
	}
	private static IEnumerator DownloadEnumerator(WWW www)
	{
		while (!www.isDone) ;

		yield return www;
	}
	public static AudioClip ReadMp3WWW(string _path)
	{
		#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
				_path = "file:///" + _path;
		#endif
		Debug.Log("FileManager:Reading " + _path);
		WWW www = new WWW(_path);
		while (!www.isDone) ;
		AudioClip clip = www.GetAudioClip(true, false, AudioType.MPEG);
		return clip;
	}
}