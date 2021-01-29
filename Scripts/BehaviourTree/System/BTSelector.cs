using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public BTSelector(List<BTNode> nodes)
    {
        children = nodes;
    }

    public override NodeState Evaluate()
    {
        foreach(var node in children)
        {
            switch(node.Evaluate())
            {
                case NodeState.Running:
                    _nodeState = NodeState.Running;
                    return _nodeState;
                case NodeState.Success:
                    _nodeState = NodeState.Success;
                    return _nodeState;
                case NodeState.Failure: break;
            }
        }

        _nodeState = NodeState.Failure;
        return _nodeState;
    }
}




