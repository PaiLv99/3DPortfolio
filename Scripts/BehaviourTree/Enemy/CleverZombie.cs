using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleverZombie : MonoBehaviour, IDamageable
{
    protected Animator _animator;
    private EnemyAI _ai;

    public virtual void SetData(ZombiesInfo info)
    {

    }


    public void Init()
    {

    }
    #region 데미지 인터페이스 
    public void TakeDamage(float damage)
    {
        
    }

    public void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        
    }
    #endregion
}
