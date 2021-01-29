using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : MonoBehaviour, IState
{
    private Animator _animator;
    private GunController _gController;
    private PlayerController _pController;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _gController = GetComponent<GunController>();
        _pController = GetComponent<PlayerController>();
    }

    public void Action()
    {
        IdleAction();
    }

    private void IdleAction()
    {
        _animator.SetBool("Idle", true);
        _animator.SetBool("SHOOTBOOL", false);
        _gController.OnTriggerRelease();
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
}
