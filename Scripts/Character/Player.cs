using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

// 플레이어의 입력이 오는 곳 입력을 체크하는 기능을 하는 스크립트
[RequireComponent (typeof(PlayerStatus))]
[RequireComponent (typeof(PlayerController))]
[RequireComponent (typeof(GunController))]
[RequireComponent (typeof(BombController))]
[RequireComponent (typeof(GetItem))]
[RequireComponent (typeof(LineRenderer))]
public class Player : BaseChar
{
    private PlayerController _playerController;
    private BombController _bombController;
    private GetItem _getItem;
    private InventoryUI _inventory;
    public PlayerStatus _status;
    public GunController _gunController;
    private ParticleSystem _dustEffect;

    public float _moveSpeed = 5;
    private readonly float _dashTime = 1.5f;
    private readonly float _dashDistance = 4;
    private readonly float _knockBackTime = 2;
    private float _dashElapsedTime;
    private float _knockElapsedTime;

    private bool IsShoot { get; set; } = false;
    public bool IsBomb { get; set; } = false;
    // gunController.ChangeGun이 완료될 시 바꾸어주어야 한다 How?? 4.6   
    private bool IsChangeGun { get; set; } = false;
    //private bool _isDeath = false;
    public bool IsDeath { get; set; } = false;
    public bool IsDash { get; set; }

    private float _idleTime = 0;
    private readonly float _idleTargetTime = 2;

    private LayerMask _wallLayer, _obstacle;

    public void Init(CharInfo info)
    {
        _wallLayer = LayerMask.NameToLayer("Wall");
        _obstacle = LayerMask.NameToLayer("Obstacle");

        InitComponent(info);
        OnDeath += DestroyPlayer;
        _animator.SetBool("IDLE", true);

        _dustEffect = Instantiate(Resources.Load<ParticleSystem>("Effect/DustEffect"), transform.position, Quaternion.Euler( new Vector3(-5, 180, 0)), transform);
        _dustEffect.gameObject.SetActive(true);
    }

    private void DestroyPlayer()
    {
        IsDeath = true;
        PoolMng.Instance.CallEvent(PoolType.ZombiePool, "ClearSpawnedEnemy");
        UIMng.Instance.Destroyer(UIList.HUD);
        UIMng.Instance.CallEvent(UIList.ActionUI, "UIClear");
        Destroy(gameObject);
    }

    private void InitComponent(CharInfo info)
    {
        UIMng.Instance.Add(UIList.HUD);
        UIMng.Instance.Add(UIList.Inventory);
        UIMng.Instance.Add(UIList.ShowItem);
        UIMng.Instance.Add(UIList.GunUpgrade);
        UIMng.Instance.Add(UIList.ActionUI);

        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");

        _inventory = FindObjectOfType<InventoryUI>();
        _playerController = GetComponent<PlayerController>();
        _gunController = GetComponent<GunController>();
        _bombController = GetComponent<BombController>();
        _status = GetComponent<PlayerStatus>();
        _animator = GetComponent<Animator>();
        _getItem = GetComponent<GetItem>();

        _status.Init(info);
        _gunController.Init(info.STARTGUN);
        _playerController.Init();
        _bombController.Init();

        UIMng.Instance.Destroyer(UIList.Select, 1.0f);
        UIMng.Instance.CallEvent(UIList.HUD, "PlayerInfoChnage");
        CheckMng.Instance.Init();
    }

    private void MoveInput()
    {
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * _moveSpeed;
        _playerController.Move(moveVelocity);

        if (moveVelocity.magnitude < 0.1f)
            _animator.SetBool("IDLE", true);
        else
            _animator.SetBool("IDLE", false);
    }

