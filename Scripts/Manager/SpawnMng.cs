using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoomCoord
{
    public int _x, _y;

    public RoomCoord(int x, int y)
    {
        _x = x;
        _y = y;
    }
}

//  다음 집으로 넘어갈 때 정리해야함 
public class SpawnMng : TSingleton<SpawnMng>
{
    private Node[,] _currRoom;
    private Queue<RoomCoord> _currCoordQueue = new Queue<RoomCoord>();   // 좌표
    private List<RoomCoord> _currCoordList = new List<RoomCoord>();
    private Vector3 _offset = new Vector3(0.5f, 0, 0.5f);

    private readonly float _spawnTime = 1f;
    private readonly int _spawnMixCount = 3;
    private readonly int _spawnMaxCount = 5;

    private int _remainZombieCount;
    private int _waveCount;

    private GameObject[] _doors;

    private void SetMap(Node[,] room)
    {
        int x, y;

        for (int i = 0; i < room.GetLength(0); i++)
        {
            for (int j = 0; j < room.GetLength(1); j++)
            {
                if (room[i,j]._nodeType == NodeType.None)
                {
                    x = (int)room[i, j].transform.position.x;
                    y = (int)room[i, j].transform.position.z;
                    _currCoordList.Add(new RoomCoord(x, y));
                }
            }
        }

        SetRandomCoord(_currCoordList);
    }

    private void SetRandomCoord(List<RoomCoord> roomCoords)
    {
        int rand = Random.Range(0, 10);
        _currCoordQueue = new Queue<RoomCoord>(Helper.SuffleArray(roomCoords.ToArray(), rand));
    }

    private RoomCoord GetRandomCoord(Queue<RoomCoord> coords)
    {
        RoomCoord randomCoord = coords.Dequeue();
        coords.Enqueue(randomCoord);
        return randomCoord;
    }

    public void SetRemainEnemyCount(int value)
    {
        _remainZombieCount -= value;
    }

    public void CheckNewWave(int value)
    {
        _remainZombieCount -= value;

        if (_remainZombieCount == 0 && _waveCount > 0)
        {
            StartCoroutine(IESpawnerStart(_spawnTime));
        }
        else if (_remainZombieCount == 0 && _waveCount == 0)
        {
            GameObject obj = PoolMng.Instance.Pop(PoolType.ItemPool, ItemType.Itembox.ToString());
            obj.gameObject.SetActive(true);
            obj.transform.position = RandomPos() + _offset;
            SoundMng.Instance.BgmPlay("BGM");
            SoundMng.Instance.SetBgmMasterVolume(1.0f);
            // 문 열어야 한다.
            DoorOC(true);
        }
    }

    private Vector3 RandomPos()
    {
        RoomCoord coord = GetRandomCoord(_currCoordQueue);
        Vector3 pos = new Vector3(coord._x, 0, coord._y);

        return pos;
    }

    public void SpawnerStart(Node[,] room)
    {
        _waveCount = Random.Range(1, 2);
        _currRoom = room;
        SetMap(room);
        StartCoroutine(IESpawnerStart(_spawnTime));
        SoundMng.Instance.BgmPlay("Battle");
        SoundMng.Instance.SetBgmMasterVolume(.5f);
        DoorOC(false);
    }

    public void DoorInit()
    {
        _doors = GameObject.FindGameObjectsWithTag("Door");
    }

    private void DoorOC(bool state)
    {
        for (int i = 0; i < _doors.Length; i++)
            _doors[i].GetComponent<BoxCollider>().isTrigger = state;
    }

    private IEnumerator IESpawnerStart(float targetTime)
    {
        yield return new WaitForSeconds(targetTime);

        int rand = Random.Range(_spawnMixCount, _spawnMaxCount);

        _remainZombieCount = rand;

        for (int i = 0; i < rand; ++i)
        {
            GameObject zombie = PoolMng.Instance.Pop(PoolType.ZombiePool);
            zombie.SetActive(true);
            zombie.transform.position = RandomPos() + _offset;
            Enemy zombies = zombie.GetComponent<Enemy>();
            zombies.Init();
            zombies.GetComponent<Chasing>()._order.room = _currRoom;
        }
        _waveCount--;
    }
}
