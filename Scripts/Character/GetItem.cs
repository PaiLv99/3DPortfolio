using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private readonly float _range = 3;
    private GunController _gunController;

    private void Start()
    {
        _gunController = FindObjectOfType<GunController>();
    }

    public void Get()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _range, 1 << LayerMask.NameToLayer("Item"));

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i] != null)
            {
                Vector3 dir = colliders[i].transform.position - transform.position;
                dir = dir.normalized;
                float angle = Vector3.Angle(transform.forward, dir);

                Item item = colliders[i].GetComponent<Item>();

                if (angle < 90)
                {
                    if (item != null && item._uniqueID != _gunController._equipedGun._item._uniqueID)
                    {
                        if (item._itemType == ItemType.Equipment)
                        {
                            colliders[i].transform.SetParent(_gunController._weaponPos);
                            colliders[i].transform.position = _gunController._weaponPos.position;
                            item._uniqueID = colliders[i].GetInstanceID();
                            _gunController._gunDic.Add(item._uniqueID, colliders[i].GetComponent<Gun>());
                            colliders[i].GetComponent<BoxCollider>().isTrigger = true;
                            colliders[i].gameObject.SetActive(false);
                            UIMng.Instance.CallEvent(UIList.Inventory, "AddItem", item);
                            UIMng.Instance.CallEvent(UIList.ActionUI, "ItemToInventory", item);
                        }
                        else if (item._itemType == ItemType.Use || item._name == "Grenade")
                        {
                            UIMng.Instance.CallEvent(UIList.Inventory, "AddItem", item);
                            UIMng.Instance.CallEvent(UIList.ActionUI, "ItemToInventory", item);
                            PoolMng.Instance.Push(PoolType.ItemPool, colliders[i].gameObject, item._name);
                        }

                        SoundMng.Instance.OncePlay("GetItem");
                    }
                }
            }
        }
    }

    public void OpenBox()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, _range, 1 << LayerMask.NameToLayer("ItemBox"));

        for (int i = 0; i < col.Length; i++)
            if (col[i] != null)
            {
                Vector3 dir = col[i].transform.position - transform.position;
                dir = dir.normalized;
                float angle = Vector3.Angle(transform.forward, dir);

                if (angle < 90)
                    col[i].GetComponent<ItemBox>().BoxOpen();
            }
    }
}
