using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineFSM : MonoBehaviour
{
    private Dictionary<State, AState> _actionDic = new Dictionary<State, AState>();

    public State _currState;
    public State _prevState;

    public void SetState(State state)
    {
        _currState = state;
    }

    private IEnumerator IEChecker()
    {
        switch(_currState)
        {
            case State.Shoot: yield return StartCoroutine(""); break;
            case State.Reroad: yield return StartCoroutine(""); break;
        }
    }
}
