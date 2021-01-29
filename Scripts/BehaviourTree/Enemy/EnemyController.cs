using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour, IDamageable
{
    private Animator animator;
    private LayerMask layer = 1 << LayerMask.NameToLayer("Player");

    private float meleeDamage = 1;
    private float _radius, _targetRadius;

    private float startHP;
    public float threshold = 30;
    private int healForFrame = 1;
    private float currhealth;
    public float CurrHP { get { return currhealth; } set { currhealth = Mathf.Clamp(value, 0, startHP); } }

    private float shootDamage = 2.0f;
    private float shootPower = 1.0f;
    private float bulletSpeed = 5.0f;
    private float bulletDistance = 20.0f;

    public float ShootRange { get; private set; }
    public float ChaseRange { get; private set; }
    public float MeleeRange { get; private set; }
    public float Speed { get; private set; }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetData(ZombiesInfo info)
    {
        startHP = info.HP;
        currhealth = startHP;

        bulletSpeed = info.SHOOTSPEED;
        Speed = info.MOVESPEED;

        ShootRange = info.SHOOTDISTANCE;
        shootDamage = info.SHOOTDAMAGE;
        shootPower = info.SHOOTPOWER;
        bulletDistance = info.SHOOTDISTANCE;
        MeleeRange = 2;
    }

    public void Heal()
    {
        CurrHP += Time.deltaTime * healForFrame;
    }

    public void Chase(bool state)
    {
        animator.SetBool("Chase", state);
    }

    public void Idle()
    {
        animator.SetBool("Chase", false);
    }    

    public void Shoot(Transform player, Transform muzzle)
    {
        Vector3 dir = player.position - muzzle.position;
        Quaternion.LookRotation(dir);

        animator.SetTrigger("Shoot");

        Projectile bullet = Instantiate(Resources.Load<Projectile>("Prefab/Guns/Projectile/Bullet"));
        bullet.Setting(muzzle);
        bullet.SetUp(shootDamage, shootPower, bulletSpeed, bulletDistance, layer);
        bullet.Shooting(muzzle.forward);
        bullet.SetTrail(true);
    }

    public void MeleeAttack(Transform player)
    {
        animator.SetTrigger("Melee");
        StartCoroutine(IEAttack(player));
    }

    private IEnumerator IEAttack(Transform player)
    {
        Vector3 originPos = transform.position;
        Vector3 dir = (player.transform.position - transform.position).normalized;
        Vector3 attackPos = player.transform.position - dir * (_radius + _targetRadius);

        float attackSpeed = 1f;
        float percent = 0;
        bool hasDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasDamage)
            {
                hasDamage = true;
                player.GetComponent<IDamageable>().TakeDamage(meleeDamage);
                UIMng.Instance.CallEvent(UIList.HUD, "UpdateHpBar");
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originPos, attackPos, interpolation);

            yield return null;
        }
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool, "DamagePopUp");
        if (obj != null)
            EffectMng.Instance.StartEffect(obj, EffectType.DamagePopUp.ToString(), hitPoint, Quaternion.identity, transform, damage);

        TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        CurrHP -= damage;
        if (CurrHP <= 0)
            Die();
    }

    private void Die()
    {
        CurrHP = startHP;

        //_player._status.AddExp(_hasExp);

        UIMng.Instance.CallEvent(UIList.Result, "KillCount");   // ResultUI._killCount++;
        SoundMng.Instance.OncePlay("EnemyDie");
        PoolMng.Instance.Push(PoolType.ZombiePool, gameObject);
        SpawnMng.Instance.CheckNewWave(1);
    }
}
