using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour, IState
{
    //private bool _flag = false;
    private float _elapsedTime;
    private float _targetTime = 2.0f;
    private Animator _animator;

    public bool DelayFlag { get; private set; } = false;

    public void Action()
    {
        if (!DelayFlag)
            SkillAction();

        StartCoroutine(IETime());
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        _animator = GetComponent<Animator>();
    }

    private IEnumerator IETime()
    {
        while (_elapsedTime >= _targetTime)
        {
            yield return null;

            _elapsedTime += Time.deltaTime;
        }
        DelayFlag = true;
        _elapsedTime = 0;
    }

    private void SkillAction()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2, 1 << LayerMask.NameToLayer("Enemy"));

        for (int i = 0; i < colliders.Length; ++i)
        {
            Vector3 dir = colliders[i].transform.position - transform.position;
            dir = dir.normalized;

            float angle = Vector3.Angle(transform.forward, dir);
            if (angle < 45f)
                colliders[i].GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
        }

        GetComponent<PlayerKeyboardFSM>().SetState(PlayerState.Idle);
    }
}
