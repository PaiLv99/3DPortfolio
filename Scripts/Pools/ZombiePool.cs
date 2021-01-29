using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : BasePool
{
    private List<GameObject> _zombiesList = new List<GameObject>();
    private List<GameObject> _activeZombiesList = new List<GameObject>();

    private GameObject[] _prefabs;
    private ZombiesInfo[] _infos;

    private readonly int _creatCount = 4;

    public override void Init()
    {
        _poolType = PoolType.ZombiePool;

        _prefabs = Resources.LoadAll<GameObject>("Prefab/Zombies");
        _infos = new ZombiesInfo[_prefabs.Length];

        for (int i = 0; i < _infos.Length; i++)
            _infos[i] = DataMng.Instance.Table(TableType.ZombiesTable, i) as ZombiesInfo;

        CreateZombie(_creatCount);
    }

    private void CreateZombie(int count)
    {
        for (int i = 0; i < _prefabs.Length; i++)
        {
            for (int j = 0; j < count; j++)
                CreateZombies(_prefabs[i], _infos[i]);
        }
    }

    private void CreateZombies(GameObject prefab, ZombiesInfo info)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.GetComponent<Enemy>().DataSetUp(info);

        if (obj.GetComponent<GunEnemy>() != null)
            obj.GetComponent<GunEnemy>().DataSetUp(info);

        _zombiesList.Add(obj);
        obj.SetActive(false);
    }

    //private void CreateZombies(GameObject prefab, ZombiesInfo info)
    //{
    //    GameObject obj = Instantiate(prefab, transform);
    //    obj.GetComponent<EnemyAI>().Init(info);


    //    _zombiesList.Add(obj);
    //    obj.SetActive(false);
    //}

    public override GameObject Pop(string str = null)
    {
        // 풀링할 객체가 없다면 새롭게 생성해준다.
        if (_zombiesList.Count == 0)
            CreateZombie(2);

        int randomNum = Random.Range(0, _zombiesList.Count);

        GameObject obj = _zombiesList[randomNum];

        _activeZombiesList.Add(_zombiesList[randomNum]);
        _zombiesList.RemoveAt(randomNum);

        return obj;
    }

    public override void Push(GameObject obj, string str)
    {
        obj.GetComponent<Chasing>()._order.Clear();
        _zombiesList.Add(obj);
        _activeZombiesList.Remove(obj);

        obj.transform.position = new Vector3(100, 0, 100);
        obj.gameObject.SetActive(false);
    }

    public void ClearSpawnedEnemy()
    {
        for (int i = 0; i < _activeZombiesList.Count; i++)
        {
            _activeZombiesList[i].GetComponent<Chasing>()._order.Clear();
            _zombiesList.Add(_activeZombiesList[i]);
            _activeZombiesList[i].transform.position = new Vector3(100, 0, 100);
            _activeZombiesList[i].gameObject.SetActive(false);
        }
        _activeZombiesList.Clear();
    }
}
