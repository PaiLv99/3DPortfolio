using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private GameObject[] _doors;
    public bool IsClear { get; set; } = false;

    private void Start()
    {
        _doors = GameObject.FindGameObjectsWithTag("Door");
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !IsClear)
        //{
        //    //StartCoroutine(IEDoorOpenClose(.5f, false));
        //    // 자신의 룸 넘버를 levelMng에게 보낸다.
        //    string roomName = transform.name;
        //    LevelMng.Instance.PlayerChecker(roomName);
        //    IsClear = true;
        //}
    }

    public IEnumerator IEDoorOpenClose(float targetTime, bool state)
    {
        yield return new WaitForSeconds(targetTime);
        for (int i = 0; i < _doors.Length; i++)
        {
            _doors[i].GetComponent<BoxCollider>().isTrigger = state;
        }
    }
}
