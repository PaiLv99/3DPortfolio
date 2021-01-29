using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider))]
public class BaseItem : MonoBehaviour
{
    private Vector3 _pos;
    private Vector3 _offset;

    private void Start()
    {   
        _pos = transform.position;
        _offset = new Vector3(0, 0.5f, 0);
    }

    void MoveItem()
    {
        transform.position = new Vector3(transform.position.x, Mathf.PingPong(Time.time, 0.5f), transform.position.z) + _offset;
    }

    private void Update()
    {
        MoveItem();
    }

}
