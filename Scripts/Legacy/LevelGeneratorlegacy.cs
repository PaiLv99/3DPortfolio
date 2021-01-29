using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorlegacy : MonoBehaviour
{
    public struct Coord
    {
        public int _x, _y;
        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }

    int totalCount = 0;

    public int _mapRow, _mapCol;
    public int _minRoomSize, _maxRoomSize;

    public Node tile;
    public Node corridorTile;
    public static int nodeCount;
    public static Node[,] nodes;

    public Enemy _enemyPrefab;
    public Player _playerPrefab;
    public GameObject _obstaclePrefab;
    public GameObject _wallPrefab;
    public GameObject _doorPrefab;
    public GameObject _emptyPrefab;
    public GameObject _endPrefab;

    private bool _hereStart = false;
    private bool _hereEnd = false;

    public List<Coord> obstacleList;
    public Queue<Coord> shuffledArray;

    public List<GameObject> _wallList = new List<GameObject>();

    public int seed = 10;
    public int enemyCount = 5;

    public GameObject[] _desk;

    private void CreateBSP(SubDungeon subDungeon)
    {
        if (subDungeon.IAmLeaf())
        {
            if (subDungeon._rect.width > _maxRoomSize || subDungeon._rect.height > _maxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {
                if (subDungeon.Split(_minRoomSize, _maxRoomSize))
                {
                    CreateBSP(subDungeon._right);
                    CreateBSP(subDungeon._left);
                }
            }
        }
    }

    private void DrawRoom(SubDungeon subDungeon)
    {
        if (subDungeon == null)
            return;

        if (subDungeon.IAmLeaf())
        {
            obstacleList = new List<Coord>();
            // _room 좌표를 얻어와 장애물 생성에 필요한 좌표를 만들어 준다.
            for (int x = (int)subDungeon._room.x + 1; x < subDungeon._room.xMax -1; ++x)
                for (int y = (int)subDungeon._room.y +1; y < subDungeon._room.yMax-1; ++y)
                    obstacleList.Add(new Coord(x, y));

            shuffledArray = new Queue<Coord>(Helper.SuffleArray(obstacleList.ToArray(), seed));
           
            // 장애물 생성
            int obstacleCount = 0;
            for (int i = 0; i < obstacleCount; ++i)
            {
                Coord randomCoord = GetRandomCoord(shuffledArray);
                Vector3 position = new Vector3(randomCoord._x, .5f, randomCoord._y);
                GameObject obstacle = Instantiate(_obstaclePrefab, position, Quaternion.identity);
            }


            // 노드 생성
            for (int x = (int)subDungeon._room.x; x < subDungeon._room.xMax; ++x)
            {
                for (int y = (int)subDungeon._room.y; y <subDungeon._room.yMax; ++y)
                {
                    Node room = Instantiate(tile, new Vector3(x,0,y), Quaternion.Euler(0, 0, 0), transform.Find("Node"));
                    //room.transform.SetParent(transform);
                    room.name = room.name + "||" + x + "::" + y;
                    room.SetNodeType(NodeType.None);
                    SetNodeType(room);
                    room.SetNode(x, y);
                    nodes[x, y] = room;
                }
            }

            // 적 생성 
            if (totalCount < 15)
            {
                for (int i = 0; i < 1; ++i)
                {
                    Coord randomCoord = GetRandomCoord(shuffledArray);
                    Vector3 position = new Vector3(randomCoord._x, 0, randomCoord._y);
                    Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity, transform.Find("Enemy"));
                }
                ++totalCount;
            }
          
            if (_hereStart == false)
            {
                _hereStart = true;

                Coord pos = GetRandomCoord(shuffledArray);
                Vector3 position = new Vector3(pos._x, 0, pos._y);
                Player player = Instantiate(_playerPrefab, position, Quaternion.identity);
            }
            else if (_hereEnd == false)
            {
                _hereEnd = true;

                Coord pos = GetRandomCoord(shuffledArray);
                Vector3 position = new Vector3(pos._x, 1, pos._y);
                GameObject end = Instantiate(_endPrefab, position, Quaternion.identity);
            }
        }
        else
        {
            DrawRoom(subDungeon._left);
            DrawRoom(subDungeon._right);
        }
    }

    private void DrawCorridor(SubDungeon subDungeon)
    {
        if (subDungeon == null)
            return;

        DrawCorridor(subDungeon._left);
        DrawCorridor(subDungeon._right);

        foreach(Rect corridors in subDungeon.corridors)
            for (int x = (int)corridors.x; x < corridors.xMax; ++x)
                for (int y = (int)corridors.y; y < corridors.yMax; ++y)
                {
                    if (nodes[x, y] == null)
                    {
                        Node corridor = Instantiate(tile, new Vector3(x, 0, y), Quaternion.Euler(0, 0, 0), transform.Find("Node"));
                        corridor.SetNodeType(NodeType.None);
                        SetNodeType(corridor);
                        corridor.SetNode(x, y);
                        nodes[x, y] = corridor;
                    }
                }
    }

    private void DrawWall()
    {
        for (int x = 0; x < _mapRow; ++x )
            for (int y = 0; y < _mapCol; ++y)
            {
                if (nodes[x, y] == null)
                {
                    //GameObject wall = Instantiate(_emptyPrefab, new Vector3(x - .5f, 1, y - .5f), Quaternion.Euler(0, 90, 0), transform.Find("Empty"));
                    continue;
                }
                if (nodes[x + 1, y] == null)
                {
                    Vector3 pos = new Vector3(x, 0, y);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 270, 0), transform.Find("Wall"));

                    _wallList.Add(wall);
                }
                if (nodes[x - 1, y] == null)
                {
                    Vector3 pos = new Vector3(x - 1, 0, y - 1);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0,90, 0), transform.Find("Wall"));
                    _wallList.Add(wall);
                }
                if (nodes[x, y + 1] == null)
                {
                    Vector3 pos = new Vector3(x - 1, 0, y);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 180, 0), transform.Find("Wall"));
                    _wallList.Add(wall);
                }
                if (nodes[x, y - 1] == null)
                {
                    Vector3 pos = new Vector3(x - 1, 0, y - 1);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 180, 0), transform.Find("Wall"));
                    //GameObject desk = Instantiate(_desk[1], pos, Quaternion.identity);
                    _wallList.Add(wall);
                }
            }
    }

    private void CheckWall(Vector3 pos , int dir)
    {   
        Ray rRay = new Ray(_wallPrefab.transform.position, transform.right);

        if (!Physics.Raycast(rRay, out RaycastHit rHit, 1f, 1 << 11))
        {
            //GameObject door = Instantiate(_doorPrefab, pos, Quaternion.Euler(0, dir, 0)) as GameObject;
        }

        Ray lRay = new Ray(_wallPrefab.transform.position, -transform.right);

        if (!Physics.Raycast(lRay, out RaycastHit lHit, 1f, 1 << 11))
        {
            //GameObject door = Instantiate(_doorPrefab, pos, Quaternion.Euler(0, dir, 0)) as GameObject;
        }
    }
  
    private Coord GetRandomCoord(Queue<Coord> coords)
    {
        Coord randomCoord = coords.Dequeue();
        coords.Enqueue(randomCoord);
        return randomCoord;
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

    public class SubDungeon
    {
        // 다리에 사용될 Rect 리스트
        public List<Rect> corridors = new List<Rect>();
        // 재귀를 통해 분할할 던젼
        public SubDungeon _left, _right;
        // 공간으로 사용할 변수 
        public Rect _rect;
        // null 초기화
        public Rect _room = new Rect(-1, -1, 0, 0);
        // 생성자 
        public SubDungeon(Rect rect)
        {
            _rect = rect;
        }

        public RoomType _roomType;

        public Rect GetRoom()
        {
            if (IAmLeaf())
                return _room;
            if (_left != null)
            {
                Rect lRoom = _left.GetRoom();
                return lRoom;
            }
            if (_right != null)
            {
                Rect rRoom = _right.GetRoom();
                return rRoom;
            }
            // null 반환 
            return new Rect(-1,-1,0,0);
        }

        // 복도 공간을 만드는 함수 
        public void CreateCorrider(SubDungeon left, SubDungeon right)
        {
            Rect lRoom = left.GetRoom();
            Rect rRoom = right.GetRoom();
            // 다리를 만들기위해 연결될 방의 임의의 지점을 얻는다. 각 방에서 점을 하나씩 얻는다.
            Vector2 lPoint = new Vector2((int)Random.Range(lRoom.x + 1, lRoom.xMax - 1), (int)Random.Range(lRoom.y + 1, lRoom.yMax - 1));
            Vector2 rPoint = new Vector2((int)Random.Range(rRoom.x + 1, rRoom.xMax - 1), (int)Random.Range(rRoom.y + 1, rRoom.yMax - 1));

            if (lPoint.x > rPoint.x)
            {
                Vector2 temp = lPoint;
                lPoint = rPoint;
                rPoint = temp;
            }

            int w = (int)(lPoint.x - rPoint.x);
            int h = (int)(lPoint.y - rPoint.y);

            // 수평이 아닐 때
            if (w != 0)
            {
                if (h < 0)
                    corridors.Add(new Rect(lPoint.x, lPoint.y, 2, Mathf.Abs(h)));
                else
                    corridors.Add(new Rect(lPoint.x, rPoint.y, 2, Mathf.Abs(h)));
                corridors.Add(new Rect(lPoint.x, rPoint.y, Mathf.Abs(w) + 1, 2));
            }
            // 수평일 때 
            else
            {
                if (h < 0)
                    corridors.Add(new Rect(lPoint.x, lPoint.y, 2, Mathf.Abs(h)));
                else
                    corridors.Add(new Rect(rPoint.x, rPoint.y, 2, Mathf.Abs(h)));
            }
        }

        // 방 공간을 만드는 함수 
        public void CreateRoom()
        {
            if (_left != null)
                _left.CreateRoom();
            if (_right != null)
                _right.CreateRoom();

            if (_left != null && _right != null)
                CreateCorrider(_left, _right);

            if (IAmLeaf())
            {
                int roomWidth = (int)Random.Range(_rect.width / 2, _rect.width - 2);
                int roomHeight = (int)Random.Range(_rect.height / 2, _rect.height - 2);
                int roomX = (int)Random.Range(1, _rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, _rect.height - roomHeight - 1);
                _room = new Rect(_rect.x + roomX, _rect.y + roomY, roomWidth, roomHeight);

                // 룸의 크기를 비교하여 roomType을 결정한다.

            }
        }

        // 더 이상 나눌 수 없는 공간을 확인하는 함수 
        public bool IAmLeaf()
        {
            return _left == null && _right == null;
        }
        // 공간을 나누는 함수 
        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IAmLeaf())
                return false;

            bool SplitH;

            if (_rect.width / _rect.height >= 1.25)
                SplitH = false;
            else if (_rect.height / _rect.width >= 1.25)
                SplitH = true;
            else
                SplitH = Random.Range(0.0f, 1.0f) > 0.5;
   
            if (Mathf.Min(_rect.width, _rect.height) / 2 < minRoomSize)
                return false;

            // 수평으로 나눌 때 
            if (SplitH)
            {
                int split = Random.Range(minRoomSize, (int)(_rect.width - minRoomSize));
                _left = new SubDungeon(new Rect(_rect.x, _rect.y, _rect.width, split));
                _right = new SubDungeon(new Rect(_rect.x, _rect.y + split, _rect.width, _rect.height - split));
            }
            // 수직으로 나눌 때
            else
            {
                int split = Random.Range(minRoomSize, (int)(_rect.height - minRoomSize));
                _left = new SubDungeon(new Rect(_rect.x, _rect.y, split, _rect.height));
                _right = new SubDungeon(new Rect(_rect.x + split, _rect.y, _rect.width - split, _rect.height));
            }
            return true;
        }
    }

    public void Awake()
    {
        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, _mapRow, _mapCol));
        nodes = new Node[_mapRow, _mapCol];

        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();
        rootSubDungeon.CreateCorrider(rootSubDungeon._left, rootSubDungeon._right);

        DrawRoom(rootSubDungeon);
        DrawCorridor(rootSubDungeon);
        DrawWall();

        nodeCount = _mapCol;
    }
}
