using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : MonoBehaviour, IState
{
    private Animator _animator;
    private EnemyFSM _fsm;

    private EnemyState _currState;
    private EnemyState _prevState;

    public void Init()
    {
        _animator = GetComponent<Animator>();
        _fsm = GetComponent<EnemyFSM>();
    }

    public void Enter()
    {
        _prevState = _fsm._state;
        Execute();
    }

    public void Execute()
    {
        _animator.SetBool("IDLE", true);
        _animator.SetBool("SHOOTBOOl", false);
    }

    public void Exit()
    {
        _fsm._state = _prevState;
    }
}
