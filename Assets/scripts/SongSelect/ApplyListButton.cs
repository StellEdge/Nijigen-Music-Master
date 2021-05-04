using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ApplyListButton : MonoBehaviour
{
    private Button m_Button;
    private GameObject cl;
    // Start is called before the first frame update
    void Start()
    {
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ButtonOnClickEvent()
    {
        CardSelectManager.InitCardListToCurrentList();
        return;
    }
}