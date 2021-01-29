using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T> : MonoBehaviour
{
    public Queue<T> CreateObject(Transform parent, int count)
    {
        Queue<T> q = new Queue<T>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = new GameObject(typeof(T).Name, typeof(T));
            if (obj.GetComponent<T>() != null)
            {
                T t = obj.GetComponent<T>();
                q.Enqueue(t);
            }
        }
        return q;
    }

    public Dictionary<string, Queue<T>> CreatePool(Transform parent, int count)
    {
        Dictionary<string, Queue<T>> dic = new Dictionary<string, Queue<T>>();

        dic.Add("", CreateObject(parent, count));


        if (dic != null)
        {
            return dic;
        }
        return null;
    }
}
