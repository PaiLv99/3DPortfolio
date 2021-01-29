using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferMapMng : TSingleton<TransferMapMng>
{
    private List<GameObject> _startPoint;
    private List<GameObject> _maps;
    private List<GameObject> _endPoint;

    private int _mapsIndex;

    public override void Init()
    {
        _startPoint = new List<GameObject>();
        _endPoint = new List<GameObject>();
        _maps = new List<GameObject>();

    }

    public Vector3 GetStartPosition(int idx)
    {
        if (_startPoint.Count > 0)
            return _startPoint[idx].transform.position;
        return Vector3.zero;
    }

    public void StartPointSet(Collider other)
    {
        _startPoint[0].GetComponent<StartPoint>().PlayerIn(other);
    }

    public void RemoveStartPoint(int idx)
    {
        _startPoint.RemoveAt(idx);
    }

    public void SetPoint()
    {
        _startPoint.AddRange(GameObject.FindGameObjectsWithTag("StartPoint"));
        _endPoint.AddRange(GameObject.FindGameObjectsWithTag("TransferMap"));
    }

    public void SetMaps()
    {
        _maps.AddRange(GameObject.FindGameObjectsWithTag("Map"));
    }

    public void Open()
    {
        _endPoint[0].GetComponent<TransferMap>().Open();
        _endPoint.RemoveAt(0);
    }

    public void ActiveMap()
    {
        for (int i = 0; i < _maps.Count; i++)
        {
            _maps[i].gameObject.SetActive(false);
        }

        _maps[_mapsIndex].gameObject.SetActive(true);
        _mapsIndex++;
    }

    public bool CheckBossRoom()
    {
        if (_startPoint.Count == 0)
            return true;

        return false;
    }

    public void RemoveAll()
    {
        if (_maps.Count > 0)
            _maps.Clear();
        if (_startPoint.Count > 0)
            _startPoint.Clear();
        _mapsIndex = 0;
    }

    public void IsLastRoom()
    {
        if (_maps.Count > 0)
            return; 


    }
}
