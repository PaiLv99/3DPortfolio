using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ThreadPath : 추적할 때 사용되는 구조체 
[System.Serializable]
public struct Order
{
    public Node startNode;
    public Node targetNode;
    public Node[,] room;
    public Node prevNode;
    public Chasing chasing;

    public void Clear()
    {
        room = null;
        startNode = null;
        targetNode = null;
    }
}

public class Chasing : MonoBehaviour, IState
{
    private ThreadPath _thread;
    private float _threadElapsedTime;
    private const float _threadTime = 0.25f;
    private float _speed = 4.0f;

    public Order _order = new Order();
    public List<Node> _path = new List<Node>();

    private Player _player;
    private Animator _animator;

    private EnemyFSM _fsm;
    private EnemyState _prevState;

    private Node _prevNode;

    public void Init()
    {
        _fsm = GetComponent<EnemyFSM>();

        _thread = FindObjectOfType<ThreadPath>();
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        //_enemy = GetComponent<Enemy>();
        //_order.enemy = _enemy;
        _order.chasing = this;
    }

    private Node FindNode()
    {
        Vector3 pos = transform.position;
        pos.y += 1;

        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10, 1 << 10))
        {
            if (hit.transform.GetComponent<Node>().NodeType == NodeType.None)
                return hit.transform.GetComponent<Node>();
        }
        return null;
    }

    private void ExecuteThread()
    {
        _threadElapsedTime = 0;
        _order.targetNode = _player.FindNode();
        _thread.Execute(_order);
    }

    private void ChasingAction()
    {
        if (_path.Count > 0 && _path[0] != null)
        {
            Vector3 currPos = transform.position;
            currPos.y = 0f;
            _animator.SetBool("SHOOTIDLE", false);
            _animator.SetBool("IDLE", false);
            Vector3 pos = _path[0].transform.position;
            Vector3 dir = pos - currPos;
            dir = dir.normalized;

            transform.position += dir * Time.deltaTime * _speed;
            transform.LookAt(Vector3.Lerp(currPos, pos, Time.time * _speed));

            float distance = (pos - currPos).sqrMagnitude;
            if (distance < .1f)
                _path.RemoveAt(0);
        }
    }

    public void Enter()
    {
        _prevState = _fsm._prevState;

        _threadElapsedTime += Time.deltaTime;
        _order.startNode = FindNode();


        if (_threadElapsedTime > _threadTime)
            ExecuteThread();

        Execute();
    }

    public void Execute()
    {
        ChasingAction();
    }

    public void Exit()
    {
        _order.Clear();
        _path.Clear();

        _fsm._state = _prevState;
    }
}
