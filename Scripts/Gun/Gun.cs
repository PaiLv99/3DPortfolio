using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum FireMode { Single, Burst, Auto, ReSet,}

    public Gem[] _damageSocket, _powerSocket, _speedSocket;
     // GunGem value;
    public float AddDamage { get;  set; }
    public float AddPower { get; set; }
    public float AddSpeed { get; set; }
    // Gun Parts
    private Transform _muzzleArr;
    private Transform _magazinePos;
    private Transform[] _muzzle;
    public Transform[] _cartridgeArr;
    public MuzzleFlash _muzzleFlash;
    // Gun Status Variable
    private float _shootSpeed, _bulletSpeed, _damage, _power, _distance, _reloadTime;

    public Sprite[] _remainAmmos = new Sprite[2];
    public int ReloadAmmo { get; private set; }
    public int CurrAmmo { get; private set; }

    private string _magazinePath;
    public BulletType _bulletType;
    private int _muzzleCount;
    private float _elapsedTime;

    private readonly int _burstCount = 3;
    private int _remainBurstCount;
    private bool _triggerRelease = true;
    public FireMode _fireMode = FireMode.Single;

    // reroad Variable
    private bool _isReload = false;
    private bool _magBool = false;
    // recoil Variable
    private Vector2 mmRecoilForce = new Vector2(.05f, .1f);
    private Vector2 mmRecoilAngle = new Vector2(10, 15);
    private Vector3 recoilVelocity;
    private float recoilAngle = 0, recoilAngleVelocity;
    private bool _isShoot = false;
    //public bool _isActive = false;
    // GunType Value
    public GunType _gunType;
    private LayerMask _layer;
    // Gun AMMO UI Symbolic Const
    private const int AMMO = 0, BG = 1; 

    // Gun Parts Component
    public int _uniqueID;
    public Item _item;

    public void Init(GunInfo info)
    {
        _item = GetComponent<Item>();
        DataSetUp(info);

        // 총기(발사체 위치, 탄피 위치, 총구화염 위치) 초기화
        _magazinePos = transform.Find("Magazine");

        _muzzleFlash = GetComponent<MuzzleFlash>();
        _remainBurstCount = _burstCount;
        CurrAmmo = ReloadAmmo;
        _muzzleArr = Helper.Find<Transform>(transform, "MuzzleArr");
        _muzzle = new Transform[_muzzleCount];

        for (int i = 0; i < _muzzleCount; i++)
            _muzzle[i] = _muzzleArr.GetChild(i);

        _layer = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Boss");

        //_uniqueID = GetInstanceID();
        _item._uniqueID = GetInstanceID();

    }
    // 발사체 생성 
    private void Shoot()
    {
        if (Time.time > _elapsedTime && CurrAmmo > 0 && CurrAmmo >= _muzzle.Length && !_isReload )
        {
            _isShoot = true;
            _elapsedTime = Time.time + _shootSpeed - AddSpeed;

            if (_fireMode == FireMode.Burst)
            {
                if (_remainBurstCount == 0)
                    return;
                _remainBurstCount--;
            }
            else if (_fireMode == FireMode.Single)
            {
                if (!_triggerRelease)
                    return;
            }

            for (int i = 0; i < _muzzle.Length; ++i)
            {
                GameObject bullet = PoolMng.Instance.Pop(PoolType.ProjectilePool, ProjectilePoolType.Bullet.ToString());

                bullet.transform.position = _muzzle[i].position;
                bullet.transform.rotation = _muzzle[i].rotation;
                bullet.GetComponent<Projectile>().SetUp(_damage, _power, _bulletSpeed, _distance, _layer);
                bullet.GetComponent<Projectile>().Shooting(_muzzle[i].forward);
                bullet.GetComponent<Projectile>().SetTrail(true);

                --CurrAmmo;

                float delta = CurrAmmo / (float)ReloadAmmo;

                UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmoImage", _remainAmmos);
                UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmo", _bulletType);
            }
            _muzzleFlash.Activate();

            if (_cartridgeArr != null)
            {
                for (int i = 0; i < _cartridgeArr.Length; ++i)
                {
                    GameObject cartrige = PoolMng.Instance.Pop(PoolType.ProjectilePool, ProjectilePoolType.Cartridge.ToString());
                    cartrige.transform.position = _cartridgeArr[i].position;
                    cartrige.transform.rotation = _cartridgeArr[i].rotation;
                }
            }
            transform.position -= transform.forward * Random.Range(mmRecoilForce.x, mmRecoilForce.y);
            recoilAngle += Random.Range(mmRecoilAngle.x, mmRecoilAngle.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 20);

            SoundMng.Instance.OncePlay("Shot", 0.8f);
        }
        _isShoot = false;

        if (CurrAmmo == 0)
            UIMng.Instance.CallEvent(UIList.ActionUI, "Reload");
    }
    // 발사 입력
    public void OnTriggerHold()
    {
        Shoot();
        _triggerRelease = false;
    }
    // 발사 입력 종료
    public void OnTriggerRelease()
    {
        _triggerRelease = true;
        _remainBurstCount = _burstCount;
    }
    // 재장전
    public void Reroad()
    {
        if (!_isReload)
        {
            UIMng.Instance.CallEvent(UIList.ActionUI, "ReloadDis");
            SoundMng.Instance.OncePlay("Reload", 0.8f);
            StartCoroutine(IEReload());
        }
    }
    // 재장전 코루틴
    private IEnumerator IEReload()
    {
        _isReload = true;
        float interpolation = 0;
        //bool state = true;

        UIMng.Instance.CallEvent(UIList.HUD, "SetReload", true);

        while (interpolation < _reloadTime)
        {
            interpolation += Time.deltaTime;
            UIMng.Instance.CallEvent(UIList.HUD, "UpdateReload", interpolation);

            if (interpolation > .5f && !_magBool)
            {
                if (_magazinePos != null && _magazinePath != "None")
                {
                    _magBool = true;

                    GameObject magazine = PoolMng.Instance.Pop(PoolType.BulletItemPool, _magazinePath);
                    magazine.gameObject.SetActive(true);
                    magazine.transform.position = _magazinePos.position;
                    magazine.transform.rotation = _magazinePos.rotation;
                    magazine.GetComponent<Magazine>().Func();
                }
            }
            yield return null;
        }

        _isReload = false;
        _magBool = false;
        var ammoTofill = Mathf.Clamp(ReloadAmmo - CurrAmmo, 0, ItemMng.Instance.CheckValue(_bulletType));
        CurrAmmo += ammoTofill;

        float delta = CurrAmmo / (float)ReloadAmmo;
        ItemMng.Instance.SubProjectileCount(_bulletType, ammoTofill);
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmoImage", _remainAmmos);
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateRemainAmmo", _bulletType);
        UIMng.Instance.CallEvent(UIList.HUD, "SetReload", false);
    }
    // 에임 조절
    public void Aim(Vector3 targetPos)
    {
        if (!_isReload)
            transform.LookAt(targetPos);
    }
    // 총 발사 후 애니메이션
    private void Recoil()
    {
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilVelocity, .1f);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilAngleVelocity, .1f);
        transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;
    }
    // 총 생성시 gunTable에서 읽어 들어온 데이터로 status 초기화 
    public void DataSetUp(GunInfo info)
    {
        _shootSpeed = info.SHOOTSPEED;
        _bulletSpeed = info.BULLETSPEED;
        _damage = info.DAMAGE;
        _power = info.POWER;
        _distance = info.MAXDISTANCE;
        ReloadAmmo = info.RELOADCOUNT;
        _bulletType = info.BULLETTYPE;
        _reloadTime = info.RELOADTIME;
        _damageSocket = new Gem[info.DAMAGESOCKET];
        _powerSocket = new Gem[info.POWERSOCKET];
        _speedSocket = new Gem[info.SPEEDSOCKET];

        // 현재 탄창 채워주는 용도
        CurrAmmo = ReloadAmmo;

        _magazinePath = info.MAGAZINEPREFAB;
        _muzzleCount = info.MUZZLECOUNT;

        _remainAmmos[AMMO] = Resources.Load<Sprite>(info.REMAINAMMOIMAGE);
        _remainAmmos[BG] = Resources.Load<Sprite>(info.REMAINAMMOBG);
    }

    private void LateUpdate()
    {
        if (!_isShoot && !_isReload) // && _isActive
            Recoil();
    }
}
