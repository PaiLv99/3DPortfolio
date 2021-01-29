using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    private readonly float _power = 10f;
    private readonly float _radio = 5f;
    private readonly float _upForce = 10f;
    private readonly int _damage = 10;

    public void Detonate()
    {
        Vector3 explosionPos = transform.position;

        Collider[] colliders = Physics.OverlapSphere(explosionPos, _radio, 1 << LayerMask.NameToLayer("Enemy"));
        GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool, EffectType.BombEffect.ToString());
        EffectMng.Instance.StartEffect(obj, EffectType.BombEffect.ToString(), transform.position, Quaternion.identity);

        for (int i = 0; i < colliders.Length; ++i)
        {
            Rigidbody rig = colliders[i].GetComponent<Rigidbody>();
            IDamageable damage = colliders[i].GetComponent<IDamageable>();

            if (damage != null)
                damage.TakeDamage(_damage);

            if (rig != null)
                rig.AddExplosionForce(_power, explosionPos, _radio * 10f, _upForce, ForceMode.VelocityChange);
        }

        SoundMng.Instance.OncePlay("Bomb");
    }
}
