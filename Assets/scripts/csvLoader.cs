using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class csvController  {

	static csvController csv;
	public List<string[]> arrayData;  

	private csvController()   //单例
	{
		arrayData = new List<string[]>();
	}

	public static csvController GetInstance()   //单例方法获取对象
	{
		if(csv == null)
		{
			csv = new csvController();
		}
		return csv;
	}

	public void loadFile(string path,string fileName)
	{
		arrayData.Clear();
		StreamReader sr = null;
		try
		{
			string file_url = path + "//" + fileName;    //根据路径打开文件
			sr = File.OpenText(file_url);
			Debug.Log("File Find in " + file_url);
		}
		catch
		{
			Debug.Log("File cannot find ! ");
			return; 
		}

		string line;
		while((line = sr.ReadLine()) != null)   //按行读取
		{
			arrayData.Add(line.Split(','));   //每行逗号分隔,split()方法返回 string[]
		}
		sr.Close();
		sr.Dispose();
	}

	public List<string[]> getData()
	{
		return arrayData;
	}
	public string getString(int row,int col)
	{
		return arrayData[row][col];
	}
	public int getInt(int row,int col)
	{
		return int.Parse(arrayData[row][col]);
	}
}
