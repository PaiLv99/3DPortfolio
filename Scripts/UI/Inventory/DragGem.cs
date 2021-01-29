using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragGem : MonoBehaviour
{
    public static DragGem instance;

    public GemSlot _dragGemSlot = null;
    private Image _gemImage;

    // 드래그한 아이템의 아이콘을 표시함.
    public void DragSetImage(Image itemImage)
    {
        _gemImage.sprite = itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float alpha)
    {
        Color color = _gemImage.color;
        color.a = alpha;
        _gemImage.color = color;
    }

    void Start()
    {
        instance = this;
        _gemImage = GetComponent<Image>();
    }
}
