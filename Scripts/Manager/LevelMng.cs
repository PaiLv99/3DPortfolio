using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMng : TSingleton<LevelMng>
{
    private readonly int _remainDungeonCount = 5;
    //private int _increseSize;
    private int _xPos = 0;
    private LevelGenerator _levelGenerator;
  
    public Dictionary<string, Node[,]> _stringRoomDic = new Dictionary<string, Node[,]>();

    public bool IsDone { get; private set; }
    public float Progress { get; private set; }

    public override void Init()
    {
        _levelGenerator = FindObjectOfType<LevelGenerator>();
        _levelGenerator.Init();
    }

    public void Generator()
    {
        StartCoroutine(IEGenerator(_remainDungeonCount));
        //for (int i = 0; i < _remainDungeonCount; i++)
        //{
        //    _levelGenerator.Generate(_xPos);
        //    _xPos += 100;
        //}
    }

    private IEnumerator IEGenerator(int count)
    {
        int index = 0;
        while (index < count)
        {
            _levelGenerator.Generate(_xPos);
            _xPos += 100;
            Progress = (float)index / (float)count;
            index++;
            yield return null;

        }
        IsDone = true;
    }

    public void PlayerChecker(string roomName)
    {
        if (_stringRoomDic.ContainsKey(roomName))
            SpawnMng.Instance.SpawnerStart(_stringRoomDic[roomName]);
    }

    public void ReInit()
    {
        if (_levelGenerator != null)
        {
            _levelGenerator = null;
            _levelGenerator = FindObjectOfType<LevelGenerator>();
            _levelGenerator.Init();
            _stringRoomDic.Clear();
            _xPos = 0;
        }
    }

    public void Clear()
    {

    }
}


