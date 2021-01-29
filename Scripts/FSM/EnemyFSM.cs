using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState { Idle, Chasing, Attack, Shoot, }

public class EnemyFSM : MonoBehaviour
{
    public EnemyState _state;
    public EnemyState _prevState;

    private Player _player;
    private Enemy _enemy;

    private Dictionary<EnemyState, IState> _actionDic = new Dictionary<EnemyState, IState>();

    public Chasing _chasing;
    private float _elapsedTime; 

    public void SetState(EnemyState state)
    {
        _state = state;
    }

    //private IEnumerator ActionType()
    //{
    //    while (!_player.IsDeath)
    //    {
    //        switch (_state)
    //        {
    //            case EnemyState.Idle:
    //                _actionDic[_state].Action(); break;
    //            case EnemyState.Attack:
    //                _actionDic[_state].Action(); break;
    //            case EnemyState.Chasing:
    //                _actionDic[_state].Action(); break;
    //            case EnemyState.Shoot:
    //                _elapsedTime += Time.deltaTime;
    //                if (_elapsedTime >= 1)
    //                {
    //                    _actionDic[_state].Action();
    //                    _elapsedTime = 0;
    //                }
    //                break;
    //        }
    //        yield return null;
    //    }
    //}

    private void Action()
    {
        if (_player.IsDeath)
            return;

        if (_state == _prevState)
            return;

        //_actionDic[_prevState].Exit();

        switch (_state)
        {
            case EnemyState.Idle: _actionDic[_state].Enter(); break;
            case EnemyState.Attack: _actionDic[_state].Enter(); break;
            case EnemyState.Chasing: _actionDic[_state].Enter(); break;
            case EnemyState.Shoot: _actionDic[_state].Enter(); break;
        }
    }


    public IEnumerator IEStateCheker()
    {
        while (!_player.IsDeath)
        {
            yield return null;

            if (_player.IsDeath)
                break;

            float distance = (_player.transform.position - transform.position).magnitude;

            if (distance <= _enemy._attackDistance)
                _state = EnemyState.Attack;
            else if (distance <= _enemy._rangeDistance && _enemy._enemyType != EnemyType.Melee)
                _state = EnemyState.Shoot;
            else if (distance <= _enemy._chasingDistance)
                _state = EnemyState.Chasing;
            else
                _state = EnemyState.Idle;

            Action();
        }
    }
    public void Init()
    {
        _enemy = GetComponent<Enemy>();
        _player = FindObjectOfType<Player>();

        _chasing = gameObject.AddComponent<Chasing>();
        if (_enemy._enemyType != EnemyType.Melee)
        {
            IState shoot = gameObject.AddComponent<EnemyShoot>();

            if (_enemy._enemyType != EnemyType.Melee)
                shoot.Init();
            _actionDic.Add(EnemyState.Shoot, shoot);
        }

        IState meleeAttack = gameObject.AddComponent<MeleeAttack>();
        IState idle = gameObject.AddComponent<Idle>();

        _chasing.Init();
        meleeAttack.Init();
        idle.Init();

        _actionDic.Add(EnemyState.Attack, meleeAttack);
        _actionDic.Add(EnemyState.Idle, idle);
        _actionDic.Add(EnemyState.Chasing, _chasing);

        SetState(EnemyState.Idle);
        StartCoroutine(IEStateCheker());
        //StartCoroutine(ActionType());
    }
}
