using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterGame : MonoBehaviour
{
    private Animator _animator;
    //private BoxCollider _collider;

    private BoxCollider[] _colliders;
    private bool _flag = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        //_collider = GetComponent<BoxCollider>();
        _colliders = GetComponents<BoxCollider>();
    }

    private void Open()
    {
        _animator.SetBool("OPEN", true);
        transform.Find("Light").gameObject.SetActive(true);
        //_collider.center = new Vector3(-2.5f, 2, -2.5f);
        //_collider.size = new Vector3(3, 3, 3);
        //_collider.isTrigger = true;

        _colliders[0].center = new Vector3(-2.5f, 2, -2.5f);
        _colliders[0].size = new Vector3(2.5f, 2.5f, 2.5f);
        _colliders[0].isTrigger = true;

        _colliders[1].enabled = false;

        _flag = true;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _flag)
        {
            UIMng.Instance.SetActive(UIList.Loading, true);
            SceneMng.Instance.Enable(SceneType.Game, true);
            _animator.SetBool("OPEN", false);
        }

       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
                Open();
        }
    }
}
