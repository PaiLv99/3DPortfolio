using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BaseChar : MonoBehaviour, IDamageable
{
    public Animator _animator;

    public float _hp;
    protected bool dead = false;
    public event System.Action OnDeath;

    //protected GameObject _damageText;

    public virtual void Die()
    {
        dead = true;
        if (OnDeath != null)
            OnDeath();
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (_hp >= damage)
        {
            GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool ,"DamagePopUp");
            if (obj != null)
                EffectMng.Instance.StartEffect(obj, EffectType.DamagePopUp.ToString(), hitPoint, Quaternion.identity, transform, damage);
        }
        TakeDamage(damage); 
    }

    public virtual void TakeDamage(float damage)
    {
        _hp -= damage;
        if (_hp <= 0)
            Die();
    }

    public virtual Node FindNode()
    {
        Vector3 pos = transform.position;
        pos.y += 1;

        Ray ray = new Ray(pos, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10, 1 << 10))
        {
            if (hit.transform.GetComponent<Node>().NodeType == NodeType.None)
                return hit.transform.GetComponent<Node>();
        }
        return null;
    }
}
