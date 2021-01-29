using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : BaseUI, IDragHandler, IBeginDragHandler
{
    public static bool _inventoryActive = false;

    // 인벤토리 게임 오브젝트
    private Transform _inventoryBG;
    private Transform _inventoryBar;
    private Transform _itemSoltParent;
    public ItemSlot[] _itemSlot;
    private Player _player;
    //public Gun curGun;
    private Button _button;
    private Vector3 _originPos;

    private Vector2 _offset;

    public override void Init()
    {
        _player = FindObjectOfType<Player>();

        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(() => { TOpenInventory(); });

        _itemSoltParent = Helper.Find<Transform>(transform, "Bar/BG/ItemSlotGrid");
        _itemSlot = _itemSoltParent.GetComponentsInChildren<ItemSlot>();

        for (int i = 0; i < _itemSlot.Length; i++)
            _itemSlot[i].Init();

        _inventoryBar = Helper.Find<Transform>(transform, "Bar");
        _inventoryBG = Helper.Find<Transform>(transform, "Bar/BG");
        _inventoryBar.gameObject.SetActive(false);
        _originPos = _inventoryBar.transform.position;
    }

    public void TOpenInventory()
    {
        _inventoryActive = !_inventoryActive;

        if (_inventoryActive)
            OpenInventory();
        else
            CloseInventory();
    }

    private void OpenInventory()
    {
        _inventoryBar.gameObject.SetActive(true);
        _inventoryBar.transform.position = _originPos;
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    private void CloseInventory()
    {
        _inventoryBar.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    public void AddItem(Item item)
    {
        if (item._itemType != ItemType.Equipment)
        {
            for (int i = 0; i < _itemSlot.Length; i++)
            {
                if (_itemSlot[i]._item == null)
                    break;
                if (_itemSlot[i]._item._name == item._name)
                {
                    _itemSlot[i].SetSlotCount(item._itemCount);
                    return;
                }
            }
        }

        for (int i = 0; i < _itemSlot.Length; i++)
        {
            if (_itemSlot[i]._item == null)
            {
                _itemSlot[i].AddItemSlot(item, item._itemCount);
                return;
            }
        }
    }

    //public void AddBullet(BulletItem item)
    //{
    //    ItemMng.Instance.AddProjectile(item, item._value);

    //    for (int i = 0; i < _itemSlot.Length; i++)
    //    {
    //        if (_itemSlot[i]._item._name == item._name)
    //            _itemSlot[i].SetSlotCount(item._value);
    //    }

    //    for (int i = 0; i < _itemSlot.Length; i++)
    //        if (_itemSlot[i]._item == null)
    //            _itemSlot[i].AddItemSlot(item);
    //}

    public bool CheckItemSlot(string str)
    {
        for (int i = 0; i < _itemSlot.Length; i++)
            if (_itemSlot[i]._item != null)
                if (_itemSlot[i]._item._name == str)
                    return true;
        return false;
    }

    public ItemSlot GetSlot(string str)
    {
        for (int i = 0; i < _itemSlot.Length; i++)
        {
            if (_itemSlot[i] != null)
                if (_itemSlot[i]._item._name == str)
                    return _itemSlot[i];
        }
        return null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _inventoryBar.position = eventData.position; // + distance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _inventoryBar.GetComponent<RectTransform>().pivot = new Vector2(.5f,.95f);
    }
}
