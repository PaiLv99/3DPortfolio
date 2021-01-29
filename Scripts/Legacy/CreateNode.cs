using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNode : MonoBehaviour
{
    private Node _node;
    private Transform _root;

    public static Node[,] nodes;

    public static int nodeCount = 30;

    private void Init()
    {
        _node = Resources.Load<Node>("Node");
        _root = transform.Find("Root");
        CreateGrid();
    }

    private void CreateGrid()
    {
        nodes = new Node[nodeCount, nodeCount];
        float center = nodeCount / 2;
        for (int row = 0; row < nodeCount; ++row)
        {
            for (int col = 0; col < nodeCount; ++col)
            {
                Node node = Instantiate(_node, new Vector3(col + 0.5f - center, 0, - row + 0.5f + center), Quaternion.Euler(90, 0, 0), _root);

                node.SetNode(row, col);
                node.SetNodeType(NodeType.None);
                node.name = "Node" + node.Row + node.Col;

                nodes[row, col] = node;
            }
        }
    }

    private void Awake()
    {
        Init();
    }
}
