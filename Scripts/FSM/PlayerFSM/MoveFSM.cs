using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFSM : MonoBehaviour, IRootState
{
    private Player _player;
    private Dictionary<PlayerState, IState> _actionDic = new Dictionary<PlayerState, IState>();
    public IState _IAction;
    public PlayerState _state;

    private bool Flag { get; set; } = false;

    public void Init()
    {
        _player = GetComponent<Player>();

        _actionDic.Add(PlayerState.Interaction, gameObject.GetComponent<Interaction>());
        _actionDic.Add(PlayerState.Skill, gameObject.GetComponent<Skill>());
        _actionDic.Add(PlayerState.ChangeFireMode, gameObject.GetComponent<ChangeFireMode>());
        _actionDic.Add(PlayerState.Reroad, gameObject.GetComponent<Reroad>());
        _actionDic.Add(PlayerState.Shoot, gameObject.GetComponent<PlayerShoot>());
        _actionDic.Add(PlayerState.Dash, gameObject.GetComponent<Dash>());

        Flag = true;
    }

    //private void ActionType(PlayerState state)
    //{
    //    switch (state)
    //    {
    //        case PlayerState.Idle: _actionDic[state].Action(); break;
    //        case PlayerState.Move: _actionDic[state].Action(); break;
    //        case PlayerState.Interaction: _actionDic[state].Action(); break;
    //        case PlayerState.Skill: _actionDic[state].Action(); break;
    //        case PlayerState.ChangeFireMode: _actionDic[state].Action(); break;
    //        case PlayerState.Reroad: _actionDic[state].Action(); break;
    //        case PlayerState.Bomb: _actionDic[state].Action(); break;
    //        case PlayerState.Shoot: _actionDic[state].Action(); break;
    //    }
    //}

    //private IEnumerator IEStateChecker()
    //{
    //    while (!_player.IsDeath)
    //    {
    //        yield return null;

    //        if (Input.GetKeyDown(KeyCode.R))
    //        {
    //            _state = PlayerState.Reroad;
    //            ActionType(_state);
    //        }
    //        if (Input.GetKeyDown(KeyCode.E))
    //        {
    //            _state = PlayerState.ChangeFireMode;
    //            ActionType(_state);
    //        }
    //        else if (Input.GetKeyDown(KeyCode.F))
    //        {
    //            _state = PlayerState.Interaction;
    //            ActionType(_state);
    //        }
    //        else if (Input.GetMouseButton(0) && !_player.IsBomb)
    //        {
    //            _state = PlayerState.Shoot;
    //            ActionType(_state);
    //        }
    //        else if (Input.GetMouseButton(0) && _player.IsBomb)
    //        {
    //            _state = PlayerState.Bomb;
    //            ActionType(_state);
    //        }
    //        else if (Input.GetMouseButtonUp(0))
    //        {
    //            switch(_state)
    //            {
    //                case PlayerState.Shoot: ActionType(PlayerState.Shoot); break;
    //                case PlayerState.Bomb: ActionType(PlayerState.Bomb); break;
    //            }
    //        }
    //    }
    //}

    public void Enter()
    {
        if (!Flag)
            Init();

        //StartCoroutine(IEStateChecker());
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        //StopCoroutine(IEStateChecker());
    }
}