    private void LookInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.up * _gunController.GunHeight);

        if (ground.Raycast(ray, out float rayDistance))
        {
            Vector3 pos = ray.GetPoint(rayDistance);
            _playerController.LookAt(pos);

            if ((new Vector2(pos.x, pos.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
                _gunController.Aim(pos);
        }
    }

    private void Dash()
    {
        IsDash = true;

        Vector3 dashDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Ray ray = new Ray(transform.position, dashDir.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit, _dashDistance, 1 << _wallLayer | _obstacle))
        {
            Vector3 targetPos = hit.point;
            targetPos.y = transform.position.y;

            float distance = Vector3.Distance(transform.position, targetPos - dashDir * .5f);
            StartCoroutine(_playerController.IEDash(targetPos - (dashDir * .5f), _dashDistance / distance));
            //_animator.SetTrigger("DASH");
        }
        else
        {
            StartCoroutine(_playerController.IEDash(transform.position + (dashDir * _dashDistance) - (dashDir * .5f)));
            //_animator.SetTrigger("DASH");
        }
    }

    private void Bomb()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);

        if (ground.Raycast(ray, out float rayDistance))
        {
            Vector3 pos = ray.GetPoint(rayDistance);
            StartCoroutine(_bombController.IEBomb(pos));
        }
    }

    private void ShowGun()
    {
        _gunController._equipedGun.gameObject.SetActive(true);
    }

    private void Hunger()
    {
        _status.Hunger();
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        StartCoroutine(Helper.DamageBlink(transform));
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHpBar");
    }

    public void ClickBomb()
    {
        if (_inventory.CheckItemSlot("Grenade"))
        {
            IsBomb = true;

            ItemSlot grenadeSlot = _inventory.GetSlot("Grenade");
            grenadeSlot.SetSlotCount(-1);
            UIMng.Instance.CallEvent(UIList.HUD, "GrenadeCount", grenadeSlot._itemCount);
        }
    }

    private void IdlePose()
    {
        // IDLE자세로 돌아오기 위한 코드
        _idleTime += Time.deltaTime;
        if (_idleTime > _idleTargetTime)
            _animator.SetBool("SHOOTIDLE", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIMng.Instance.CallEvent(UIList.Pause, "Open");
        }
        if (!InventoryUI._inventoryActive)
        {
            LookInput();
            MoveInput();
            IdlePose();
            Hunger();
            // Shoot && Bomb
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Shoot
                if (Input.GetMouseButton(0) && !IsChangeGun && _gunController._equipedGun.CurrAmmo > 0 && !IsBomb)
                {
                    _idleTime = 0;
                    IsShoot = true;

                    switch (_gunController._equipedGun._gunType)
                    {
                        case GunType.Pistol:
                            _animator.SetInteger("GUNTYPE", 0);
                            break;
                        case GunType.ShotGun:
                            _animator.SetInteger("GUNTYPE", 1);
                            break;
                        case GunType.Auto:
                            _animator.SetInteger("GUNTYPE", 2);
                            break;
                    }
                    _animator.SetBool("SHOOTIDLE", true);
                    _animator.SetBool("SHOOTBOOL", true);
                    _gunController.OntriggerHold();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    _gunController.OnTriggerRelease();
                    _animator.SetBool("SHOOTBOOL", false);
                    IsShoot = false;
                }
                // Bomb
                if (Input.GetMouseButton(0) && !IsChangeGun && IsBomb && !IsShoot) //  && !_isShoot
                {
                    //if (_inventory.CheckItemSlot(ProjectileType.Grenade.ToString()))
                    {
                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                        Plane plane = new Plane(Vector3.up, Vector3.zero);

                        if (plane.Raycast(ray, out float distance))
                        {
                            Vector3 pos = ray.GetPoint(distance);
                            _bombController.line.enabled = true;
                            _bombController.DrawLine(pos);
                        }
                    }
                }
                if (Input.GetMouseButtonUp(0) && IsBomb)
                {
                    _gunController._equipedGun.gameObject.SetActive(false);
                    _bombController.line.enabled = false;
                    _animator.SetTrigger("GRENADE");
                    IsBomb = false;
                }
            }
            // Dash
            if (Input.GetMouseButton(1) && !IsChangeGun && !IsDash)
            {
                if (Time.time > _dashElapsedTime)
                {
                    _dashElapsedTime = Time.time + _dashTime;
                    Dash();
                }
            }
            // ChangeGun
            if (Input.GetAxis("Mouse ScrollWheel") > 0.5f && !IsChangeGun)
                _gunController.ChangeGun(1);
            if (Input.GetAxis("Mouse ScrollWheel") < -0.5f &&!IsChangeGun)
                _gunController.ChangeGun(-1);
            // Melee
            if (Input.GetKeyDown(KeyCode.Space) && !IsChangeGun)
            {
                if (Time.time > _knockElapsedTime)
                {
                    _knockElapsedTime = Time.time + _knockBackTime;
                    _playerController.KonckBack();
                }
            }
            // Change fireMode 
            if (Input.GetKeyDown(KeyCode.E))
                _gunController.ChangeFireMode();
            // Reroad 
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (ItemMng.Instance.CheckValue(_gunController._equipedGun._bulletType) > 0)
                {
                    _animator.SetTrigger("REROADTRIGGER");
                    _gunController.Reroad();
                }
            }
            // Interaction
            if (Input.GetKeyDown(KeyCode.F))
            {
                _getItem.OpenBox();
                _getItem.Get();
            }
        }
        if (Input.GetKeyDown(KeyCode.I))
            _inventory.TOpenInventory();
    }
}
