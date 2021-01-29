using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Go to L
public class ItemToInventory : MonoBehaviour
{
    private Image _itemImage;

    // Start is called before the first frame update
    void Start()
    {
        _itemImage = GetComponent<Image>();
    }

    public void SetImage(Sprite itemImage)
    {
        _itemImage.sprite = itemImage;
        _itemImage.color = Color.white;
    }
}
