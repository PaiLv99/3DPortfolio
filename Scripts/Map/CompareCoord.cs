using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareCoord : IComparer<Node>
{
    public int Compare(Node a, Node b)
    {
        if ((int)a.Pos.x < (int)b.Pos.x)
            return -1;
        if ((int)a.Pos.x > (int)b.Pos.x)
            return 1;

        if ((int)a.Pos.x == (int)b.Pos.x)
        {
            if ((int)a.Pos.z < (int)b.Pos.z)
                return -1;
            if ((int)a.Pos.z > (int)b.Pos.z)
                return 1;
        }

        return 0;
    }
}
