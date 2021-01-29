using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemUseType { Food, Medikit, HandLight, MysteryFood, Whisky }

public class ItemPool : BasePool
{
    private readonly int _count = 2;
    private readonly string Path = "Prefab/Items/";
    private GameObject[] _prefabs;
    private GameObject _itemBoxPrefab;
    private ItemInfo[] _infos;

    //private readonly string pPath = "Prefab/Items/";
    //private readonly string iPath = "Image/Items/";

    public override void Init()
    {
        _poolType = PoolType.ItemPool;
        _pool = new Dictionary<string, Queue<GameObject>>();

        _itemBoxPrefab = Resources.Load<GameObject>("Prefab/Box/Itembox");
        _prefabs = Resources.LoadAll<GameObject>("Prefab/Items");

        _infos = new ItemInfo[_prefabs.Length];
        SetInfo();
        ItemSet();

        for (int i = 0; i < _prefabs.Length; i++)
            AddDic(_prefabs[i].name, _prefabs[i], _count);

        AddDic(_itemBoxPrefab.name, _itemBoxPrefab, _count);
    }

    private void ItemSet()
    {
        for (int i = 0; i < _infos.Length; i++)
            for (int j = 0; j < _prefabs.Length; j++)
                if (_infos[i].NAME == _prefabs[j].name)
                {
                    if (_prefabs[j].GetComponent<Item>() != null)
                        _prefabs[j].GetComponent<Item>().SetItem(_infos[i]);
                }
    }

    private void SetInfo()
    {
        for (int i = 0; i < _prefabs.Length; i++)
            _infos[i] = DataMng.Instance.Table(TableType.ItemTable, _prefabs[i].name) as ItemInfo;
    }

    public override GameObject Pop(string str)
    {
        if (_pool[str].Count == 0)
            Add(str, Path + str);

        if (_pool.ContainsKey(str))
        {
            return _pool[str].Dequeue();
        }
        return null;
    }

    public override void Push(GameObject obj, string str)
    {
        if (_pool.ContainsKey(str))
        {
            obj.transform.SetParent(transform);
            _pool[str].Enqueue(obj);
            obj.SetActive(false);
        }
    }

    public override void PushAll()
    {
        Item[] objs = GetComponentsInChildren<Item>();

        if (objs != null)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].gameObject.activeSelf)
                {
                    if (objs[i]._name != null)
                    {
                        string str = objs[i]._name;

                        if (_pool.ContainsKey(str))
                        {
                            _pool[str].Enqueue(objs[i].gameObject);
                            objs[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }

}
