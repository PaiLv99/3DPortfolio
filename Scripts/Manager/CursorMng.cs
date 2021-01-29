using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorMng : MonoBehaviour
{
    private Image image;
    public Sprite weapon;
    public Sprite hand;

    private List<Image> m_tagImages = new List<Image>();

    public Sprite sprite;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Finish");
        //GameObject[] obj = GameObject.FindGameObjectsWithTag("UIPosition");
        for (int i = 0; i < objects.Length; ++i)
        
        {
            m_tagImages.Add(objects[i].GetComponent<Image>());
        }

    }
    
    void Update()
    {
        if (image == null)
            return;

        Vector3 pos = Input.mousePosition;
        pos.z = image.transform.position.z;
        image.transform.position = pos;

        bool state = false;
        foreach (Image img in m_tagImages)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(img.rectTransform, Input.mousePosition))
            {
                state = true;
            }

        }

        if (state)
            image.sprite = hand;
        else
            image.sprite = weapon;
    }
}
