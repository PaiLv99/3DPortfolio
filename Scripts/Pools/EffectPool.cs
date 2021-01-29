using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType { BombEffect, BoxOpenEffectO, DamagePopUp, DeathEffect, DustEffect, GemEffect, SpeedUp }

public class EffectPool : BasePool
{
    private readonly int _count = 2;
    private readonly string Path = "Effect/";
    private GameObject[] _prefabs;

    public override void Init()
    {
        _poolType = PoolType.EffectPool;
        _pool = new Dictionary<string, Queue<GameObject>>();
        _prefabs = Resources.LoadAll<GameObject>("Effect");

        for (int i = 0; i < _prefabs.Length; i++)
            AddDic(_prefabs[i].name, _prefabs[i], _count);
    }

    public override GameObject Pop(string str)
    {
        if (_pool[str].Count == 0)
            Add(str, Path + str);
           
        if (_pool.ContainsKey(str))
            return _pool[str].Dequeue();
        return null;
    }

    public override void Push(GameObject obj, string str)
    {
        if (_pool.ContainsKey(str))
        {
            obj.transform.parent = transform;
            _pool[str].Enqueue(obj);
            obj.SetActive(false);
        }
    }
}
