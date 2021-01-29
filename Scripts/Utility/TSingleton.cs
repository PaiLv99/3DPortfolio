using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class  TSingleton<T> : MonoBehaviour where T : TSingleton<T>
{
    private static T instance;
    
    public virtual void Init()
    {

    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Helper.CreateObject<T> (null, true);
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}
