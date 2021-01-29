using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BTNode 
{
    protected NodeState _nodeState;
    public NodeState NodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
}

public enum NodeState { Failure, Running, Success }


