using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsMeleeAvailable : BTNode
{
    private Transform origin;
    private Transform player;

    public IsMeleeAvailable(Transform origin, Transform player)
    {
        this.origin = origin;
        this.player = player;
    }

    public override NodeState Evaluate()
    {


        throw new System.NotImplementedException();
    }
}
