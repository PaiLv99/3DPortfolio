using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class GemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public enum GemSlotType { Damage, Power, Speed }

    public Gem _gem;
    public Image _gemImage;
    public int _gemCount;

    private TextMeshProUGUI _gemCountText;
    private GemSlotType _gemSlotType;
    private bool _isSocket = false;

    public void Init(GemSlotType type, bool state = false)
    {
        _gemCountText = Helper.Find<TextMeshProUGUI>(transform, "Text");
        _gemImage = Helper.Find<Image>(transform, "GemImage");
        _gemSlotType = type;
        _isSocket = state;
    }

    public void GemAddSlot(Gem gem)
    {
        _gem = gem;
        _gemImage.sprite = gem._gemImage;
        SetSlotCount(1);
        SetColor(1);
    }

    private void SetColor(float alpha)
    {
        Color color = _gemImage.color;
        color.a = alpha;
        _gemImage.color = color;
    }

    public void SetSlotCount(int count)
    {
        _gemCount += count;
        _gemCountText.text = _gemCount.ToString();

        if (_gemCount <= 0)
            ClearSlot();
    }

    private void ClearSlot()
    {
        _gem = null;
        _gemCount = 0;
        _gemCountText.text = "";
        _gemImage.sprite = null;
        SetColor(0);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_gem != null && !_isSocket)
        {
            UIMng.Instance.CallEvent(UIList.GunUpgrade, "SetPrevSlot", this);
            DragGem.instance._dragGemSlot = this;
            DragGem.instance.DragSetImage(_gemImage);
            DragGem.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_gem != null)
            DragGem.instance.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragGem.instance._dragGemSlot != null && this != DragGem.instance._dragGemSlot)
        {
            if (_gemSlotType == DragGem.instance._dragGemSlot._gemSlotType && DragGem.instance._dragGemSlot._gemCount >= 1)
            {
                if (_isSocket)
                {
                    AddSocket(DragGem.instance._dragGemSlot._gem);
                }
            }
        }
        DragGem.instance.SetColor(0);
        DragGem.instance._dragGemSlot = null;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_gem != null)
        {
            DragGem.instance.SetColor(0);
            DragGem.instance._dragGemSlot = null;
        }
    }

    private void AddSocket(Gem gem)
    {
        _gem = gem;
        _gemImage.sprite = DragGem.instance._dragGemSlot._gem._gemImage;
        SetColor(1);
        UIMng.Instance.CallEvent(UIList.GunUpgrade, "AddSlotCheck");
        UIMng.Instance.CallEvent(UIList.GunUpgrade, "SetPrevSlotCount");
    }
}
