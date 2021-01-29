using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AShoot : AState
{
    private GunController _gunController;
    private Animator _animator;

    private State _state;
    private State _prevState;

    public void Init()
    {
        _gunController = GetComponent<GunController>();
        _animator = GetComponent<Animator>();
    }

    public override void Enter(State state)
    {
        if (_state == state)
            return;

        _prevState = _state;
        _state = State.Idle;
        Execute();
    }

    public override void Exit()
    {
        _state = _prevState;
    }

    public override void Execute()
    {
        switch (_gunController._equipedGun._gunType)
        {
            case GunType.Pistol:
                _animator.SetInteger("GUNTYPE", 0);
                break;
            case GunType.ShotGun:
                _animator.SetInteger("GUNTYPE", 1);
                break;
            case GunType.Auto:
                _animator.SetInteger("GUNTYPE", 2);
                break;
        }
        _animator.SetBool("SHOOTIDLE", true);
        _animator.SetBool("SHOOTBOOL", true);
        _gunController.OntriggerHold();
    }
}
