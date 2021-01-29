using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType { EffectPool, ItemPool, ProjectilePool, ZombiePool, BulletItemPool }

public class PoolMng : TSingleton<PoolMng>
{
    private Dictionary<PoolType, BasePool> _poolDic = new Dictionary<PoolType, BasePool>();

    public override void Init()
    {
        AddPool<ProjectilePool>(PoolType.ProjectilePool, true);
        AddPool<ZombiePool>(PoolType.ZombiePool, true);
        AddPool<ItemPool>(PoolType.ItemPool, true);
        AddPool<EffectPool>(PoolType.EffectPool, true);
        AddPool<BulletItemPool>(PoolType.BulletItemPool, true);
    }

    public T AddPool<T>(PoolType type, bool state = false) where T : BasePool
    {
        if (!_poolDic.ContainsKey(type))
        {
            T t = Helper.CreateObject<T>(transform, true);
            t.enabled = state;
            _poolDic.Add(type, t);
            return t;
        }
        return null;
    }

    public void Push(PoolType type, GameObject obj, string str = null)
    {
        switch(type)
        {
            case PoolType.ZombiePool: _poolDic[type].Push(obj, str); break;
            case PoolType.EffectPool: _poolDic[type].Push(obj, str); break;
            case PoolType.ItemPool: _poolDic[type].Push(obj, str); break;
            case PoolType.ProjectilePool: _poolDic[type].Push(obj, str); break;
            case PoolType.BulletItemPool: _poolDic[type].Push(obj, str); break;
        }
    }

    public GameObject Pop(PoolType type, string str = null)
    {
        if (_poolDic.ContainsKey(type))
        {
            switch (type)
            {
                case PoolType.ProjectilePool: return _poolDic[type].Pop(str);
                case PoolType.ZombiePool: return _poolDic[type].Pop(str);
                case PoolType.EffectPool: return _poolDic[type].Pop(str);
                case PoolType.ItemPool: return _poolDic[type].Pop(str);
                case PoolType.BulletItemPool: return _poolDic[type].Pop(str);
            }
        }
        return null;
    }

    public void CallEvent(PoolType type, string funcName)
    {
        if (_poolDic.ContainsKey(type))
            _poolDic[type].SendMessage(funcName, SendMessageOptions.DontRequireReceiver);
    }

    public void PushAll()
    {
        for (int i = 0; i < _poolDic.Count; i++)
        {
            if (_poolDic[(PoolType)i] == _poolDic[PoolType.ZombiePool])
                continue;

            _poolDic[(PoolType)i].PushAll();
        }
    }
}