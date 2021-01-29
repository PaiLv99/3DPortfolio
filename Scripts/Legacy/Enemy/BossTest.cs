using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTest : Enemy
{
    public int _projectileNum;
    public int _projectileSpeed;
    public Projectile _projectileP;
    public Transform _projectilePos;
    public Transform[] _shotGunMuzzle;
    public GameObject[] _weaponObjs;

    // ShootA : 
    private Vector3 _startPos;
    private const float _circleRadius = 1f;

    private float _elasedTime;
    //private float shootDistanceA = 3;
    //private float shootDistanceB = 5;
    //private float shootDistanceC = 7;


    private Sprite _keySprite;

    public override void Init()
    {
        base.Init();
        OnDeath += BossDie;
        _keySprite = Resources.Load<Sprite>("Image/Key");
        _animator.SetBool("EATING", true);
    }

    private void BossDie()
    {
        UIMng.Instance.CallEvent(UIList.Result, "Open");
    }

    private void MakeKey()
    {

    }
}
