using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//http://www.41post.com/4776/programming/unity-making-a-simple-audio-visualization
public class Spectrum : MonoBehaviour
{
    //An AudioSource object so the music can be played  
    private AudioSource aSource;
    //A float array that stores the audio samples  
    public float[] samples = new float[64];
    //A renderer that will draw a line at the screen  
    private LineRenderer lRenderer;
    //A reference to the cube prefab  
    public GameObject cube;
    //The transform attached to this game object  
    private Transform goTransform;
    //The position of the current cube. Will also be the position of each point of the line.  
    private Vector3 cubePos;
    //An array that stores the Transforms of all instantiated cubes  
    private Transform[] cubesTransform;
    //The velocity that the cubes will drop  
    private Vector3 gravity = new Vector3(0.0f, 0.25f, 0.0f);

    void Awake()
    {
        //Get and store a reference to the following attached components:  
        //AudioSource  
        this.aSource = GetComponent<AudioSource>();
        MusicPlayer.InitMusicPlayer(aSource);
    }

    void Start()
    {
        //The line should have the same number of points as the number of samples  
        lRenderer.SetVertexCount(samples.Length);
        //The cubesTransform array should be initialized with the same length as the samples array  
        cubesTransform = new Transform[samples.Length];
        //Center the audio visualization line at the X axis, according to the samples array length  
        goTransform.position = new Vector3(-samples.Length / 2, goTransform.position.y, goTransform.position.z);

        //Create a temporary GameObject, that will serve as a reference to the most recent cloned cube  
        GameObject tempCube;

        //For each sample  
        for (int i = 0; i < samples.Length; i++)
        {
            //Instantiate a cube placing it at the right side of the previous one  
            tempCube = (GameObject)Instantiate(cube, new Vector3(goTransform.position.x + i, goTransform.position.y, goTransform.position.z), Quaternion.identity);
            //Get the recently instantiated cube Transform component  
            cubesTransform[i] = tempCube.GetComponent<Transform>();
            //Make the cube a child of this game object  
            cubesTransform[i].parent = goTransform;
        }
    }

    void _Update()
    {
        
        //Obtain the samples from the frequency bands of the attached AudioSource  
        aSource.GetSpectrumData(this.samples, 0, FFTWindow.BlackmanHarris);

        //For each sample  
        for (int i = 0; i < samples.Length; i++)
        {
            /*Set the cubePos Vector3 to the same value as the position of the corresponding 
             * cube. However, set it's Y element according to the current sample.*/
            cubePos.Set(cubesTransform[i].position.x, Mathf.Clamp(samples[i] * (50 + i * i), 0, 50), cubesTransform[i].position.z);

            //If the new cubePos.y is greater than the current cube position  
            if (cubePos.y >= cubesTransform[i].position.y)
            {
                //Set the cube to the new Y position  
                cubesTransform[i].position = cubePos;
            }
            else
            {
                //The spectrum line is below the cube, make it fall  
                cubesTransform[i].position -= gravity;
            }

            /*Set the position of each vertex of the line based on the cube position. 
             * Since this method only takes absolute World space positions, it has 
             * been subtracted by the current game object position.*/
            lRenderer.SetPosition(i, cubePos - goTransform.position);
        }
        
    }
    /*
    private Material met;
    private Mesh mesh;
    public FFTWindow fftWindow = FFTWindow.Rectangular;
    public int sampleSection = 256;
    public float[] spectrum;
    public float[] spectrumLogValues;


    public float sampleInterval = 0.1f;
    public int logLevel = 0;
    public int dropSpeed = 1;
    public int maxHeight = 50;


    private float currentSampleTime = 0;
    private float[] currDropSpeeds;
    void Start()
    {
        mesh = gameObject.AddComponent<MeshFilter>().mesh;
        MeshRenderer mere = gameObject.AddComponent<MeshRenderer>();
        mere.material = met;
    }
    
    void Update()
    {
        if (currentSampleTime < sampleInterval)
        {
            currentSampleTime += Time.deltaTime;
            return;
        }
        else
            currentSampleTime -= sampleInterval;


        if (spectrum == null || spectrum.Length != sampleSection)
            spectrum = new float[sampleSection];
        if (spectrumLogValues == null || spectrumLogValues.Length != sampleSection)
            spectrumLogValues = new float[sampleSection];
        if (currDropSpeeds == null || currDropSpeeds.Length != sampleSection)
            currDropSpeeds = new float[sampleSection];


        AudioListener.GetSpectrumData(spectrum, 0, fftWindow);

        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            float currentVal = 0;
            if (logLevel == 0)
                currentVal = spectrum[i];
            if (logLevel < 0)
                currentVal = Mathf.Clamp(spectrum[i] * (maxHeight + i * i), 0, maxHeight);
            else
                currentVal = -Mathf.Log(spectrum[i], logLevel);


            if (spectrumLogValues[i] == 0 || currentVal - spectrumLogValues[i] >= Mathf.Abs(currDropSpeeds[i]))
            {
                currDropSpeeds[i] = 0;
                spectrumLogValues[i] = currentVal;
            }
            else
            {
                spectrumLogValues[i] = Mathf.Clamp(spectrumLogValues[i] - currDropSpeeds[i], 0, maxHeight);
                currDropSpeeds[i] += dropSpeed * Time.deltaTime;
            }
        }

        mesh.vertices = MakeVertex();
        mesh.triangles = VertexSort();
    }
    public Vector3[] MakeVertex()
    {
        //这里是三角形的三个顶点，可调节长短
        Vector3[] rec = new Vector3[sampleSection+2];
        rec[0] = new Vector3(0, 0, 0);

        for(int i = 1; i <= sampleSection; i++)
        {
            rec[i] = new Vector3(i, 0.5f * spectrumLogValues[i-1] , 0);
        }
        rec[sampleSection+1] = new Vector3(sampleSection, 0, 0);
        return rec;
    }
    /// <summary>
    /// 获取三角形排序
    /// </summary>
    /// <returns></returns>
    public int[] VertexSort()
    {
        //排序
        int[] rec = new int[sampleSection+2];
        for (int i = 0; i < sampleSection+2; i++)
        {
            rec[i] =i;
        }
        return rec;
    }
    
    void OnDrawGizmos()
    {
        for (int i = 1; i < spectrumLogValues.Length - 1; i++)
            Gizmos.DrawCube(new Vector3(i * 1, 0.5f * spectrumLogValues[i], 0), new Vector3(1, spectrumLogValues[i], 1));
    }
    */
}
