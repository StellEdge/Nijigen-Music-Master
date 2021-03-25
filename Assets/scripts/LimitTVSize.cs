using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LimitTVSize : MonoBehaviour
{
    // Start is called before the first frame update
    private Button m_Button;
    public bool LimitTvSize;
    private Sprite selected, normal;
    void Start()
    {
        LimitTvSize = false;
        m_Button = gameObject.GetComponent<Button>();
        m_Button.onClick.AddListener(ButtonOnClickEvent);
        selected = Resources.Load<Sprite>("limtvsize_selected");
        normal = Resources.Load<Sprite>("limtvsize");
    }
    // Update is called once per frame
    void Update()
    {
        if (LimitTvSize)
        {
            m_Button.GetComponent<Image>().sprite = selected;
        }
        else
        {
            m_Button.GetComponent<Image>().sprite = normal;
        }
    }
    public void ButtonOnClickEvent()
    {
        LimitTvSize = !LimitTvSize;
    }
}
