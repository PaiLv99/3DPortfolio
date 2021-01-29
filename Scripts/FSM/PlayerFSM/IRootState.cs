using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRootState
{
    void Enter();
    void Execute();
    void Exit();
}
