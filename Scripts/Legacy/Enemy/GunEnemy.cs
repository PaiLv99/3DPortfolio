using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunEnemyShootType { Once, Burst, circle, Auto }

public class GunEnemy : Enemy
{
    public Transform[] _muzzle;

    // circle shoot variable
    // private const float _shootRadius = 1f;

    // shoot variable
    public float _shootDamage, _shootPower, _shootSpeed, _bulletDistance;
  
    //float startHealth, int attackDamage, float speed, int exp,float chasingDistance, EnemyType enemyType,
    public override void DataSetUp(ZombiesInfo info)
    {
        base.DataSetUp(info);

        _rangeDistance = info.SHOOTDISTANCE;
        _shootDamage = info.SHOOTDAMAGE;
        _bulletDistance = info.BULLETDISTANCE;
        _shootSpeed = info.SHOOTSPEED;
        _shootPower = info.SHOOTPOWER;
    }

    //private void CircleShoot(int countOfProjectile)
    //{
    //    float angleStep = 360f / countOfProjectile;
    //    float angle = 0f;

    //    for (int i = 0; i <= countOfProjectile - 1; i++)
    //    {
    //        // 방향 계산
    //        float projectileDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * _shootRadius;
    //        float projectileDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * _shootRadius;

    //        Vector3 projectileVector = new Vector3(projectileDirX, projectileDirY, 0);
    //        Vector3 projectileMoveDir = (projectileVector - transform.position).normalized;
    //        ProjectilePooling.Instance.PopByEnemy(_muzzle[i], _shootSpeed, _shootDamage, _shootPower, _bulletDistance, _layer, new Vector3(projectileMoveDir.x, 0, projectileMoveDir.y));
    //        angle += angleStep;
    //    }
    //    elapsedTime = 0;
    //}
}
