using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileData
{
    public string _projectileName;
    public int _value = 100;
    public BulletType _bulletType;
}

public class ItemMng : TSingleton<ItemMng>
{
    private Player _player;
    public ProjectileData[] _projectileDates;
    private InventoryUI _inventory;
    public Dictionary<string, ProjectileData> _projectileDic;

    public override void Init()
    {
        _inventory = FindObjectOfType<InventoryUI>();
        _projectileDates = new ProjectileData[4];
        _projectileDic = new Dictionary<string, ProjectileData>();

        for (int i = 0; i < _projectileDates.Length; i++)
        {
            _projectileDates[i] = new ProjectileData{ _bulletType = (BulletType)i };
        }

        _player = FindObjectOfType<Player>();
    }

    private void DictionInit(string str)
    {
        _projectileDic.Add(str, new ProjectileData());
    }

    public void SubProjectileCount(BulletType type, int count)
    {
        _projectileDates[(int)type]._value -= count;
    }

    public void AddProjectile(BulletItem item, int count)
    {
        _projectileDates[(int)item._bulletType]._value += count;

        // 인벤토리 UI 데이터와 변화
        if (_player._gunController._equipedGun._bulletType == item._bulletType)
            UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmo", item._bulletType);
    }

    public void UsedItem(Item item)
    {
        if (item._itemType == ItemType.Use)
        {
            for (int i = 0; i < item._parts.Length; i++)
            {
                switch(item._parts[i])
                {
                    case "HP": _player._status.HpUpdate(item._values[i]); break;
                    case "HUNGRY": _player._status.Food(item._values[i]); break;
                    case "SPEED": _player._status.SpeedUp(item._values[i]); break;
                    case "LIGHT": _player._status.HandLight(item._values[i]); break;
                    default: return;
                }
            }
        }
    }

    public void UsedQuickSlot(Item item)
    {
        int num = _inventory._itemSlot.Length;

        for (int i = 0; i < num; i++)
        {
            if (item == _inventory._itemSlot[i]._item)
            {
                UsedItem(item);
                _inventory._itemSlot[i].SetSlotCount(-1);
            }
        }
    }

    public int CheckValue(BulletType type)
    {
        return _projectileDates[(int)type]._value;
    }
}
