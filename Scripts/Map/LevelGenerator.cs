using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum RoomType { Square10, Square15, Square20, Row15, Row20, Row24, Col15, Col20, Col24, Start, End }

public class LevelGenerator : MonoBehaviour
{
    public class SubDungeon
    {
        public LevelGenerator _levelGenerator = FindObjectOfType<LevelGenerator>();
        // 다리에 사용될 Rect 리스트
        public List<Rect> corridors = new List<Rect>();
        // 재귀를 통해 분할할 던젼
        public SubDungeon _left, _right;
        // 분할된 공간으로 사용할 변수 
        public Rect _rect;
        // _room은 방으로 사용되는 공간이다.
        public Rect _room = new Rect(-1, -1, 0, 0);
        // 생성자 
        public SubDungeon(Rect rect)
        {
            _rect = rect;
        }

        // 분할된 공간을 상수와 비교하여 해당하는 룸타입 결정한다 그 때를 위한 변수
        public RoomType _roomType;

        private Rect GetRoomLeft()
        {
            if (IAmLeaf())
                return _room;
            
            Rect lRoom = _left.GetRoomLeft();
            return lRoom;
        }

        private Rect GetRoomRight()
        {
            if (IAmLeaf())
                return _room;

            Rect rRoom = _right.GetRoomRight();
            return rRoom;
        }

        // 복도 공간을 만드는 함수 
        private void CreateCorrider(SubDungeon left, SubDungeon right)
        {
            Rect lRoom = left.GetRoomRight();
            Rect rRoom = right.GetRoomLeft();

            Vector2 lPoint = new Vector2((int)lRoom.x + ((int)lRoom.width / 2), (int)lRoom.y + ((int)lRoom.height / 2));
            Vector2 rPoint = new Vector2((int)rRoom.x + ((int)rRoom.width / 2), (int)rRoom.y + ((int)rRoom.height / 2));

            int w = (int)lPoint.x - (int)rPoint.x;
            int h = (int)lPoint.y - (int)rPoint.y;
            int centerPosH = (int)((lPoint.y + rPoint.y) / 2 - 1);
            int centerPosW = (int)((lPoint.x + rPoint.x) / 2 - 1);

            bool state = false;

            if ((rPoint.x - lPoint.x) < 0 )
                state = true;

            if (state)
                centerPosH = (int)(right._rect.height + right._rect.y - 1);
            else
                centerPosW = (int)(left._rect.width + left._rect.x - 1);

            if (Mathf.Abs(h) <= rRoom.height / 2 - 1)
                corridors.Add(new Rect(lPoint.x, lPoint.y, Mathf.Abs(w), 1));
            else if (Mathf.Abs(w) <= lRoom.width/ 2 - 1)
                corridors.Add(new Rect(rPoint.x, rPoint.y, 1, Mathf.Abs(h)));
            else if (rPoint.x - lPoint.x > 0 && rPoint.y - lPoint.y < 0)
            {
                if (lRoom.xMax  - 1 > rRoom.x)
                    corridors.Add(new Rect(rRoom.x + 1, rRoom.y, 1, Mathf.Abs(h)));
                else
                {
                    Vector2 hPos;
                    hPos = new Vector2((int)(rPoint.x + lPoint.x) / 2, rPoint.y);

                    int rW = (int)(hPos.x - rPoint.x);
                    int lW = (int)(lRoom.xMax - hPos.x);

                    corridors.Add(new Rect(hPos.x, hPos.y, 1, Mathf.Abs(h) + 1));
                    corridors.Add(new Rect(hPos.x, rPoint.y, Mathf.Abs(rW), 1));
                    corridors.Add(new Rect(lRoom.xMax , lPoint.y, Mathf.Abs(lW), 1));
                }
            }
            else
            {
                Vector2 corYPos;
                Vector2 corXPos;

                // startPoint = rPoint;
                corYPos = state ? rPoint : new Vector2(centerPosW, centerPosH);
                corXPos = state ? new Vector2(rPoint.x, centerPosH) : new Vector2(centerPosW, rPoint.y);
                AddCorridors(corXPos, corYPos, rPoint, centerPosW, centerPosH);
                // startPoint = lPoint;
                corYPos = state ? new Vector2(lPoint.x, centerPosH) : new Vector2(centerPosW, lPoint.y);
                corXPos = state ? new Vector2(centerPosW, centerPosH) : lPoint;
                AddCorridors(corXPos, corYPos, lPoint, centerPosW, centerPosH);
            }
        }

