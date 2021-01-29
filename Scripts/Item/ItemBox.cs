using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    private enum ItemName { Food, Water, Whisky, Medikit, Grenade, }
    private readonly float _time = 0.5f;

    private readonly float[] _boxProb = { 0.8f, 0.2f };
    private readonly float[] _usedProb = { 0.2f, 0.2f, 0.2f, 0.1f, 0.1f, 0.2f };
    private readonly float[] _projectileProb = { .25f, .25f, .25f, .25f };
   
    private readonly string Path = "Prefab/Guns/PlayerGuns/";
   

    private void CreateItem(float index)
    {   
        switch(index)
        {
            case 0:
                Debug.Log("Item");
                switch (Helper.Choose(_usedProb))
                {
                    case 0: PopObject(PoolType.ItemPool, "Food"); break;
                    case 1: PopObject(PoolType.ItemPool, "Whisky"); break;
                    case 2: PopObject(PoolType.ItemPool, "Water"); break;
                    case 3: PopObject(PoolType.ItemPool, "Medikit"); break;
                    case 4: PopObject(PoolType.ItemPool, "Grenade"); break;
                    case 5: PopObject(PoolType.ItemPool, "MysteryFood"); break;
                    case 6: PopObject(PoolType.ItemPool, "RottenFood"); break;
                }
                break;
            case 1: CreateGun(); break;
        }
        PoolMng.Instance.Push(PoolType.ItemPool, gameObject, "Itembox");
    }

    private void CreateGun()
    {
        Debug.Log("CreateGun");
        int gunLength = DataMng.Instance.TableLength(TableType.GunTable);
        int gunIdx = Random.Range(0, 13);
        GunInfo gunInfo = DataMng.Instance.Table(TableType.GunTable, gunIdx) as GunInfo;
        Gun gun = Instantiate(Resources.Load<Gun>(Path + gunInfo.NAME), transform.position, Quaternion.identity);
        gun.Init(gunInfo);

        ItemInfo info = DataMng.Instance.Table(TableType.ItemTable, gunInfo.NAME) as ItemInfo;
        gun.GetComponent<Item>().SetItem(info);

        gun.enabled = false;
    }

    private void PopObject(PoolType type, string itemType)
    {
        GameObject obj = PoolMng.Instance.Pop(type, itemType);
        obj.gameObject.SetActive(true);
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
    }

    public void BoxOpen()
    {
        GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool , EffectType.BoxOpenEffectO.ToString());
        EffectMng.Instance.StartEffect(obj, EffectType.BoxOpenEffectO.ToString(), transform.position, transform.rotation, transform);
        StartCoroutine(IECreateItem(_time));
    }

    private IEnumerator IECreateItem(float targetTime)
    {
        yield return new WaitForSeconds(targetTime);
        CreateItem(Helper.Choose(_boxProb));
    }
}
