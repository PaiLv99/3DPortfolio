using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AState : MonoBehaviour
{
    public abstract void Enter(State state);
    public abstract void Execute();
    public abstract void Exit();
}
