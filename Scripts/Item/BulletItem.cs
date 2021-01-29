using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletItem : MonoBehaviour
{
    private Player _player;
    private readonly float _chasingSpeed = 1.5f;
    private readonly float _chsinngDistance = 3.0f;

    public int _value;
    public BulletType _bulletType;

    public Sprite _image;
    public string _name, _prefabPath, _imagePath, _lore, _uiName;

    public void SetInfo(BulletInfo info)
    {
        _name = info.NAME;
        _value = info.VALUE;
        _bulletType = info.BULLETTYPE;
        _prefabPath = info.PREFABPATH;
        _imagePath = info.IMAGEPATH;
        _lore = info.LORE;
        _uiName = info.UINAME;
        _image = Resources.Load<Sprite>(_imagePath);
    }

    public void Chase()
    {
        _player = FindObjectOfType<Player>();

        float distance = Vector3.Distance(_player.transform.position, transform.position);
        StartCoroutine(IEChasing(_player.transform.position, _chsinngDistance / distance));
    }

    private IEnumerator IEChasing(Vector3 targetPos, float radio = 1.0f)
    {
        yield return new WaitForSeconds(0.5f);

        float percent = 0.0f;

        while (percent <= 1)
        {
            percent += Time.deltaTime * _chasingSpeed * radio;
            transform.position = Vector3.Lerp(transform.position, targetPos, percent);

            yield return null;
        }
        // AddCount ProjectileData
        ItemMng.Instance.AddProjectile(this, _value);
        UIMng.Instance.CallEvent(UIList.Inventory, "AddProjectile", this);
        // Disappear gameObject
        PoolMng.Instance.Push(PoolType.BulletItemPool, gameObject, gameObject.name);
        gameObject.SetActive(false);
    }
}
