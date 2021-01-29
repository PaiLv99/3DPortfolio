using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    None,
    Wall,
}

public class Node : MonoBehaviour
{
    int _fCost;
    int _gCost;
    int _hCost;

    public int FCost { get { return _gCost + _hCost; } }
    public int GCost { get { return _gCost; } }
    public int HCost { get { return _hCost; } }

    public void SetGCost(int cost) { _gCost = cost; }
    public void SetHCost(int cost) { _hCost = cost; }

    int _row, _col;
    public int Row { get { return _row; } }
    public int Col { get { return _col; } }

    public void SetNode(int row, int col)
    {
        _row = row; _col = col;
    }


    public Node _parent;
    public Node Parent { get { return _parent; } }
    public void SetParent(Node parent) { _parent = parent; }

    // 노드 위치를 알리기 위한 프로퍼티 
    public Vector3 Pos
    { 
        get { return transform.position; }
        set { transform.position = value; }
    }

    private BoxCollider _collider;
    public bool Contains(Vector3 pos)
    {
        return _collider.bounds.Contains(pos);
    }

    public NodeType _nodeType;
    public NodeType NodeType { get { return _nodeType; } }
    public void SetNodeType(NodeType type) { _nodeType = type; }

    private void Init()
    {
        _collider = GetComponent<BoxCollider>();
    }

}
