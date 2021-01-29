using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ThreadPath : MonoBehaviour
{
    private Thread pathThread;
    private NeighborLevel neighbor;
    private NodeComparer comparer = new NodeComparer();

    public List<Order> orderList = new List<Order>();
    public List<Node> closedPath = new List<Node>();

    private bool state = false;

    private Node _start;
    private Node _target;
    private Node[,] _roomNodes;

    private int GetDistance(Node a, Node b)
    {
        int y = Mathf.Abs(a.Row - b.Row);
        int x = Mathf.Abs(a.Col - b.Col);

        return Mathf.Min(x, y) * 14 + Mathf.Abs(x - y) * 10;
    }

    private List<Node> TracePath(Node start, Node target)
    {
        List<Node> path = new List<Node>();
        Node currentNode = target;

        while (currentNode != start)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        if (path.Count > 0)
        {
            path.RemoveAt(0);
            path.Reverse();
        }

        return path;
    }

    public void Execute(Order order)
    {
        if (order.startNode == null || order.targetNode == null && order.prevNode == order.startNode)
            return;

        for (int i = 0; i < orderList.Count; ++i)
            if (orderList[i].chasing == order.chasing)
            {
                orderList.RemoveAt(i);
                return;
            }

        orderList.Add(order);
        UpdatePathFind();
    }

    private void PathFind()
    {
        if (_start != null && _target != null)
            PathFind(_start, _target);
    }

    private void PathFind(Node start, Node target)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        Node currentNode = start;

        while (currentNode != target)
        {
            Node[] neighbors = neighbor.NeighborNode(_roomNodes, currentNode);
            for (int i = 0; i < neighbors.Length; ++i)
            {
                if (closedList.Contains(neighbors[i]))
                    continue;

                //if (closedPath.Contains(neighbors[i]))
                    //continue;

                if (neighbors[i].NodeType == NodeType.Wall)
                    continue;

                int gCost = currentNode.GCost + GetDistance(currentNode, neighbors[i]);

                if (!openList.Contains(neighbors[i]) || gCost < neighbors[i].GCost)
                {
                    neighbors[i].SetGCost(gCost);
                    neighbors[i].SetHCost(GetDistance(neighbors[i], target));
                    neighbors[i].SetParent(currentNode);
                    openList.Add(neighbors[i]);
                }
            }

            if (!closedList.Contains(currentNode))
                closedList.Add(currentNode);

            //if (!closedPath.Contains(currentNode))
               //closedPath.Add(currentNode);

            if (openList.Contains(currentNode))
                openList.Remove(currentNode);

            if (openList.Count > 0)
            {
                openList.Sort(comparer);
                currentNode = openList[0];
            }
            else
                break;
        }

        if (currentNode == target)
        {
            List<Node> path = TracePath(start, target);
            // 6.11 추가. 
            //closedPath = path;
            //orderList[0].enemy._enemyFSM._chasing._path = path;
            orderList[0].chasing._order.prevNode = path[0];
            orderList[0].chasing._path = path;
            orderList.RemoveAt(0);
            state = false;
            // 2.13추가
            _start = null;
            _target = null;
            return;
        }
    }

    private void UpdatePathFind()
    {
        if ( !state && orderList.Count > 0 )
        {
            _start = orderList[0].startNode;
            _target = orderList[0].targetNode;
            _roomNodes = orderList[0].room;
            pathThread = new Thread(new ThreadStart(PathFind)) { IsBackground = true };
            pathThread.Start();
            state = true;
        }
    }

    private void Start()
    {
        neighbor = FindObjectOfType<NeighborLevel>();
    }
}
