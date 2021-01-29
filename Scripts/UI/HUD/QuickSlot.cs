using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class QuickSlot : MonoBehaviour
{
    public ItemSlot[] _slots = new ItemSlot[4];
    private InventoryUI _inventory;

    void Start()
    {
        _inventory = FindObjectOfType<InventoryUI>();

        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i] = transform.GetChild(i).GetComponent<ItemSlot>();
            _slots[i].Init(true);
        }
    }

    void FixedUpdate()
    {
        InputCheck();
    }

    private void InputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Execute(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Execute(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Execute(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            Execute(3);
    }

    private void Execute(int num)
    {
        if (_slots[num]._item != null)
        {
            if (_slots[num]._item._itemType == ItemType.Use)
            {
                ItemMng.Instance.UsedQuickSlot(_slots[num]._item);
                RemoveQuickSlot(_slots[num]._item, num);
            }
        }
    }

    private void RemoveQuickSlot(Item item, int num)
    {
        for (int i = 0; i < _inventory._itemSlot.Length; i++)
        {
            if (_slots[num]._item == _inventory._itemSlot[i]._item)
                return;
        }
        _slots[num].ClearSlot();
    }

    public void CheckInventory()
    {

    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    if (eventData.button == PointerEventData.InputButton.Left)
    //    {
    //        //ItemMng.Instance.UsedQuickSlot(_slots[num]._item);
    //    }
    //}

    //public void IsActivateedQuickSlot(int num)
    //{

    //}

    //public void SetSlotCount(int count)
    //{

    //}
}
