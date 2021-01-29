using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private Player _player;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        Vector3 pos = _player.transform.position;
        pos.y = transform.position.y;
        transform.position = pos;
          }
}
