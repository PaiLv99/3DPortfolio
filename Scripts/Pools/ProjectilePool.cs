using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectilePoolType { Bullet, Cartridge, }

public class ProjectilePool : BasePool
{
    private GameObject[] _prefabs;
    private GameObject _grenadePrefab;

    private readonly int _count = 100;
    private readonly string Path = "Prefab/Guns/Projectile";
    private readonly string GrenadePath = "Prefab/Guns/Grenade";

    public override void Init()
    {
        _poolType = PoolType.ProjectilePool;
        _pool = new Dictionary<string, Queue<GameObject>>();

        _prefabs = Resources.LoadAll<GameObject>(Path);

        for (int i = 0; i < _prefabs.Length; i++)
            AddDic( _prefabs[i].name, _prefabs[i], _count);

        _grenadePrefab = Resources.Load<GameObject>("Prefab/Guns/Grenade/GrenadeThrow");

        AddDic(_grenadePrefab.name, _grenadePrefab, 3);
    }

    public override GameObject Pop(string str = null)
    {
        if (_pool[str].Count == 0)
            Add(str, Path + str);

        GameObject obj = _pool[str].Dequeue();
        obj.gameObject.SetActive(true);

        return obj;
    }

    public override void Push(GameObject obj, string str)
    {
        _pool[str].Enqueue(obj);
        obj.transform.position = transform.position;
        obj.gameObject.SetActive(false);
    }

    public override void PushAll()
    {
        Projectile[] projectiles = GetComponentsInChildren<Projectile>();
        Cartridge[] cartridges = GetComponentsInChildren<Cartridge>();

        if (projectiles != null)
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (projectiles[i].gameObject.activeSelf)
                {
                    _pool["Bullet"].Enqueue(projectiles[i].gameObject);
                    projectiles[i].gameObject.SetActive(false);
                }
            }

        if (cartridges != null)
            for (int i = 0; i < cartridges.Length; i++)
                if (cartridges[i].gameObject.activeSelf)
                {
                    _pool["Cartridge"].Enqueue(cartridges[i].gameObject);
                    cartridges[i].gameObject.SetActive(false);
                }
    }
}
