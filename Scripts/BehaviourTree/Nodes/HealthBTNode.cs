using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBTNode : BTNode
{
    private EnemyController controller;

    public HealthBTNode(EnemyController controller)
    {
        this.controller = controller;
    }


    public override NodeState Evaluate()
    {
        return controller.CurrHP <= controller.threshold ? NodeState.Success : NodeState.Failure; 
    }
}
