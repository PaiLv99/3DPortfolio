using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborLevel : MonoBehaviour
{
    //private bool CheckNode(int row, int col)
    //{
    //    if (row < 0 || row >= LevelGenerator.nodeCount)
    //        return false;
    //    if (col < 0 || col >= LevelGenerator.nodeCount)
    //        return false;
    //    if (LevelGenerator.nodes[row, col] == null)
    //        return false;

    //    return true;
    //}

    //public Node[] NeighborNode(Node node)
    //{
    //    List<Node> neighbor = new List<Node>();

    //    for (int row = -1; row <= 1; ++row)
    //    {
    //        for (int col = -1; col <= 1; ++col)
    //        {
    //            if (row == 0 && col == 0)
    //                continue;

    //            if (CheckNode(row + node.Row, col + node.Col))
    //                neighbor.Add(LevelGenerator.nodes[row + node.Row, col + node.Col]);
    //        }
    //    }
    //    return neighbor.ToArray();
    //}

    private bool CheckNode(Node[,] nodes, int row, int col)
    {
        if (row < 0 || row >= nodes.GetLength(0))
            return false;
        if (col < 0 || col >= nodes.GetLength(1))
            return false;
        if (nodes[row, col] == null)
            return false;

        return true;
    }

    public Node[] NeighborNode(Node[,] nodes, Node node)
    {
        List<Node> neighbor = new List<Node>();

        for (int row = -1; row <= 1; ++row)
        {
            for (int col = -1; col <= 1; ++col)
            {
                if (row == 0 && col == 0)
                    continue;

                if (CheckNode(nodes, row + node.Row, col + node.Col))
                {
                    neighbor.Add(nodes[row + node.Row, col + node.Col]);
                }
            }
        }
        return neighbor.ToArray();
    }

    //public Node FindNode(Vector3 pos)
    //{
    //    for (int row = 0; row < LevelGenerator.nodeCount; ++row)
    //    {
    //        for (int col = 0; col < LevelGenerator.nodeCount; ++col)
    //        {
    //            if (LevelGenerator.nodes[row,col].Contains(pos))
    //                return LevelGenerator.nodes[row, col];
    //        }
    //    }
    //    return null;
    //}
}
