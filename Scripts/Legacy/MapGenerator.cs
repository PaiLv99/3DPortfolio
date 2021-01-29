using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Transform _tilePrefab;
    public Transform _obstaclePrefab;
    public Vector2 _mapSize;
    public int seed = 10;

    private List<Coord> allTileCoord;
    private Queue<Coord> shuffledTileCoords;

    [Range(0,1)]
    public float _outLinePercent;

    public void Generator()
    {
        allTileCoord = new List<Coord>();

        for (int x = 0; x < _mapSize.x; ++x)
        {
            for (int y = 0; y < _mapSize.y; ++y)
            {
                allTileCoord.Add(new Coord(x,y));
            }
        }

        shuffledTileCoords = new Queue<Coord>(Helper.SuffleArray(allTileCoord.ToArray(), seed));

        string holdName = "Generated Map";

        if (transform.Find(holdName))
            DestroyImmediate(transform.Find(holdName).gameObject);

        Transform mapHolder = new GameObject(holdName).transform;
        mapHolder.parent = transform;
        for (int x = 0; x < _mapSize.x; ++x)
        {
            for (int y = 0; y < _mapSize.y; ++y)
            {
                Vector3 position = new Vector3(-_mapSize.x / 2 + x + .5f, 0, -_mapSize.y / 2 + y + .5f);
                Transform instance = Instantiate(_tilePrefab, position, Quaternion.Euler(90, 0, 0)) as Transform;
                instance.localScale = Vector3.one * (1 - _outLinePercent);
                instance.parent = mapHolder;
            }
        }

        int obstacleCount = 10;

        for (int i = 0; i < obstacleCount; ++i)
        {
            Coord randomCoord = GetRandomCoord();
            Vector3 position = Position(randomCoord._x, randomCoord._y);
            Transform obstacle = Instantiate(_obstaclePrefab, position, Quaternion.identity) as Transform;
            obstacle.parent = mapHolder;
             
        }
    }

    private Vector3 Position(int x, int y)
    {
        return new Vector3(-_mapSize.x / 2 + x + .5f, 0, -_mapSize.y / 2 + y + .5f);
    }

    public Coord GetRandomCoord() 
    {
        Coord randomCoord = shuffledTileCoords.Dequeue();
        shuffledTileCoords.Enqueue(randomCoord);
        return randomCoord;
    }

    public struct Coord
    {
        public int _x, _y;

        public Coord(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }

    private void Awake()
    {
        Generator();
    }
}
