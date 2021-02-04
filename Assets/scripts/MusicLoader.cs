using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading;


//Sub module of Music manager
public static class MusicLoader
{
	private static string MUSICPATH = MusicDataPath.MUSICPATH;
	private static DirectoryInfo musicdir;
	private static List<MusicFolder> musicfolders;
	private const string cLocalPath = "file:///";
	//build audio list:
	private static Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
	private static object audioDic_lock = new object();

	public static void InitMusicLoader()
	{
		musicfolders = new List<MusicFolder>();
		MusicFolder temp_musicfolder = new MusicFolder();
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
						temp_musicfolder.Name = musicsets[i].Name;
						temp_musicfolder.musicdata.Clear();
						foreach(string[] info in musiclist){
							temp_music.SetMusicData(info);
							temp_musicfolder.musicdata.Add(temp_music);
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
		UpdateMusicList(musicfolders);


	}

	public static void UpdateMusicList(List<MusicFolder> mfolders)
	{
		//Get music folders from MusicLoader
		List<Thread> threads = new List<Thread>();
		Thread t;
		foreach (MusicFolder mf in mfolders)
		{
            t = new Thread(ReadAndLoadMusicFolder);
			t.Start(mf);
		}
	}

	private static void ReadAndLoadMusicFolder(object obj)
	{
		MusicFolder mf = obj as MusicFolder;
		Debug.Log("Start loading "+mf.Name);
		foreach (MusicData md in mf.musicdata)
		{
			string musicfilepath = cLocalPath + MUSICPATH + md.music;

			//Read Here:
			WWW www = new WWW(" ");
			while (!www.isDone)
			{
			}

			//Convert Music to AudioClip 
			AudioClip ac = NAudioPlayer.FromMp3Data(www.bytes);	 
			
			ac.name = md.title;
			lock (audioDic_lock)
			{
				audioDic.Add(ac.name, ac);
				Debug.Log("Add Music:"+ ac.name);
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
}