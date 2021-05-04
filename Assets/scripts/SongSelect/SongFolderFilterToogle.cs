using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongFolderFilterToogle : MonoBehaviour
{
    public Toggle toggle1;
    public string setname;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ToggleCallValueChanged(Toggle toggle)
    {
        Debug.Log("Toggle: " + toggle + " is " + toggle.isOn);
        if (toggle.isOn)
        {
            CardSelectManager.AddFilter(setname);
            CardSelectManager.AddAllCardOfFolder(setname);
        }
        else
        {
            CardSelectManager.RemoveFilter(setname);
            CardSelectManager.RemoveAllCardOfFolder(setname);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
