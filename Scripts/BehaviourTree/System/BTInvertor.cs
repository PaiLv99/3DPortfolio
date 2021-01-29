using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTInvertor : BTNode
{
    protected BTNode child;

    public BTInvertor(BTNode child)
    {
        this.child = child;
    }

    public override NodeState Evaluate()
    {
        if (child.NodeState == NodeState.Failure)
            _nodeState = NodeState.Success;

        if (child.NodeState == NodeState.Success)
            _nodeState = NodeState.Failure;

        if (child.NodeState == NodeState.Running)
            _nodeState = NodeState.Running;

        return _nodeState;
    }
}
