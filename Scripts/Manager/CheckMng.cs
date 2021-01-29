using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMng : TSingleton<CheckMng>
{
    private Player _player;
    private List<GameObject> _prevObstacle = new List<GameObject>();

    private const int Item = 0, Wall = 1, ItemBox = 2, Enter = 3;

    public override void Init()
    {
        _player = FindObjectOfType<Player>();
    }

    private void Update()
    {
        if (!_player.IsDeath)
        {
            CheckWall(_player.transform);
            CheckItem(_player.transform);
            CheckBullet(_player.transform);
        }
    }


    private void CheckInteraction()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        int layer = 1 << LayerMask.NameToLayer("Item");

        switch(layer)
        {
            case Item: break;
            case Enter: break;
            case Wall: break;
            case ItemBox: break;
        }
    }

    private void CheckItem(Transform player)
    {
        if (Physics.Raycast(player.position, player.forward, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Item")))
            UIMng.Instance.CallEvent(UIList.ActionUI, "ShowItemOpen", hit);
        else if (Physics.Raycast(player.position, player.forward, out RaycastHit enterHit, 1f,
                 1 << LayerMask.NameToLayer("Enter") | 1 << LayerMask.NameToLayer("ItemBox")))
            UIMng.Instance.CallEvent(UIList.ActionUI, "ShowOpen");
        else
            UIMng.Instance.CallEvent(UIList.ActionUI, "Disappear");
    }

    private void CheckWall(Transform player)
    {
        if (Physics.Raycast(player.position, Vector3.back, out RaycastHit hit, 1f, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            GameObject obj = hit.collider.gameObject;
            Color colorA = obj.GetComponent<MeshRenderer>().material.color;
            colorA.a = 0f;

            _prevObstacle.Add(obj);

            for (int i = 0; i < _prevObstacle.Count; i++)
            {
                if (obj == _prevObstacle[i])
                    obj.GetComponent<MeshRenderer>().enabled = false;
                else
                    _prevObstacle[i].GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < _prevObstacle.Count; i++)
            {
                _prevObstacle[i].GetComponent<MeshRenderer>().enabled = true;
            }
            _prevObstacle.Clear();
        }
    }

    private void SetWallColor()
    {
        for (int i = 0; i < _prevObstacle.Count; i++)
            _prevObstacle[i].SetActive(true);
    }

    private void CheckBullet(Transform player)
    {
        Collider[] colliders = Physics.OverlapSphere(player.position, 3f, 1 << LayerMask.NameToLayer("Item"));

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].transform.GetComponent<BulletItem>() != null)
                colliders[i].GetComponent<BulletItem>().Chase();
        }
    }

}
