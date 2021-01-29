using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCheck : BTNode
{
    private Transform origin;
    private Transform player;

    public ObstacleCheck(Transform origin, Transform player)
    {
        this.origin = origin;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        RaycastHit hit;
        Vector3 dir = player.transform.position - origin.transform.position;
        if (Physics.Raycast(origin.position, dir, out hit))
        {
            if (hit.transform == player)
                return NodeState.Success;
        }
        return NodeState.Failure;
    }
}
