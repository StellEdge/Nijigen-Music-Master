using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardSelectItem : MonoBehaviour
{
    public RawImage rImage;
    public Button m_Button;

    public int index;
    public bool state = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int card_per_line = 8;
        gameObject.transform.localPosition = new Vector3(-400+112*(index % card_per_line), -(index/ card_per_line) * 160 + GameObject.Find("CardListScroll").GetComponent<Scrollbar>().value * (CardSelectManager.CardNum - 1)/ card_per_line * 160, -1);
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