        private void AddCorridors(Vector2 xPos, Vector2 yPos, Vector2 startPos, int centerPosW, int centerPosH)
        {
            int w = (int)(startPos.x - centerPosW);
            int h = (int)(startPos.y - centerPosH);

            corridors.Add(new Rect(yPos.x, yPos.y, 1, Mathf.Abs(h)));
            corridors.Add(new Rect(xPos.x, xPos.y, Mathf.Abs(w), 1));
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

            if (Mathf.Min(_rect.height, _rect.width) / 2 < minRoomSize)
                return false;

            // 수평으로 나눌 때 
            if (SplitH)
            {
                int split = Random.Range(minRoomSize, (int)(_rect.height - minRoomSize));
                _right = new SubDungeon(new Rect(_rect.x, _rect.y, _rect.width, split));
                _left = new SubDungeon(new Rect(_rect.x, _rect.y + split, _rect.width, _rect.height - split));
            }
            // 수직으로 나눌 때
            else
            {
                int split = Random.Range(minRoomSize, (int)(_rect.width - minRoomSize));
                _left = new SubDungeon(new Rect(_rect.x, _rect.y, split, _rect.height));
                _right = new SubDungeon(new Rect(_rect.x + split, _rect.y, _rect.width - split, _rect.height));
            }
            return true;
        }

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
                _levelGenerator._rectList.Add(_rect);
                // 가로형 던젼
                if (_rect.width / _rect.height >= 1.33)
                {
                    int rand = Random.Range(0, 3);
                    switch (rand)
                    {
                        case 0: MakeRoom(14, 10, RoomType.Col15); break;
                        case 1: MakeRoom(18, 10, RoomType.Col20); break;
                        case 2: MakeRoom(20, 10, RoomType.Col24); break;
                    }
                }
                // 세로형 던젼 
                else if (_rect.height / _rect.width >= 1.33)
                {
                    int rand = Random.Range(0, 3);
                    switch (rand)
                    {
                        case 0: MakeRoom(10, 14, RoomType.Row15); break;
                        case 1: MakeRoom(10, 18, RoomType.Row20); break;
                        case 2: MakeRoom(10, 20, RoomType.Row24); break;
                    }
                }
                // 정사각형 
                else
                {
                    int rand = Random.Range(0, 2);
                    switch (rand)
                    {
                        case 0: MakeRoom(10, 10, RoomType.Square10); break;
                        case 1: MakeRoom(12, 12, RoomType.Square15); break;
                    }
                }
            }
        }

        public void MakeRoom(int roomWidth, int roomHeight, RoomType roomType)
        {
            int roomX = (int)Random.Range(1, _rect.width - roomWidth - 1);
            int roomY = (int)Random.Range(1, _rect.height - roomHeight - 1);
            _room = new Rect(_rect.x + roomX, _rect.y + roomY, roomWidth, roomHeight);
            _roomType = roomType;
        }
    }

    public int _mapRow, _mapCol;
    public int _minRoomSize, _maxRoomSize;
    public Node[,] nodes;
    private Node _nodePrefab;
    private GameObject _wallPrefab;
    private GameObject _doorPrefab;
    private Dictionary<RoomType, List<GameObject>> _roomDic = new Dictionary<RoomType, List<GameObject>>();
    private List<int>[] deck;
    private GameObject _startRoom;
    private GameObject _endRoom;
    private CompareCoord _compare = new CompareCoord();

    private int _mapNum;
    private int _roomNumber;
    public List<Rect> _rectList = new List<Rect>();

    private readonly string Path = "Prefab/Envioronment/Room/";

    private void CreateBSP(SubDungeon subDungeon)
    {
        if (subDungeon.IAmLeaf())
        {
            if (subDungeon._rect.width > _maxRoomSize || subDungeon._rect.height > _maxRoomSize) // || Random.Range(0.0f, 1.0f) > 0.25)
            {
                if (subDungeon.Split(_minRoomSize, _maxRoomSize))
                {
                    CreateBSP(subDungeon._right);
                    CreateBSP(subDungeon._left);
                }
            }
        }
    }

    private void SetNodeType(Node node)
    {
        Vector3 pos = node.transform.position + new Vector3(.5f,0,.5f);
        pos.y = -1;

        Ray ray = new Ray(pos, Vector3.up);

        if (Physics.Raycast(ray, out RaycastHit hit, 10, 1 << 11))
            node.SetNodeType(NodeType.Wall);
        else
            node.SetNodeType(NodeType.None);
    }

    private void DrawRoom(SubDungeon subDungeon)
    {
        if (subDungeon == null)
            return;

        if (subDungeon.IAmLeaf())
        {
            if (subDungeon._rect == _rectList[0])
                subDungeon._roomType = RoomType.Start;

            if (subDungeon._rect == _rectList[_rectList.Count - 1])
                subDungeon._roomType = RoomType.End;

            // 타입에 맞는 방을 생산한다
            switch (subDungeon._roomType)
            {
                case RoomType.Square10: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Square15: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Square20: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Row15: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Row20: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Row24: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Col15: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Col20: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Col24: SelectRoom(subDungeon._roomType, subDungeon); break;
                case RoomType.Start: StartOrEnd(subDungeon._roomType, subDungeon); break;
                case RoomType.End: StartOrEnd(subDungeon._roomType, subDungeon); break;
            }
        }
        else
        {
            DrawRoom(subDungeon._left);
            DrawRoom(subDungeon._right);
        }
    }

    private void CreateNode(SubDungeon subDungeon, Transform parent)
    {
        Node[,] roomNodes = new Node[(int)subDungeon._room.width, (int)subDungeon._room.height];
        int nodeX = 0, nodeY = 0;

        GameObject RoomNode = new GameObject("RoomNode");
        RoomNode.transform.parent = parent;

        for (int x = (int)subDungeon._room.x; x < (int)subDungeon._room.xMax; x++)
        {
            for (int y = (int)subDungeon._room.y; y < (int)subDungeon._room.yMax; y++)
            {
                Node node = Instantiate(_nodePrefab, new Vector3(x, 0, y), Quaternion.identity, RoomNode.transform);
                node.SetNode(nodeX, nodeY);
                SetNodeType(node);
                nodes[x, y] = node;
                roomNodes[nodeX, nodeY] = node;
                nodeY++;
            }
            nodeY = 0;
            nodeX++;
        }
        LevelMng.Instance._stringRoomDic.Add(parent.name, roomNodes);
    }

    private void DrawCorridor(SubDungeon subDungeon)
    {
        if (subDungeon == null || subDungeon.corridors.Count == 0)
            return;

        DrawCorridor(subDungeon._left);
        DrawCorridor(subDungeon._right);

        GameObject corridorsParent = new GameObject("CorridorsParent");
        corridorsParent.transform.parent = transform.Find("Map" + _mapNum.ToString());

        List<Node> corridorList = new List<Node>();

        foreach (Rect corridors in subDungeon.corridors)
        {
            for (int x = (int)corridors.x; x < (int)corridors.xMax; ++x)
            {
                for (int y = (int)corridors.y; y < (int)corridors.yMax; ++y) 
                {
                    if (nodes[x, y] == null)
                    {
                        Node corridor = Instantiate(_nodePrefab, new Vector3(x, 0, y), Quaternion.identity, corridorsParent.transform);
                        corridor.SetNode(x, y);
                        nodes[x, y] = corridor;
                        corridorList.Add(corridor);
                    }
                }
            }
        }
        corridorList.Sort(_compare);
        DrawDoor(corridorList, corridorsParent);
    }

    private void DrawDoor(List<Node> corridor, GameObject parent)
    {
        if (corridor.Count <= 0)
            return;

        GameObject startDoor = Instantiate(_doorPrefab, corridor[0].transform.position + new Vector3(.5f, .5f, .5f), Quaternion.identity, parent.transform);
        GameObject endDoor = Instantiate(_doorPrefab, corridor[corridor.Count - 1].transform.position + new Vector3(.5f, .5f, .5f), Quaternion.identity, parent.transform);
    }

    private void DrawWall()
    {
        GameObject Wall = new GameObject("Wall");
        Wall.transform.parent = transform.Find("Map" + _mapNum.ToString());

        for (int x = 0; x < _mapRow; ++x)
            for (int y = 0; y < _mapCol; ++y)
            {
                if (nodes[x, y] == null)
                    continue;

                if (nodes[x + 1, y] == null)
                {
                    Vector3 pos = new Vector3(x + 1, 0, y + 1);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 270, 0), Wall.transform);
                }
                if (nodes[x - 1, y] == null)
                {
                    Vector3 pos = new Vector3(x, 0, y);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 90, 0), Wall.transform);
                }
                if (nodes[x, y + 1] == null)
                {
                    Vector3 pos = new Vector3(x, 0, y + 1);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 180, 0), Wall.transform);
                }
                if (nodes[x, y - 1] == null)
                {
                    Vector3 pos = new Vector3(x, 0, y);
                    GameObject wall = Instantiate(_wallPrefab, pos, Quaternion.Euler(0, 180, 0), Wall.transform);
                }
            }
    }

    private void StartOrEnd(RoomType roomType, SubDungeon subDungeon)
    {
        int xPos = (int)subDungeon._room.x + (int)subDungeon._room.width / 2;
        int yPos = (int)subDungeon._room.y + (int)subDungeon._room.height / 2;

        if (roomType == RoomType.Start)
        {
            GameObject startRoom = new GameObject("Map" + _mapNum.ToString() + "StartRoom");
            startRoom.transform.parent = transform.Find("Map" + _mapNum.ToString());
            Instantiate(_startRoom, new Vector3(xPos, 0, yPos), Quaternion.identity, startRoom.transform);
            CreateNode(subDungeon, startRoom.transform);
        }
        else
        {
            GameObject endRoom = new GameObject("Map" + _mapNum.ToString() + "EndRoom");
            endRoom.transform.parent = transform.Find("Map" + _mapNum.ToString());
            Instantiate(_endRoom, new Vector3(xPos, 0, yPos), Quaternion.identity, endRoom.transform);
            CreateNode(subDungeon, endRoom.transform);
        }
    }

    private void DeckInit(RoomType roomType)
    {
        deck[(int)roomType] = new List<int>();
        for (int i = 0; i < _roomDic[roomType].Count; i++)
            deck[(int)roomType].Add(i);
    }

    private GameObject SelectRoomList(RoomType roomType)
    {
        _roomNumber++;
        int rand = Random.Range(0, deck[(int)roomType].Count);
        int roomNumber = deck[(int)roomType][rand];
        deck[(int)roomType].RemoveAt(rand);

        return _roomDic[roomType][roomNumber];
    }

    private void SelectRoom(RoomType roomType, SubDungeon subDungeon)
    {
        if(deck[(int)roomType] == null || deck[(int)roomType].Count == 0)
            DeckInit(roomType);

        int xPos = (int)subDungeon._room.x;
        int yPos = (int)subDungeon._room.y;

        GameObject room = new GameObject("Map" + _mapNum.ToString() + "Room" + _roomNumber.ToString());
        room.transform.position = new Vector3(xPos, 0, yPos);
        room.AddComponent<BoxCollider>().size = new Vector3(subDungeon._room.width, 2, subDungeon._room.height);
        room.GetComponent<BoxCollider>().center = new Vector3(subDungeon._room.width / 2, 1, subDungeon._room.height / 2);
        room.GetComponent<BoxCollider>().isTrigger = true;

        room.AddComponent<Room>();
        room.transform.parent = transform.Find("Map" + _mapNum.ToString());

        Instantiate(SelectRoomList(roomType), new Vector3(xPos, 0, yPos), Quaternion.identity, room.transform);
        CreateNode(subDungeon, room.transform);
    }

    public void Init()
    {
        List<GameObject>[] rooms = new List<GameObject>[9];
        deck = new List<int>[9];

        for (int i = 0; i < 9; i++)
        {
            rooms[i] = new List<GameObject>();
            rooms[i].AddRange(Resources.LoadAll<GameObject>(Path + i.ToString()));

            if (!_roomDic.ContainsKey((RoomType)i))
                _roomDic.Add((RoomType)i, rooms[i]);
        }

        _startRoom = Resources.Load<GameObject>(Path + "StartRoom");
        _endRoom = Resources.Load<GameObject>(Path + "EndRoom");
        _doorPrefab = Resources.Load<GameObject>(Path + "Door");
        _nodePrefab = Resources.Load<Node>(Path + "TileNode");
        _wallPrefab = Resources.Load<GameObject>(Path + "WallPrefab");
    }

    public void Generate(int x)
    {
        GameObject map = new GameObject("Map" + _mapNum.ToString());
        map.transform.parent = transform;
        map.tag = "Map";

        SubDungeon rootSubDungeon = new SubDungeon(new Rect(0, 0, _mapRow, _mapCol));
        nodes = new Node[_mapRow, _mapCol];

        CreateBSP(rootSubDungeon);
        rootSubDungeon.CreateRoom();

        DrawRoom(rootSubDungeon);
        DrawCorridor(rootSubDungeon);
        DrawWall();

        _rectList.Clear();

        _mapNum++;
        map.transform.position = new Vector3(x, 0, 0);
    }
}
