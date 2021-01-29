using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    private Player _player;
    private BossTest _boss;
    private GameObject _map;
    private CameraController _cameraController;
    private Animator _animator;

    private Vector3 _playerStartPos = new Vector3(10,0,10);
    private Transform _nodeParent;
    private Node _nodePrefab;

    private Node[,] _nodes = new Node[20,20];

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _map = Resources.Load<GameObject>("Model/Envioronment/BossMap");
        _nodePrefab = Resources.Load<Node>("Model/Envioronment/TileNode");
        //_player = FindObjectOfType<Player>();
        //_player.Init();
        _boss = FindObjectOfType<BossTest>();
        _cameraController = FindObjectOfType<CameraController>();
        _cameraController.Init();

        _nodeParent = Helper.Find<Transform>(transform, "NodeParent");
        _animator = GetComponent<Animator>();

        CreateNode();
    }

    private void CreateNode()
    {
        for (int x = 0; x <_nodes.GetLength(0); x++)
        {
            for (int y = 0; y < _nodes.GetLength(1); y++)
            {
                Node node = Instantiate(_nodePrefab, new Vector3(x, 0, y), Quaternion.identity, _nodeParent);
                SetNodeType(node);
                node.SetNode(x, y);
                _nodes[x, y] = node;
            }
        }
        //_boss._order.room = _nodes;     // 보스한테 방 좌표를 대입시킨다.
    }

    private void SetNodeType(Node node)
    {
        Vector3 pos = node.transform.position;
        pos.y = -1;

        Ray ray = new Ray(pos, Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, 1 << 11))
            node.SetNodeType(NodeType.Wall);
        else
            node.SetNodeType(NodeType.None);
    }

    public void SetAnimation(string path)
    {
        _boss._animator.SetTrigger(path);
    }

    public void IsFade()
    {
        UIMng.Instance.FadeOut(true, .5f, null);
    }

    public void CutScene(int num)
    {
        if (num == 1)
            _animator.SetBool("START", true);
        if (num == 2)
            _animator.SetBool("START", false);
    }

    public void CameraClear()
    {
        Debug.Log("In");
        _cameraController.ClearCameraTransform();
    }
}
