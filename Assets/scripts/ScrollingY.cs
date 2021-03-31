using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingY : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject quadGameObject;
    private Renderer quadRenderer;
    private Vector2 Offset;
    float scrollSpeed = 0.002f;

    void Start()
    {
        quadGameObject = gameObject;
        quadRenderer = quadGameObject.GetComponent<Renderer>();
        Offset = GetComponent<Renderer>().material.mainTextureOffset;
    }

    void Update()
    {
        Offset = new Vector2(Offset[0], Offset[1] + (Time.deltaTime * scrollSpeed));
        quadRenderer.material.mainTextureOffset = Offset;
    }
}
