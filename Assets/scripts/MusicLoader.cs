using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MusicLoader : MonoBehaviour
{
	public string MUSICPATH = "E:/SJTU备份箱/游戏设计/MusicData/";
	private DirectoryInfo musicdir;
	private List<MusicFolder> musicfolders;
	// Start is called before the first frame update
	void Start()
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
	}
	public List<MusicFolder> MusicDirFilter(List<MusicFolder> mfolders,string[] fltr)
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

	public long GetMusicDirLength(){
		if (musicdir.Exists)
			return 0;
		long len = 0;
		foreach (FileInfo fi in musicdir.GetFiles())
		{
			len++;
		}
		return len;
	}
	// Update is called once per frame
	void Update()
	{

	}
}