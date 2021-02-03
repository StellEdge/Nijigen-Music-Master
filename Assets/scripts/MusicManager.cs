using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NAudio;
using NAudio.Wave;

public class MusicManager : MonoBehaviour
{
    //单例对象化？
    private string TEST_SONG_PATH = "E:/SJTU备份箱/游戏设计/MusicData/CoreSet/015-刀剑神域Crossing field.mp3";
    private const string cLocalPath = "file://localhost/";
    private List<AudioClip> audioList;
    private static object audioList_lock = new object();


    IWavePlayer waveOutDevice;
    AudioFileReader audioFileReader;

    private void Awake()
    {

    }
    private AudioSource bgAudioSource;

    void Start()
    {
        bgAudioSource = gameObject.AddComponent<AudioSource>();
        bgAudioSource.playOnAwake = false;
        bgAudioSource.hideFlags = HideFlags.HideInHierarchy;
        bgAudioSource.transform.SetParent(this.transform);


        waveOutDevice = new WaveOut();
        audioFileReader = new AudioFileReader(TEST_SONG_PATH);
        waveOutDevice.Init(audioFileReader);
        waveOutDevice.Play();

        //foreach (AudioClip ac in audioList)
        //{
        //    audioDic.Add(ac.name, ac);
        //}

    }
    void Update()
    {

    }
    void OnApplicationQuit()
    {
        if (waveOutDevice != null)
        {
            waveOutDevice.Stop();
        }
        if (waveOutDevice != null)
        {
            waveOutDevice.Dispose();
            waveOutDevice = null;
        }
    }
    public void UpdateMusicList(List<MusicFolder> mfolders){
        //Get music folders from MusicLoader
        foreach(MusicFolder mf in mfolders)
        {
            StartCoroutine(ReadAndLoadMusicFolder(mf));
        }
    }

    private IEnumerator ReadAndLoadMusicFolder(MusicFolder mf)
    {
        lock (audioList_lock)
        {

        }
        yield break;
    }
    public void PlayAudio(string audioName, float volume = 1, bool isLoop = false)
    {

    }
    public void PauseAudio(int index)
    {
        bgAudioSource.Pause();
    }
    public void ResumeAudio(int index)
    {
        bgAudioSource.UnPause();
    }
}