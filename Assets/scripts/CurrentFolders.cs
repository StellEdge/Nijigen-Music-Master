using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CurrentFolders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Text>().text = "CurrentSongs:"+MusicWheelBase.GetWheelItemCount()+ " CurrentPacks:";
        foreach (string s in MusicLoader.GetFolderFilter())
        {
            gameObject.GetComponent<Text>().text = gameObject.GetComponent<Text>().text+s+" ";
        }
        
    }
}
