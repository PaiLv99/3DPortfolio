using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Soldier, Melee, Mountie, Industrial, }

[RequireComponent(typeof (CreateItem))]
[RequireComponent(typeof (Rigidbody))]
public class Enemy : BaseChar 
{
    //private Rigidbody _rigidbody;
    //protected int _hitCount;
    //protected int _attackCount;
    //protected bool _isFlee = false;
    //protected readonly Vector3 _offset = new Vector3(0, 1, 0);

    public EnemyType _enemyType;
    protected Player _player;
    private CreateItem _createItem;

    private float _speed;
    public float _chasingDistance;
    public float _rangeDistance;
    public float _attackDistance = 2.0f;
    private int _hasExp;

    private readonly float[] _itemDropProb = { 0.7f, 0.2f, 0.1f };
    private readonly float[] _bulletProb = { .25f, .25f, .25f, .25f };

    [SerializeField]
    public List<Node> _path = new List<Node>();

    public EnemyFSM _enemyFSM;

    public virtual void Init()
    {
        _player = FindObjectOfType<Player>();
        _createItem = GetComponent<CreateItem>();

        if (_enemyFSM == null)
        {
            _enemyFSM = GetComponent<EnemyFSM>();
            _enemyFSM.Init();
        }

        // Player가 죽을 때 
        _player.OnDeath += TargetDeath;
        // Enemy가 죽을 때 
        //OnDeath += CreateItem;
        OnDeath += CreateBullet;
        OnDeath += EnemyDie;
        OnDeath += _player._status.MakeGem;
    }

    //float startHealth, int attackDamage, float speed, int exp, float chasingDistance, EnemyType enemyType,
    public virtual void DataSetUp(ZombiesInfo info)
    {
        _hp = info.HP;
        //_attackDamage = info.ATTACKDAMAGE;
        _speed = info.MOVESPEED;
        _hasExp = info.EXP;
        _chasingDistance = info.CHASINGDISTANCE;
        _enemyType = info.ENEMYTYPE;
    }

    #region OnDeath Method
    private void CreateItem()
    {
        switch (Helper.Choose(_itemDropProb))
        {
            case 0:
                //GetPos(CreateBullet(Helper.Choose(_bulletProb)));
                break;
            case 1:
                GameObject whisky = PoolMng.Instance.Pop(PoolType.ItemPool, ItemUseType.Whisky.ToString());
                whisky.gameObject.SetActive(true);
                GetPos(whisky);
                break;
            case 2:
                GameObject food = PoolMng.Instance.Pop(PoolType.ItemPool, ItemUseType.Food.ToString());
                food.gameObject.SetActive(true);
                GetPos(food);
                break;
        }
    }

    private void CreateBullet()
    {
        switch(Helper.Choose(_bulletProb))
        {
            case 0: 
                GameObject auto = PoolMng.Instance.Pop(PoolType.BulletItemPool, "AutoAmmo");
                auto.gameObject.SetActive(true);
                GetPos(auto);
                break;
            case 1: GameObject hand = PoolMng.Instance.Pop(PoolType.BulletItemPool, "HandAmmo");
                hand.gameObject.SetActive(true);
                GetPos(hand);
                break;
            case 2: GameObject revolver = PoolMng.Instance.Pop(PoolType.BulletItemPool, "RevolverAmmo");
                revolver.gameObject.SetActive(true);
                GetPos(revolver);
                break;
            case 3: GameObject shotgun = PoolMng.Instance.Pop(PoolType.BulletItemPool, "ShotgunAmmo");
                shotgun.gameObject.SetActive(true);
                GetPos(shotgun);
                break;
        }
    }

    private void EnemyDie()
    {
        _player._status.AddExp(_hasExp);
        UIMng.Instance.CallEvent(UIList.Result, "KillCount");   // ResultUI._killCount++;
        SoundMng.Instance.OncePlay("EnemyDie");
        PoolMng.Instance.Push(PoolType.ZombiePool, gameObject);
        SpawnMng.Instance.CheckNewWave(1);
    }

    // 타겟이 죽었을 때 해야할 처리 : 추적을 멈춰야한다 
    private void TargetDeath()
    {
        _player.IsDeath = true;
        _enemyFSM.SetState(EnemyState.Idle);
        StopCoroutine(_enemyFSM.IEStateCheker());
    }

    private void GetPos(GameObject obj)
    {
        obj.transform.position = transform.position;
        obj.transform.rotation = Quaternion.identity;
    }
    #endregion

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (damage >= _hp)
        {
            GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool, EffectType.DeathEffect.ToString());
            EffectMng.Instance.StartEffect(obj, EffectType.DeathEffect.ToString(), hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
        }
        base.TakeHit(damage, hitPoint, hitDirection);
        //++_hitCount;
    }
}
