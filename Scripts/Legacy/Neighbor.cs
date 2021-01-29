using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neighbor : MonoBehaviour
{
    private bool CheckNode(int row, int col)
    {
        if (row < 0 || row >= CreateNode.nodeCount)
            return false;
        if (col < 0 || col >= CreateNode.nodeCount)
            return false;

        return true;
    }

    public Node[] NeighborNode(Node node)
    {
        List<Node> neighbor = new List<Node>();

        for (int row = -1; row <= 1; ++row)
        {
            for (int col = -1; col <= 1; ++col)
            {
                if (row == 0 && col == 0)
                    continue;

                if (CheckNode(row + node.Row, col + node.Col))
                {
                    neighbor.Add(CreateNode.nodes[row + node.Row, col + node.Col]);
                }
            }
        }
        return neighbor.ToArray();
    }

    public Node FindNode(Vector3 pos)
    {
        for (int row = 0; row < CreateNode.nodeCount; ++row)
        {
            for (int col = 0; col < CreateNode.nodeCount; ++col)
            {
                Debug.Log("FindNodeIn");
                //Debug.Log(CreateNode.nodes[3, 4]);
                if (CreateNode.nodes[row,col].Contains(pos))
                {
                    Debug.Log("FindNodeI2");
                    return CreateNode.nodes[row, col];
                }
            }
        }
        return null;
    }
}
