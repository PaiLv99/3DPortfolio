using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeBTNode : BTNode
{
    private float range;
    private Transform origin;
    private Transform player;

    public RangeBTNode(float range, Transform origin, Transform player)
    {
        this.range = range;
        this.origin = origin;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        float distance = Vector3.Distance(player.position, origin.position);
        return distance <= range ? NodeState.Success : NodeState.Failure;
    }
}
