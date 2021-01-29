using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Init();

    void Enter();
    void Execute();
    void Exit();
}
