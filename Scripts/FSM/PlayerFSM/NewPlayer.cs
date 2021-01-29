using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : BaseChar
{
    private RootFSM _state;
    private PlayerStatus _status;

    private PlayerController _controller;
    private BombController _bombController;
    private GetItem _getItem;
    private InventoryUI _inventory;
    public GunController _gunController;
    private ParticleSystem _dustEffect;

    public float _moveSpeed = 5;
    private readonly float _dashTime = 1.5f;
    private readonly float _dashDistance = 4;
    private readonly float _knockBackTime = 2;
    private float _dashElapsedTime;
    private float _knockElapsedTime;

    private bool IsDeath { get; set; } = false;


    public void Init(CharInfo info)
    {
        _state = GetComponent<RootFSM>();
        _state.Init();

        _status = GetComponent<PlayerStatus>();
        _status.Init(info);

        _dustEffect = Instantiate(Resources.Load<ParticleSystem>("Effect/DustEffect"), transform.position, Quaternion.Euler(new Vector3(-5, 180, 0)), transform);
        _dustEffect.gameObject.SetActive(true);
    }

    public void CharInit(CharInfo info)
    {


    }

    private void DestroyPlayer()
    {
        IsDeath = true;
        PoolMng.Instance.CallEvent(PoolType.ZombiePool, "ClearSpawnedEnemy");
        UIMng.Instance.Destroyer(UIList.HUD);
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
      
        _status = GetComponent<PlayerStatus>();
        _status.Init(info);
        _gunController.Init(info.STARTGUN);

        UIMng.Instance.Destroyer(UIList.Select, 1.0f);
        UIMng.Instance.CallEvent(UIList.HUD, "PlayerInfoChnage");
        CheckMng.Instance.Init();
    }
}
