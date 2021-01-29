using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePooling : TSingleton<ProjectilePooling>
{
    public List<Projectile> _projectilePool = new List<Projectile>();
    public List<Projectile> _activeProjectilePool = new List<Projectile>();

    private Projectile _projectilePrefab;
    private readonly int _createCount = 100;

    public override void Init()
    {
        _projectilePrefab = Resources.Load<Projectile>("Guns/Bullet");

        CreateProjectile();
    }

    private void CreateProjectile()
    {
        for (int i = 0; i < _createCount; i++)
        {
            Projectile obj = Instantiate(_projectilePrefab, transform);
            _projectilePool.Add(obj);

            obj.gameObject.SetActive(false);
        }
    }

    public void PopByPlayer(Transform muzzle, float speed, float damage, float power, float distance, LayerMask layer)
    {
        if (_projectilePool.Count == 0)
            CreateProjectile();
        
        _projectilePool[0].gameObject.SetActive(true);
        _projectilePool[0].transform.position = muzzle.position;
        _projectilePool[0].transform.rotation = muzzle.rotation;
        _projectilePool[0].SetUp(speed, damage, power, distance, layer);
        _projectilePool[0].Shooting(muzzle.forward);

        _activeProjectilePool.Add(_projectilePool[0]);
        _projectilePool.RemoveAt(0);
    }

    public void PopByEnemy(Transform muzzle, float speed, float damage, float power, float distance, LayerMask layer, Vector3 pos)
    {
        if (_projectilePool.Count == 0)
            CreateProjectile();

        _projectilePool[0].gameObject.SetActive(true);
        _projectilePool[0].transform.position = muzzle.position;
        _projectilePool[0].transform.rotation = muzzle.rotation;
        _projectilePool[0].SetUp(speed, damage, power, distance, layer);
        _projectilePool[0].Shooting(muzzle.forward);

        _activeProjectilePool.Add(_projectilePool[0]);
        _projectilePool.RemoveAt(0);
    }

    public void Push(Projectile obj)
    {
        _activeProjectilePool.Remove(obj);
        _projectilePool.Add(obj);
        obj.gameObject.SetActive(false);
    }
}
