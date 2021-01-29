using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BaseUI : MonoBehaviour
{
    public virtual void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    public virtual void Init()
    {

    }
}


