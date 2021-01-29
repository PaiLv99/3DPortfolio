using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    private Animator _animator;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
    //    {
    //        _animator = GetComponent<Animator>();
    //        _animator.SetTrigger("OPEN");
    //        transform.Find("Light").gameObject.SetActive(true);
    //    }
    //}

    public void PlayerIn(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool("OPEN", true);
            transform.Find("Light").gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _animator.SetBool("OPEN", false);
            transform.Find("Light").gameObject.SetActive(false);

        }
    }
}
