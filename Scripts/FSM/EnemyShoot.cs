using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour, IState
{
    private Animator _animator;
    private Player _player;
    private float _damage, _power, _speed, _ditance;
    public LayerMask _layer;
    private Transform[] _muzzle;
    private GunEnemy _enemy;

    private EnemyFSM _fsm;
    private EnemyState _prevState;
    private float _elapsedTime;

    public void Init()
    {
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<GunEnemy>();
        _layer = 1 << LayerMask.NameToLayer("Player");

        if (_enemy._enemyType != EnemyType.Melee)
            _muzzle = _enemy._muzzle;
        _damage = _enemy._shootDamage;
        _power = _enemy._shootPower;
        _speed = _enemy._shootSpeed;
        _ditance = _enemy._bulletDistance;

        _fsm = GetComponent<EnemyFSM>();
    }

    private void Shooting()
    {
        Vector3 target = _player.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(target);
        _animator.SetInteger("GUNTYPE", 0);
        _animator.SetBool("SHOOTIDLE", true);
        _animator.SetTrigger("ZOMBIESHOT");
    }

    private void Once()
    {
        for (int i = 0; i < _muzzle.Length; i++)
        {
            GameObject bullet = PoolMng.Instance.Pop(PoolType.ProjectilePool, "Bullet");
            bullet.gameObject.SetActive(true);
            bullet.transform.position = _muzzle[i].position;
            bullet.transform.rotation = _muzzle[i].rotation;
            bullet.GetComponent<Projectile>().SetUp(_damage, _power, _speed, _ditance, _layer);
            bullet.GetComponent<Projectile>().Shooting(_muzzle[i].forward);
            bullet.GetComponent<Projectile>().SetTrail(true);
        }
    }

    public void BurstShot(int num)
    {
        StartCoroutine(IEBurstShot(num));
    }

    public IEnumerator IEBurstShot(int num)
    {
        for (int j = 0; j < num; j++)
        {
            for (int i = 0; i < _muzzle.Length; i++)
            {
                Shooting();
                Once();
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    public void Enter()
    {
        _prevState = _fsm._state;
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= 1)
        {
            Vector3 target = _player.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(target);
            _animator.SetInteger("GUNTYPE", 0);
            _animator.SetBool("SHOOTIDLE", true);
            _animator.SetTrigger("ZOMBIESHOT");
            _elapsedTime = 0;
            Execute();
        }
    }

    public void Execute()
    {
        switch (_enemy._enemyType)
        {
            case EnemyType.Soldier: BurstShot(3); break;
            case EnemyType.Industrial: Once(); break;
            case EnemyType.Mountie: Once(); break;
        }
    }

    public void Exit()
    {
        _fsm._prevState = _prevState;
    }
}
