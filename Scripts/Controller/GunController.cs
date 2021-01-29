using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType { Pistol, ShotGun, Auto }

public class GunController : MonoBehaviour
{
    public Transform _weaponPos;
    public Gun _equipedGun;
    private Gun _startGun;
    private int gunIndex = 0;

    public Dictionary<int, Gun> _gunDic = new Dictionary<int, Gun>();
    private readonly string _weaponPosString = "Hips_jnt/Spine_jnt/Spine_jnt 1/Chest_jnt/Shoulder_Right_jnt/Arm_Right_jnt/Forearm_Right_jnt/Hand_Right_jnt/WeaponPos";

    public void Init(string startgunPath)
    {
        _weaponPos = transform.Find(_weaponPosString);
        StartGun(startgunPath);
    }

    //private void FixedUpdate()
    //{
    //    //_equipedGun.transform.Rotate(_weaponPos.localEulerAngles);
    //}

    private void StartGun(string path)
    {
        _startGun = Resources.Load<Gun>("Prefab/Guns/PlayerGuns/" + path);
        _startGun = Instantiate(_startGun, _weaponPos.position, Quaternion.identity);
        _startGun.transform.parent = _weaponPos;

        GunInfo gunInfo = DataMng.Instance.Table(TableType.GunTable, path) as GunInfo;
        _startGun.Init(gunInfo);
        ItemInfo itemInfo = DataMng.Instance.Table(TableType.ItemTable, gunInfo.NAME) as ItemInfo;
        _startGun._item.SetItem(itemInfo);

        _gunDic.Add(_startGun._item._uniqueID, _startGun);
        _equipedGun = _startGun;

        UIMng.Instance.CallEvent(UIList.Inventory, "AddItem", _startGun.GetComponent<Item>());

        GunUIChange(_startGun);
    }

    private void EquipGun(Gun gunToEquip)
    {
        if (_equipedGun != null)
        {
            _equipedGun.enabled = false;
            _equipedGun.gameObject.SetActive(false);
        }

        gunToEquip.gameObject.SetActive(true);
        // 3.1 change: 교체가 될 때 활성화 나머지는 비 활성화! 
        gunToEquip.enabled = true;
        _equipedGun = gunToEquip;
    }

    public void ChangeGun(int ud)
    {
        if (_gunDic.Count >= 2)
        {
            gunIndex += 1 * ud;
            gunIndex = Mathf.Abs(gunIndex % _gunDic.Count);

            // keys에 유니크 아이디를 값으로 저장해 줌 
            List<int> keys = new List<int>(_gunDic.Keys);
            EquipGun(_gunDic[keys[gunIndex]]);

            GunUIChange(_equipedGun);
        }
    }

    private void GunUIChange(Gun gun)
    {
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmo", gun._bulletType);
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmoImage", gun._remainAmmos);
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateGunImage", gun._item._sprite);
    }

    public void OntriggerHold()
    {
        if (_equipedGun != null)
            _equipedGun.OnTriggerHold();
    }

    public void OnTriggerRelease()
    {
        if (_equipedGun != null)
            _equipedGun.OnTriggerRelease();
    }

    public void ChangeFireMode()
    {
        _equipedGun._fireMode++;

        if (_equipedGun._fireMode == Gun.FireMode.ReSet)
        {
            _equipedGun._fireMode = Gun.FireMode.Single;
            return;
        }
    }

    public void Aim(Vector3 pos)
    {
        if (_equipedGun != null)
            _equipedGun.Aim(pos);
    }

    public void Reroad()
    {
        if (_equipedGun != null)
            _equipedGun.Reroad();
    }
    // mouse pointer abjust
    public float GunHeight
    {
        get { return _weaponPos.position.y; }
    }
}
