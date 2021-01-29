using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFSM : MonoBehaviour, IRootState
{
    private Dictionary<PlayerState, IState> _actionDic = new Dictionary<PlayerState, IState>();
    private PlayerState _state;
    private IState _action;
    private Player _player;

    public void Init()
    {
        _player = GetComponent<Player>();

        _state = PlayerState.Idle;

        _actionDic.Add(PlayerState.Interaction, gameObject.GetComponent<Interaction>());
        _actionDic.Add(PlayerState.Skill, gameObject.GetComponent<Skill>());
        _actionDic.Add(PlayerState.ChangeFireMode, gameObject.GetComponent<ChangeFireMode>());
        _actionDic.Add(PlayerState.Reroad, gameObject.GetComponent<Reroad>());
        _actionDic.Add(PlayerState.Shoot, gameObject.GetComponent<PlayerShoot>());
        _actionDic.Add(PlayerState.Dash, gameObject.GetComponent<Dash>());
    }

    private IEnumerator IEStateCheck()
    {
        while (!_player.IsDeath)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.R))
                _state = PlayerState.Reroad;
            if (Input.GetKeyDown(KeyCode.E))
                _state = PlayerState.ChangeFireMode;
            if (Input.GetKeyDown(KeyCode.F))
                _state = PlayerState.Interaction;
            if (Input.GetKeyDown(KeyCode.Space))
                _state = PlayerState.Skill;
            if (Input.GetMouseButtonDown(0))
                _state = PlayerState.Shoot;
            if (Input.GetAxis("Mouse ScrollWheel") > 0.5f)
                _state = PlayerState.ChangeGun;
            if (Input.GetAxis("Mouse ScrollWheel") < 0.5f)
                _state = PlayerState.ChangeGun;
        }
    }

    //private void ActionType(PlayerState state)
    //{
    //    switch(state)
    //    {
    //        case PlayerState.Shoot:
    //            _actionDic[state].Action(); break;
    //        case PlayerState.Bomb:
    //            _actionDic[state].Action(); break;
    //        case PlayerState.Interaction:
    //            _actionDic[state].Action(); break;
    //        case PlayerState.Reroad:
    //            _actionDic[state].Action(); break;
    //        case PlayerState.Skill:
    //            _actionDic[state].Action(); break;
    //        case PlayerState.ChangeFireMode:
    //            _actionDic[state].Action(); break;

    //    }
    //}

    public void Enter()
    {
        StartCoroutine(IEStateCheck());
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        StopCoroutine(IEStateCheck());
    }
}
