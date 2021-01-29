using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour, IState
{
    private Player _player;
    private float _radius, _targetRadius;
    private Animator _animator;
    
    private int _attackDamage = 1;
    private float _attackTime = 1.0f;
    private float _elapsedAttackTime;

    private EnemyFSM _fsm;
    private EnemyState _prevState;

    public void Init()
    {
        _player = FindObjectOfType<Player>();
        _radius = GetComponent<CapsuleCollider>().radius;
        _targetRadius = _player.GetComponent<CapsuleCollider>().radius;
        _animator = GetComponent<Animator>();
        _fsm = GetComponent<EnemyFSM>();
    }

    public void Action()
    {
        AttackAction();
    }

    private void AttackAction()
    {
        if (Time.time > _elapsedAttackTime)
        {
            transform.LookAt(_player.transform);
            _elapsedAttackTime = Time.time + _attackTime;
            _animator.SetBool("SHOOTIDLE", false);
            _animator.SetTrigger("ZOMBIEATTACK");
            StartCoroutine(IEAttack());
        }
    }

    private IEnumerator IEAttack()
    {
        Vector3 originPos = transform.position;
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        Vector3 attackPos = _player.transform.position - dir * (_radius + _targetRadius);

        float attackSpeed = 1f;
        float percent = 0;
        bool hasDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasDamage)
            {
                hasDamage = true;
                _player.TakeDamage(_attackDamage);
                UIMng.Instance.CallEvent(UIList.HUD, "UpdateHpBar");
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originPos, attackPos, interpolation);

            yield return null;
        }
    }

    public void Enter()
    {
        _prevState = _fsm._state;

        if (Time.time > _elapsedAttackTime)
        {
            transform.LookAt(_player.transform);
            _elapsedAttackTime = Time.time + _attackTime;
            _animator.SetBool("SHOOTIDLE", false);
            _animator.SetTrigger("ZOMBIEATTACK");
            Execute();
        }
    }

    public void Execute()
    {
        StartCoroutine(IEAttack());
    }

    public void Exit()
    {
        _fsm._prevState = _prevState;
    }
}
