using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSequence : BTNode
{
    protected List<BTNode> children = new List<BTNode>();

    public BTSequence(List<BTNode> nodes)
    {
        children = nodes;
    }

    public override NodeState Evaluate()
    {
        bool isAnyChildRunning = false;
        foreach(var node in children)
        {
            switch(node.Evaluate())
            {
                case NodeState.Running:
                    isAnyChildRunning = true;
                    break;
                case NodeState.Success:
                    break;
                case NodeState.Failure:
                    _nodeState = NodeState.Failure;
                    return _nodeState; 
            }
        }

        _nodeState = isAnyChildRunning ? NodeState.Running : NodeState.Success;
        return _nodeState;
    }
}




