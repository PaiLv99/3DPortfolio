using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;

    public ItemSlot _itemSlot = null;
    private Image _itemImage;

    // 드래그한 아이템의 아이콘을 표시함.
    public void DragSetImage(Image itemImage)
    {
        _itemImage.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = _itemImage.color;
        color.a = alpha;
        _itemImage.color = color;
    }

    void Start()
    {
        instance = this;
        _itemImage = GetComponent<Image>();
    }
}
