using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMap : MonoBehaviour
{
    readonly float _targetTime = 1f;

    private Animator _animator;

    private BoxCollider[] _colliders;
    private bool _flag;
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _colliders = GetComponents<BoxCollider>();
    }

    public void Open()
    {
        _animator.SetBool("OPEN", true);
        transform.Find("Light").gameObject.SetActive(true);

        _colliders[0].center = new Vector3(-2.5f, 2, -2.5f);
        _colliders[0].size = new Vector3(4, 4, 4);
        _colliders[0].isTrigger = true;

        _colliders[1].enabled = true;

        _flag = true;
    }

    private IEnumerator IELoading(float target)
    {
        float elapsedTime = 0;

        while (elapsedTime < target)
        {
            elapsedTime += Time.deltaTime;

            UIMng.Instance.CallEvent(UIList.Loading, "SetProgress", elapsedTime);
            yield return null;
        }

        UIMng.Instance.CallEvent(UIList.Loading, "StopBlink");
        UIMng.Instance.SetActive(UIList.Loading, false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.F))
                Open();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _flag)
        {
            if (TransferMapMng.Instance.CheckBossRoom())
            {
                UIMng.Instance.SetActive(UIList.Loading, true);
                SceneMng.Instance.Enable(SceneType.Boss, true);
                return;
            }

            //UI Process
            UIMng.Instance.SetActive(UIList.Loading, true);
            StartCoroutine(IELoading(_targetTime));
            UIMng.Instance.FadeIn(true, _targetTime, null);


            // Translate
            _animator.SetBool("OPEN", false);
            StartCoroutine(IETranslatePlayer(other, _targetTime));
            UIMng.Instance.CallEvent(UIList.Result, "AddMapCount");
        }
    }

    private IEnumerator IETranslatePlayer(Collider other, float targetTime)
    {
        yield return new WaitForSeconds(targetTime);

        TransferMapMng.Instance.ActiveMap();
        other.transform.position = TransferMapMng.Instance.GetStartPosition(0) + new Vector3(1.5f, 0, .5f);
        TransferMapMng.Instance.StartPointSet(other);
        TransferMapMng.Instance.RemoveStartPoint(0);
        SoundMng.Instance.OncePlay("ElevatorBell");


    }
}
