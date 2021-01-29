using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePool : MonoBehaviour
{
    public PoolType _poolType;
    protected Dictionary<string, Queue<GameObject>> _pool;

    public virtual void Init()
    {

    }

    public virtual void Push(GameObject obj, string str =  null)
    {
        
    }

    public virtual GameObject Pop(string str = null)
    {
        return null;
    }

    public virtual void AddDic(string str,GameObject prefab, int count)
    {
        _pool.Add(str, CreateObject(prefab, count));
    }

    public virtual Queue<GameObject> CreateObject(GameObject prefab, int count)
    {
        Queue<GameObject> q = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            q.Enqueue(obj);
            obj.SetActive(false);
        }
        if (q != null)
            return q;

        return null;
    }

    public virtual GameObject AddObject(string path)
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>(path), transform);

        return obj;
    }

    public virtual void Add( string str, string path)
    {
        if (_pool.ContainsKey(str))
            _pool[str].Enqueue(AddObject(path));
    }

    public virtual void PushAll() 
    {
        Transform[] objs = GetComponentsInChildren<Transform>();

        if (objs != null)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].gameObject.activeSelf)
                {
                    if (objs[i].name != null)
                    {
                        string str = objs[i].name;

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
