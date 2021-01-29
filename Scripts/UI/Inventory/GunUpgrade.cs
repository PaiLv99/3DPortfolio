
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GunUpgrade : BaseUI, IBeginDragHandler, IDragHandler
{
    private Player _player;
    private Gun _selectGun;
    private RectTransform _gunInventoryBar;

    private GemSlot _damageGem;
    private GemSlot _powerGem;
    private GemSlot _speedGem;

    private Transform _damageSlotParent;
    private Transform _powerSlotParent;
    private Transform _speedSlotParent;

    private GemSlot[] _damageSockets;
    private GemSlot[] _powerSockets;
    private GemSlot[] _speedSockets;

    private Vector2 _originPos;

    private Button _buttons;
    // public 을 private으로 변경 아직 체크하지 않았다.
    private GemSlot _prevGemSlot;
    //public Gem prevGem;
    private ShowGun _showGun;

    private readonly string GemPath = "Bar/GunInventory/GemGrid/";
    private readonly string SlotPath = "Bar/GunInventory/SlotGrid/";
    private readonly string Damage = "Damage", Power = "Power", Speed = "Speed";
    
    public override void Init()
    {
        _player = FindObjectOfType<Player>();
        _showGun = FindObjectOfType<ShowGun>();
        _showGun.Init();
        _gunInventoryBar = Helper.Find<RectTransform>(transform, "Bar");

        _damageGem = Helper.Find<GemSlot>(transform, GemPath + Damage);
        _powerGem = Helper.Find<GemSlot>(transform, GemPath + Power);
        _speedGem = Helper.Find<GemSlot>(transform, GemPath + Speed);

        _damageGem.Init(GemSlot.GemSlotType.Damage);
        _powerGem.Init(GemSlot.GemSlotType.Power);
        _speedGem.Init(GemSlot.GemSlotType.Speed);

        _damageSlotParent = Helper.Find<Transform>(transform, SlotPath + Damage);
        _powerSlotParent = Helper.Find<Transform>(transform, SlotPath + Power);
        _speedSlotParent = Helper.Find<Transform>(transform, SlotPath + Speed);

        _damageSockets = new GemSlot[5];
        _powerSockets = new GemSlot[5];
        _speedSockets = new GemSlot[5];

        _damageSockets = _damageSlotParent.GetComponentsInChildren<GemSlot>();
        _powerSockets = _powerSlotParent.GetComponentsInChildren<GemSlot>();
        _speedSockets = _speedSlotParent.GetComponentsInChildren<GemSlot>();

        for (int i = 0; i < _damageSockets.Length; i++)
        {
            _damageSockets[i].Init(GemSlot.GemSlotType.Damage, true);
            _powerSockets[i].Init(GemSlot.GemSlotType.Power, true);
            _speedSockets[i].Init(GemSlot.GemSlotType.Speed, true);
        }

        _buttons = GetComponentInChildren<Button>();
        _buttons.onClick.AddListener(() => { CloseUpdate(); } );

        _originPos = _gunInventoryBar.transform.position;
        _gunInventoryBar.gameObject.SetActive(false);
    }

    private void CloseUpdate()
    {
        _gunInventoryBar.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    public void AddGem(Gem gem)
    {
        if (gem != null)
        {
            switch(gem._gemType)
            {
                case GemType.Damage: _damageGem.GemAddSlot(gem); break;
                case GemType.Power: _powerGem.GemAddSlot(gem); break;
                case GemType.Speed: _speedGem.GemAddSlot(gem); break;
            }
        }
    }

    public void DrawSocket()
    {
        if (_selectGun != null)
        {
            for (int i = 0;  i < 5; i++)
            {
                if (i < _selectGun._damageSocket.Length)
                    _damageSockets[i].gameObject.SetActive(true);
                else
                    _damageSockets[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                if (i < _selectGun._damageSocket.Length)
                    _powerSockets[i].gameObject.SetActive(true);
                else
                    _powerSockets[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < 5; i++)
            {
                if (i < _selectGun._speedSocket.Length)
                    _speedSockets[i].gameObject.SetActive(true);
                else
                    _speedSockets[i].gameObject.SetActive(false);
            }
        }
    }

    public void AddSlotCheck()
    {
        _selectGun.AddDamage = 0;
        _selectGun.AddPower = 0;
        _selectGun.AddSpeed = 0;

        for (int i = 0; i < 5; i++)
        {
            if (_damageSockets[i]._gem != null)
            {
                _selectGun._damageSocket[i] = _damageSockets[i]._gem;
                _selectGun.AddDamage += 1;
            }

            if (_powerSockets[i]._gem != null)
            {
                _selectGun._powerSocket[i] = _powerSockets[i]._gem;
                _selectGun.AddPower += 1f;
            }
            if (_speedSockets[i]._gem != null)
            {
                _selectGun._speedSocket[i] = _speedSockets[i]._gem;
                _selectGun.AddSpeed += .1f;
            }
        }
        _showGun.UpdateAddGemInfo(_selectGun);
        _selectGun.GetComponent<Item>()._equipmentUpgrade++;
    }

    public void Open(int uniqueID)
    {
        _gunInventoryBar.gameObject.SetActive(true);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");

        _selectGun = _player._gunController._gunDic[uniqueID];
        DrawSocket();
        AddSlotCheck();

        _showGun.DrawGunInfo(_selectGun);
    }

    public void SetPrevSlot(GemSlot prev)
    {
        _prevGemSlot = prev;
    }

    public void SetPrevSlotCount()
    {
        _prevGemSlot.SetSlotCount(-1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        _gunInventoryBar.pivot = new Vector2(.5f, .95f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _gunInventoryBar.transform.position = eventData.position;
    }
}
