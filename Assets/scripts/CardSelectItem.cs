using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSelectItem : MonoBehaviour
{
    public RawImage rImage;
    public Button m_Button;

    public int index;
    private bool state;
    // Start is called before the first frame update
    void Start()
    {
        state = false;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.localPosition = new Vector3(-400+112*(index % 8), -(index/8) * 160 + GameObject.Find("CardListScroll").GetComponent<Scrollbar>().value * (CardSelectManager.CardNum - 1)/8 * 160, -1);
    }
    public void ButtonOnClickEvent()
    {
        if (!state)
        {
            Debug.Log("Add :" + CardSelectManager.mdata[index].title);
            CardSelectManager.AddCard(CardSelectManager.mdata[index]);
            rImage.color = Color.cyan;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Text>().color = Color.cyan;
            }
        }
        else
        {
            Debug.Log("Remove :" + CardSelectManager.mdata[index].title);
            CardSelectManager.RemoveCard(CardSelectManager.mdata[index]);
            rImage.color = Color.white;
            foreach (Transform child in transform)
            {
                child.gameObject.GetComponent<Text>().color = Color.white;
            }
        }
        state = !state;
    }
    private void OnDestroy()
    {
        Destroy(rImage.texture);
        //Debug.Log("OnDestroy");
    }
}
