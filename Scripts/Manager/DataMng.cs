using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType { CharTable, GunTable, ZombiesTable, ItemTable, BulletTable, MagazineTable }

public class DataMng
{
    private Dictionary<TableType, LowText> _tableDic = new Dictionary<TableType, LowText>();

    private static DataMng instance;
    public static DataMng Instance
    {
        get
        {
            if (instance == null)
                instance = new DataMng();

            return instance;
        }
    }

    public void AddTable<T>(TableType t) where T : LowText, new()
    {
        if (!_tableDic.ContainsKey(t))
        {
            T table = new T();
            table.Load("Table/" + t.ToString());
            _tableDic.Add(t, table);
        }
    }

    public object Table(TableType tType, int idx)
    {
        if (_tableDic.ContainsKey(tType))
        {
            return _tableDic[tType].GetInfo(idx);
        }
        return null;
    }

    public GameDate Table(TableType type, string name)
    {
        if (_tableDic.ContainsKey(type))
            return _tableDic[type].GetInfo(name);
        return null;
    }

    public GameDate GetInfo(string name)
    {
        return _tableDic[TableType.ZombiesTable].GetInfo(name);
    }

    public int TableLength(TableType type)
    {
        return _tableDic[type].DicCount();
    }
}
