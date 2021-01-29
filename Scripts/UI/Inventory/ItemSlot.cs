using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler 
{
    public Item _item;
    public BulletItem _bItem;
    public int _itemCount;
    public Image _itemIcon;

    private TextMeshProUGUI _textCount;
    private RaycastHit _rayHit;

    private Rect _inventoryRect;
    private Rect _quickSlotR;

    private QuickSlot _quickSlot;
    private RectTransform _quickSlotRect;
    private bool IsQuickSlot { get; set; } = false;

    private Player _player;

    private ItemDrop _itemDrop;

    private InventoryUI _inventory;

    public void Init(bool isQuickSlot = false)
    {
        _textCount = GetComponentInChildren<TextMeshProUGUI>();
        _inventoryRect = transform.parent.parent.GetComponent<RectTransform>().rect;

        _quickSlot = FindObjectOfType<QuickSlot>();

        _quickSlotRect = _quickSlot.GetComponent<RectTransform>();
        _quickSlotR = _quickSlot.GetComponent<RectTransform>().rect;
        IsQuickSlot = isQuickSlot;

        _player = FindObjectOfType<Player>();
        _itemDrop = FindObjectOfType<ItemDrop>();
        _inventory = FindObjectOfType<InventoryUI>();
    }

    // 아이템 확득 시 인벤토리 추가
    public void AddItemSlot(Item item, int itemCount = 1)
    {
        _item = item;
        _itemCount = itemCount;
        _itemIcon.sprite = item._sprite;

        if (item._itemType != ItemType.Equipment)
            _textCount.text = _itemCount.ToString();
        else if (item._itemType == ItemType.Equipment)
        {
            if (item._equipmentUpgrade == 0)
                _textCount.text = "";
            else
                _textCount.text = item._equipmentUpgrade.ToString();
        }
        else
            _textCount.text = "";

        SetColor(1);
    }

    public void AddItemSlot(BulletItem item)
    {
        _bItem = item;
        _itemCount = item._value;
        _itemIcon.sprite = item._image;
        _textCount.text = _itemCount.ToString();
        SetColor(1);
    }

    // 알파값 조절
    private void SetColor(float alpha)
    {
        Color color = _itemIcon.color;
        color.a = alpha;
        _itemIcon.color = color;
    }

    public void SetSlotCount(int count)
    {
        _itemCount += count;
        _textCount.text = _itemCount.ToString();

        if (_itemCount <= 0)
            ClearSlot();
    }

    public void ClearSlot()
    {
        _item = null;
        _itemCount = 0;
        _itemIcon.sprite = null;
        _textCount.text = "";
        SetColor(0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                if (eventData.clickCount == 2)
                {
                    if (_item._itemType == ItemType.Use)
                    {
                        ItemMng.Instance.UsedItem(_item);

                        if (!IsQuickSlot)
                            SetSlotCount(-1);
                        else if (IsQuickSlot)
                        {
                            //Debug.Log(_item.name);
                            for (int  i = 0; i < _inventory._itemSlot.Length; i++)
                            {
                                if (_inventory._itemSlot[i]._item._name == _item._name)
                                {
                                    _inventory._itemSlot[i].SetSlotCount(-1);
                                    SetSlotCount(-1);
                                    return;
                                }
                            }
                        }
                    }
                    return;
                }
            }
        }
        if (eventData.button == PointerEventData.InputButton.Right && !IsQuickSlot)
            if (_item != null)
            {
                if (_item._itemType == ItemType.Use)
                {
                    UIMng.Instance.CallEvent(UIList.ShowItem, "Open", _item);
                }
                else if (_item._itemType == ItemType.Equipment)
                {
                    Debug.Log(_item._uniqueID);
                    //Debug.Log(_player._gunController._gunDic[_item._uniqueID]);

                    if (_player._gunController._gunDic[_item._uniqueID] != null)
                        UIMng.Instance.CallEvent(UIList.GunUpgrade, "Open", _item._uniqueID);
                }
                else if (_item._itemType == ItemType.Projectile)
                    return;
            }
        //UIMng.Instance.CallEvent(UIList.ShowItem, "Open", _item);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            DragSlot.instance._itemSlot = this;
            DragSlot.instance.DragSetImage(_itemIcon);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float x = DragSlot.instance.transform.localPosition.x;
        float y = DragSlot.instance.transform.localPosition.y;

        if (_item != null && DragSlot.instance._itemSlot != null)
        {
            if (!((x > _inventoryRect.xMin && x < _inventoryRect.xMax && y > _inventoryRect.yMin && y < _inventoryRect.yMax)
            || (x > _quickSlotR.xMin && x < _quickSlotR.xMax && y > _quickSlotRect.transform.localPosition.y - _quickSlotR.yMax && y < _quickSlotRect.transform.localPosition.y - _quickSlotR.yMin)))
            {
                if (!DragSlot.instance._itemSlot.IsQuickSlot)
                {
                    _itemDrop.CallEvent();
                    return;
                }
                ClearSlot();
                DragSlot.instance.SetColor(0);
                DragSlot.instance._itemSlot = null;
            }
            else
            {
                DragSlot.instance.SetColor(0);
                DragSlot.instance._itemSlot = null;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance._itemSlot != null)
        {
            if (IsQuickSlot)
            {
                if (DragSlot.instance._itemSlot._item._itemType == ItemType.Use)
                {
                    if (DragSlot.instance._itemSlot.IsQuickSlot)
                        ChanegeQuickSlot();
                    else
                    {
                        if (CheckQuickSlot())
                            AddQuickSlot(DragSlot.instance._itemSlot._item);
                    }
                }
            }
            else
            {
                if (DragSlot.instance._itemSlot.IsQuickSlot) 
                   return;

                ChangeSlot();
            }
            DragSlot.instance.SetColor(0);
            DragSlot.instance._itemSlot = null;
        }
    }

    private bool CheckQuickSlot()
    {
        for (int i = 0; i < _quickSlot._slots.Length; i++)
        {
            if (_quickSlot._slots[i]._item != null)
            {
                if (DragSlot.instance._itemSlot._item == _quickSlot._slots[i]._item)
                    return false;
            }
        }
        return true;
    }

    private void ChangeSlot()
    {
        Item tempItem = _item;
        int tempItemCount = _itemCount;

        AddItemSlot(DragSlot.instance._itemSlot._item, DragSlot.instance._itemSlot._itemCount);

        if (tempItem != null)
            DragSlot.instance._itemSlot.AddItemSlot(tempItem, tempItemCount);
        else
            DragSlot.instance._itemSlot.ClearSlot();
    }

    private void ChanegeQuickSlot()
    {
        Item tempItem = _item;
        int tempItemCount = _itemCount;

        AddQuickSlot(DragSlot.instance._itemSlot._item);

        if (tempItem != null)
            DragSlot.instance._itemSlot.AddQuickSlot(tempItem);
        else
            DragSlot.instance._itemSlot.ClearSlot();
    }

    private void AddQuickSlot(Item item)
    {
        _item = item;
        _itemIcon.sprite = DragSlot.instance._itemSlot._item._sprite;

        SetColor(1);
    }
}
