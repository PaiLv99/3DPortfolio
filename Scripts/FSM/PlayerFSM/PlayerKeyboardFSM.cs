using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerKeyboardFSM : MonoBehaviour
{
    private Player _player;
    private Dictionary<PlayerState, IState> _actionDic = new Dictionary<PlayerState, IState>();
    public IState _IAction;
    public PlayerState _state;

    public void Init()
    {
        _player = GetComponent<Player>();

        IState move = new PlayerMove();
        IState idle = new Idle();
        IState interaction = new Interaction();
        IState skill = new Skill();
        IState changeFireMode = new ChangeFireMode();
        IState reroad = new Reroad();
        IState shoot = new PlayerShoot();

        move = gameObject.AddComponent<PlayerMove>();
        idle = gameObject.AddComponent<PlayerIdle>();
        interaction = gameObject.AddComponent<Interaction>();
        skill = gameObject.AddComponent<Skill>();
        changeFireMode = gameObject.AddComponent<ChangeFireMode>();
        reroad = gameObject.AddComponent<Reroad>();
        shoot = gameObject.AddComponent<PlayerShoot>();

        move.Init();
        idle.Init();
        interaction.Init();
        skill.Init();
        changeFireMode.Init();
        reroad.Init();
        shoot.Init();

        _actionDic.Add(PlayerState.Move, move);
        _actionDic.Add(PlayerState.Idle, idle);
        _actionDic.Add(PlayerState.Interaction, interaction);
        _actionDic.Add(PlayerState.Skill, skill);
        _actionDic.Add(PlayerState.ChangeFireMode, changeFireMode);
        _actionDic.Add(PlayerState.Reroad, reroad);
        _actionDic.Add(PlayerState.Shoot, shoot);
    }


    public void SetState(PlayerState state)
    {
        _state = state;
    }

    //private void ActionType(PlayerState state)
    //{
    //    switch(state)
    //    {
    //        case PlayerState.Idle: _actionDic[PlayerState.Idle].Action(); break;
    //        case PlayerState.Move: _actionDic[PlayerState.Move].Action(); break;
    //        case PlayerState.Interaction: _actionDic[PlayerState.Interaction].Action(); break;
    //        case PlayerState.Skill: _actionDic[PlayerState.Skill].Action(); break;
    //        case PlayerState.ChangeFireMode: _actionDic[PlayerState.ChangeFireMode].Action(); break;
    //        case PlayerState.Reroad: _actionDic[PlayerState.Reroad].Action(); break;
    //        case PlayerState.Bomb: _actionDic[PlayerState.Bomb].Action(); break;
    //        case PlayerState.Shoot: _actionDic[PlayerState.Shoot].Action(); break;
    //    }
    //}

    private IEnumerator IEStateChecker()
    {
        while (!_player.IsDeath)
        {
            yield return null;
            switch (_state)
            {
                case PlayerState.Idle:
                    if (Input.GetAxis("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
                        _state = PlayerState.Move;
                    break;
                case PlayerState.Move:
                    if (Input.GetKeyDown(KeyCode.R))
                        _state = PlayerState.Reroad;
                    else if (Input.GetKeyDown(KeyCode.E))
                        _state = PlayerState.ChangeFireMode;
                    else if (Input.GetKeyDown(KeyCode.F))
                        _state = PlayerState.Interaction;
                    else if (Input.GetMouseButtonDown(0) && !_player.IsBomb)
                        _state = PlayerState.Shoot;
                    else if (Input.GetMouseButton(0) && _player.IsBomb)
                        _state = PlayerState.Bomb;
                    else if (Input.GetMouseButtonUp(0))
                        _state = PlayerState.Idle;
                    break;
            }
        }
    }



}
