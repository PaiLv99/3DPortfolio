using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeFireMode : MonoBehaviour, IState
{
    private GunController _controller;

    public void Action()
    {
        ChangeAction();
    }

    public void Enter()
    {
        throw new NotImplementedException();
    }

    public void Execute()
    {
        throw new NotImplementedException();
    }

    public void Exit()
    {
        throw new NotImplementedException();
    }

    public void Init()
    {
        _controller = GetComponent<GunController>();
    }

    private void ChangeAction()
    {
        _controller.ChangeFireMode();
    }
}
