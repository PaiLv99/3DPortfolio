using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour, IState
{
    private GetItem _getItem;

    public void Action()
    {
        InteractionAction();
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
        _getItem = GetComponent<GetItem>();
    }

    private void InteractionAction()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, 2.0f, 1 << LayerMask.NameToLayer("Item")))
            _getItem.Get();
        else if (Physics.Raycast(ray, 2.0f, 1 << LayerMask.NameToLayer("ItemBox")))
            _getItem.OpenBox();
        else if (Physics.Raycast(ray, 2.0f, 1 << LayerMask.NameToLayer("StartPoint")) 
                || Physics.Raycast(ray, 2.0f, 1 << LayerMask.NameToLayer("EndPoint")))
            TransferMapMng.Instance.Open();
    }
}
