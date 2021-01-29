using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsCoveredBTNode : BTNode
{
    private Transform origin;
    private Transform player;

    public IsCoveredBTNode(Transform origin, Transform player)
    {
        this.origin = origin;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        if (Physics.Raycast(origin.position, player.position - origin.position, out hit))
        {
            if (hit.transform != player)
                return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
