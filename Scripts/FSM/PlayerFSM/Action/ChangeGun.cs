using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGun : MonoBehaviour, IState
{
    private GunController _controller;


    public void Action()
    {
        ChangeAction();
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }

    public void Init()
    {
        _controller = GetComponent<GunController>();
    }

    private void ChangeAction()
    {
        //_controller.ChangeGun();
    }
}
