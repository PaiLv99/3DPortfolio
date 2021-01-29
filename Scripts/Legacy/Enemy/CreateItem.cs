using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
    private readonly float[] _itemDropProb = { 0.5f, 0.2f, 0.2f, 0.1f };
    private readonly float[] _bulletProb = { .25f, .25f, .25f, .25f };

    public void Item()
    {
        switch (Helper.Choose(_itemDropProb))
        {
            case 0: break;
            case 1:
                GetPos(Bullet(Helper.Choose(_bulletProb)));
                break;
            case 2:
                GameObject whisky = PoolMng.Instance.Pop(PoolType.ItemPool, ItemUseType.Whisky.ToString());
                whisky.gameObject.SetActive(true);
                GetPos(whisky);
                break;
            case 3:
                GameObject food = PoolMng.Instance.Pop(PoolType.ItemPool, ItemUseType.Food.ToString());
                food.gameObject.SetActive(true);
                GetPos(food);
                break;
        }
    }

    public GameObject Bullet(float rand)
    {
        switch (rand)
        {
            case 0:
                GameObject auto = PoolMng.Instance.Pop(PoolType.BulletItemPool, "AutoAmmo");
                auto.gameObject.SetActive(true);
                return auto;
            case 1:
                GameObject hand = PoolMng.Instance.Pop(PoolType.BulletItemPool, "HandAmmo");
                hand.gameObject.SetActive(true);
                return hand;
            case 2:
                GameObject revolver = PoolMng.Instance.Pop(PoolType.BulletItemPool, "Revolver");
                revolver.gameObject.SetActive(true);
                return revolver;
            case 3:
                GameObject shotgun = PoolMng.Instance.Pop(PoolType.BulletItemPool, "ShotgunAmmo");
                shotgun.gameObject.SetActive(true);
                return shotgun;
        }
        return null;
    }

    private void GetPos(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
    }
}
