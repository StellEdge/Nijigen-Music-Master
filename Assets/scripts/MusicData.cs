using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicData{
	public int NO;
	public string title,subtitle,artist,translated,animation,music,image;
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
	public ArrayList musicdata;
	public int Length;
	public string Name;
	public MusicFolder(){
		musicdata = new ArrayList();
	}
}
