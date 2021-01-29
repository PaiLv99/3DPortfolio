using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState { Move, Idle, Interaction, Skill, ChangeFireMode, Reroad, Shoot, Bomb, Dash, ChangeGun, Inventory }
public enum State { Move, Idle, Shoot, Skill, Reroad, Bomb, Dash, ChangeGun, Interacction }

public class RootFSM : MonoBehaviour
{
    public Dictionary<State, IRootState> _stateDic = new Dictionary<State, IRootState>();
    private Dictionary<State, IState> _actionDic = new Dictionary<State, IState>();

    private Player _player;
    private State _state;

    private IRootState _aim;

    public void Init()
    {
        //_player = FindObjectOfType<Player>();
        
        MoveFSM moveState = gameObject.AddComponent<MoveFSM>();
        IdleFSM idleState = gameObject.AddComponent<IdleFSM>();
        moveState.Init();
        idleState.Init();

        if (_stateDic.ContainsKey(State.Idle) && _stateDic.ContainsKey(State.Move))
        {
            _stateDic.Add(State.Idle, idleState);
            _stateDic.Add(State.Move, moveState);
        }

        SetState(State.Idle);
        CreateComponent();
        _aim.Enter();

        StartCoroutine(IEStateChecker());
    }

    private void CreateComponent()
    {
        _aim = gameObject.AddComponent<Aim>();

        _actionDic.Add(State.Move, gameObject.AddComponent<PlayerMove>());
        _actionDic.Add(State.Idle, gameObject.AddComponent<PlayerIdle>());

        gameObject.AddComponent<PlayerShoot>();
        gameObject.AddComponent<Interaction>();
        gameObject.AddComponent<ChangeGun>();
        gameObject.AddComponent<ChangeFireMode>();
        gameObject.AddComponent<Reroad>();
        gameObject.AddComponent<Dash>();
        gameObject.AddComponent<Skill>();


        //for (int i = 0; i < _actionDic.Count; i++)
        //_actionDic[i].Init();
    }

    private void ActionEnter(State state)
    {
        switch(state)
        {
            case State.Move: _stateDic[state].Enter(); break;
            case State.Idle: _stateDic[state].Enter(); break;
        }
    }

    private IEnumerator IEStateChecker()
    {
        while (!_player.IsDeath)
        {
            yield return null;
            switch (_state)
            {
                case State.Idle:
                    if (Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Horizontal") > 0)
                    {
                        _stateDic[State.Idle].Exit();
                        _state = State.Move;
                        ActionEnter(_state);
                    }
                    break;
                case State.Move:
                    if (Input.GetAxisRaw("Vertical") <= 0 || Input.GetAxisRaw("Horizontal") <= 0)
                    {
                        _stateDic[State.Move].Exit();
                        _state = State.Idle;
                        ActionEnter(_state);
                    }
                    break;
            }
        }
    }

    public void SetState(State state)
    {
        _state = state;
    }
}
