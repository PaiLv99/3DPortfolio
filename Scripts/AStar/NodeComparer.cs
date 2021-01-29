using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeComparer : IComparer<Node>
{
    public int Compare(Node a, Node b)
    {
        if (a.FCost < b.FCost)
            return -1;
        if (a.FCost > b.FCost)
            return 1;
        if (a.FCost == b.FCost)
        {
            if (a.HCost < b.HCost)
                return -1;
            if (a.HCost > b.HCost)
                return 1;
        }
        return 0;
    }
}
