using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask _obstacleMask;
    public LayerMask _wallMask;
    public LayerMask _layer;
    //private int _layerMask;
    public Color _trailColor;

    private float _speed;
    private float _damage;
    private float _power;
    private float _distance;
    private float _currDistance;
    private readonly float skinWidth = 0.1f;

    private TrailRenderer _trail;

    // 발사체마다 다른 속도를 만들기 위해 만든 함수 
    public void SetUp(float damage, float power, float speed, float distance, LayerMask _layerMask)
    {
        _damage = damage;
        _power = power;
        _speed = speed;
        _distance = distance;
        _layer = _layerMask;

    }

    private void PropertyClear()
    {
        _speed = 0;
        _damage = 0;
        _distance = 0;
        _power = 0;
        _layer = 0;
        _currDistance = 0;
    }

    private void InsideEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, .1f, _layer);
       
        if (colliders.Length > 0)
            OnHit(colliders[0], transform.position);
    }

    private void Start()
    {
        InsideEnemy();
        GetComponent<TrailRenderer>().material.SetColor("_TintColor", _trailColor);
        _trail = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate()
    {
        float moveDistance = _speed * Time.fixedDeltaTime;
        CheckCollision(moveDistance);

        _currDistance += moveDistance;

        if (_distance <= _currDistance)
            PushToPool();
    }

    public void Shooting(Vector3 dir)
    {
        transform.GetComponent<Rigidbody>().velocity = dir * _speed;
    }

    private void CheckCollision(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, moveDistance + skinWidth, _layer, QueryTriggerInteraction.Collide))
            OnHit(hit.collider, hit.point);
        else if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, _obstacleMask, QueryTriggerInteraction.Collide))
            PushToPool();
        else if (Physics.Raycast(ray, out hit, moveDistance + skinWidth, _wallMask, QueryTriggerInteraction.Collide))
            PushToPool();
    }

    private void OnHit(Collider hit, Vector3 hitPoint)
    {
        hit.transform.GetComponent<Rigidbody>().AddForce(transform.position * _power, ForceMode.Force);

        IDamageable damageable = hit.GetComponent<IDamageable>();

        if (damageable != null)
            damageable.TakeHit(_damage, hitPoint, transform.forward);

        PushToPool();
    }

    private void PushToPool()
    {
        PropertyClear();
        SetTrail(false);
        PoolMng.Instance.Push(PoolType.ProjectilePool, gameObject, "Bullet");
    }

    public void SetTrail(bool state)
    {
        if (_trail != null)
            _trail.enabled = state;
    }

    public void Setting(Transform transform)
    {
        this.transform.position = transform.position;
        this.transform.rotation = transform.rotation;
    }
}
