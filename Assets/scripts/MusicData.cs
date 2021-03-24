using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
		while (!www.isDone);
		return www.bytes;
	}
	public static AudioClip ReadMp3WWW(string _path)
	{
		#if UNITY_EDITOR || UNITY_IOS || UNITY_ANDROID
				_path = "file:///" + _path;
		#endif
		Debug.Log("FileManager:Reading " + _path);
		WWW www = new WWW(_path);
		while (!www.isDone) ;
		return www.GetAudioClip(true, false, AudioType.MPEG);
	}
}