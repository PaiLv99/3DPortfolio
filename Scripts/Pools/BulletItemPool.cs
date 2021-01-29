using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType { AutoAmmo, HandAmmo, RevolverAmmo, ShotgunAmmo, }
public enum MagazineType { Auto, Hand, Revolver, Shotgun}

public class BulletItemPool : BasePool
{
    private GameObject[] _bPrefabs;
    private BulletInfo[] _infos;
    private GameObject[] _mPrefabs;
    private MagazineInfo[] _mInfo;


    private readonly int _count = 5;
    private readonly string Path = "Prefab/Bullets/";

    public override void Init()
    {
        _pool = new Dictionary<string, Queue<GameObject>>();
        _bPrefabs = Resources.LoadAll<GameObject>("Prefab/Bullets");
        _mPrefabs = Resources.LoadAll<GameObject>("Prefab/Guns/Magazine");

        _infos = new BulletInfo[_bPrefabs.Length];
        _mInfo = new MagazineInfo[_mPrefabs.Length];

        SetInfo();

        for (int i = 0; i < _infos.Length; i++)
            AddDic( _infos[i].NAME, _bPrefabs[i], _count);

        for (int i = 0; i < _mPrefabs.Length; i++)
            AddDic( _mInfo[i].NAME, _mPrefabs[i], 3);

    }

    private void SetInfo()
    {
        for (int i = 0; i < _infos.Length; i++)
            _infos[i] = DataMng.Instance.Table(TableType.BulletTable, i) as BulletInfo;

        for (int i = 0; i < _infos.Length; i++)
            for (int j = 0; j < _bPrefabs.Length; j++)
                if (_infos[i].NAME == _bPrefabs[i].name)
                    _bPrefabs[i].GetComponent<BulletItem>().SetInfo(_infos[i]);

        for (int i = 0; i < _mInfo.Length; i++)
            _mInfo[i] = DataMng.Instance.Table(TableType.MagazineTable, i) as MagazineInfo;

        for (int i = 0; i < _mInfo.Length; i++)
            for (int j = 0; j < _mPrefabs.Length; j++)
                if (_mInfo[i].NAME == _mPrefabs[i].name)
                    _mPrefabs[i].GetComponent<Magazine>().SetInfo(_mInfo[i]);
    }

    public override GameObject Pop(string str = null)
    {
        if (_pool[str].Count == 0)
            Add(str, Path + str);

        if (_pool.ContainsKey(str))
            return _pool[str].Dequeue();

        return null;
    }

    public override void Push(GameObject obj, string str = null)
    {
        if (_pool.ContainsKey(str))
        {
            _pool[str].Enqueue(obj);
            obj.SetActive(false);
        }
    }

    public override void PushAll()
    {
        BulletItem[] objs = GetComponentsInChildren<BulletItem>();

        if (objs != null)
        {
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].gameObject.activeSelf)
                {
                    if (objs[i]._name != null)
                    {
                        string str = objs[i]._name;

                        if (_pool.ContainsKey(str))
                        {
                            _pool[str].Enqueue(objs[i].gameObject);
                            objs[i].gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        Magazine[] magazine = GetComponentsInChildren<Magazine>();

        if (magazine != null)
        {
            for (int i = 0; i < magazine.Length; i++)
                if (magazine[i].gameObject.activeSelf)
                    if (magazine[i]._name != null)
                    {
                        string str = magazine[i]._name;
                        if (_pool.ContainsKey(str))
                        {
                            _pool[str].Enqueue(magazine[i].gameObject);
                            magazine[i].gameObject.SetActive(false);
                        }
                    }
        }
    }

}
