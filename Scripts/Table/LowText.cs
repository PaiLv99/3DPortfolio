using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowText
{
    protected Dictionary<int, GameDate> _infoDic = new Dictionary<int, GameDate>();

    protected void AddInfo( int idx, GameDate val )
    {
        if (!_infoDic.ContainsKey(idx))
            _infoDic.Add(idx, val);
    }

    protected virtual void Parse( string text )
    {

    }

    public void Load(string path )
    {
        TextAsset t = Resources.Load<TextAsset>(path);
        if (t != null)
            Parse(t.text);

    }

    public GameDate GetInfo(int idx)
    {
        if(_infoDic.ContainsKey(idx))
            return _infoDic[idx];

        return null;
    }

    public GameDate GetInfo(string name)
    {
        for (int i = 0; i < _infoDic.Count; i++)
        {
            if (_infoDic[i].NAME == name)
                return _infoDic[i];
        }
        return null;
    }

    public int DicCount()
    {
        return _infoDic.Count;
    }
}
