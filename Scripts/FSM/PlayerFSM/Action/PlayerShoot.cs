using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour, IState
{
    private GunController _gunController;
    private Animator _animator;

    private CoroutineFSM _FSM;

    public void Init()
    {
        _gunController = GetComponent<GunController>();
        _animator = GetComponent<Animator>();
        _FSM = GetComponent<CoroutineFSM>();
    }

    public void Action()
    {
        ShootAction();
    }

    private void ShootAction()
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

        GetComponent<RootFSM>().SetState(State.Move);
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

    //public override void Enter(State state)
    //{
    //    if (_FSM._currState == state) return;

    //    _FSM._currState = State.Shoot;
    //    _FSM._prevState = state;
    //}

    //public override void Execute()
    //{
    //    switch (_gunController._equipedGun._gunType)
    //    {
    //        case GunType.Pistol:
    //            _animator.SetInteger("GUNTYPE", 0);
    //            break;
    //        case GunType.ShotGun:
    //            _animator.SetInteger("GUNTYPE", 1);
    //            break;
    //        case GunType.Auto:
    //            _animator.SetInteger("GUNTYPE", 2);
    //            break;
    //    }
    //    _animator.SetBool("SHOOTIDLE", true);
    //    _animator.SetBool("SHOOTBOOL", true);

    //    _gunController.OntriggerHold();
    //}

    //public override void Exit()
    //{
    //    _FSM.SetState(_FSM._prevState);
    //}
}
