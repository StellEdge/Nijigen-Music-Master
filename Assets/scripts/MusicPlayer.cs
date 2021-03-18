using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio;
using NAudio.Wave;
using System.IO;
using System.Threading;
public static class MusicPlayer
{
    private const string cLocalPath = "file:///";
    private static Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();
    public static string currentAudio= "";
    private static AudioSource bgAudioSource;
    private static bool is_playing;

    private static object audioload_lock = new object();

    public static string GetCurAudio()
    {
        return currentAudio;
    }
    public static float GetSongLength()
    {
        if (bgAudioSource.clip)
            return bgAudioSource.clip.length;
        else
            return 0.01f;
    }
    public static bool GetIsPlaying()
    {
        return is_playing;
    }
    public static void InitMusicPlayer(AudioSource s)
    {
        bgAudioSource = s;
    }
    private static void LoadAudio_Tool(object obj)
    {
        string path = obj as string;

        AudioClip clip;
        //FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        //fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        //byte[] bytes = new byte[fileStream.Length];
        byte[] bytes = File.ReadAllBytes(path);
        //读取文件
        //fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        //fileStream.Close();
        //fileStream.Dispose();
        //fileStream = null;
        if (bytes.Length != 0)
        {
            //WWW www = new WWW(musicfilepath);
            clip = NAudioPlayer.FromMp3Data(bytes);
        }
        else
        {
            clip = null;
        }
        
    }
    public static void LoadAudioAsync(string audioName,string path)
    {
        if (!audioDic.ContainsKey(audioName))
        {
            Thread t;
            t = new Thread(LoadAudio_Tool);
            t.Start(path);
        }
    }
    public static void LoadAudio(string audioName, string path)
    {
        if (audioDic.ContainsKey(audioName))
        {
            return;
        }
        if (!File.Exists(path))
        {
            Debug.Log("No such file:" + path);
            debugtext.Add("MyDebug", "No such file: " + path);
        }

        AudioClip clip;
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        debugtext.Add("Music Loading",path);
        fileStream.Seek(0, SeekOrigin.Begin);
        //创建文件长度缓冲区
        byte[] bytes = new byte[fileStream.Length];
        //读取文件
        fileStream.Read(bytes, 0, (int)fileStream.Length);
        //释放文件读取流
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;
        /*
        if (bytes.Length != 0)
        {
            //WWW www = new WWW(musicfilepath);
            clip = NAudioPlayer.FromMp3Data(bytes);
            
        }
        else
        {
            clip = null;
            debugtext.Add("MyDebug", "Load " + audioName + "failed");
        }*/
        clip = NAudioPlayer.FromMp3Data(bytes);
        audioDic.Add(audioName, clip);
        Debug.Log("Load " + audioName);
    }
    public static float GetAudioPos()
    {
        if (currentAudio != null)
        {
            return bgAudioSource.time/ bgAudioSource.clip.length;
        }
        return 0;
    }

    public static void PlayAudioAtPos(float f)
    {
        if (currentAudio != null)
        {
            bgAudioSource.time = bgAudioSource.clip.length * f;
        }
    }
    public static void PlayAudio(string audioName, float volume = 1, bool isLoop = false)
    {
        if (!audioDic.ContainsKey(audioName))
        {
            return;
        }
        if (audioName != "" && audioName != currentAudio)
        {
            bgAudioSource.Stop();
            is_playing = false;
            bgAudioSource.clip = audioDic[audioName];
            currentAudio = audioName;
        }
        else
        {
            if (bgAudioSource.isPlaying)
            {
                is_playing = false;
                bgAudioSource.Stop();
            }
            else
            {
                bgAudioSource.Play();
                is_playing = true;
            }
        }
    }
    public static void PauseAudio()
    {
        bgAudioSource.Pause();
        is_playing = false;
    }
    public static void ResumeAudio()
    {
        bgAudioSource.UnPause();
        is_playing = true;
    }
    public static void UpdateAudioDic(Dictionary<string, AudioClip> d)
    {
        audioDic = d;
    }
    public static void SetVolume(float f)
    {
        bgAudioSource.volume = f;
    }
}
