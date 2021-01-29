using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : BaseUI
{
    private Image _hp, _hungry, _gear;
    private TextMeshProUGUI _hpText, _hungryText, _gearText;
    private Image _gunImage;
    private TextMeshProUGUI _remainAmmoText, _grenadeText;
    private Image _remainAmmoImage;
    private Image _remainAmmoBG;
    private Slider _reload;

    private Transform _buffParent;
    private BaseBuff[] _buffs;

    private Player _player;
    private Button _inventoryButton;
    private Button _pause;
    private Button _grenade;

    public override void Init()
    {
        _player = FindObjectOfType<Player>();
        PlayerStatusInfo();
        BuffInit();
        GunInit();

        _reload = Helper.Find<Slider>(transform, "Reload", false);
        _inventoryButton = Helper.Find<Button>(transform, "InventoryKick");
        _inventoryButton.onClick.AddListener(() => { UIMng.Instance.CallEvent(UIList.Inventory, "TOpenInventory"); });
        _pause = Helper.Find<Button>(transform, "Pause");
        _pause.onClick.AddListener(() => UIMng.Instance.CallEvent(UIList.Pause, "Open"));
        _grenade = Helper.Find<Button>(transform, "Grenade");
        _grenadeText = Helper.Find<TextMeshProUGUI>(transform, "Grenade/Count");
        _grenade.onClick.AddListener(() => { _player.ClickBomb(); });
    }

    private void PlayerInfoChnage()
    {
        UpdateHpBar();
        UpdateGemBar();
        UpdateHungryBar();
    }

    private void GunInit()
    {
        _gunImage = Helper.Find<Image>(transform, "GunSlot/Gun");
        _remainAmmoText = Helper.Find<TextMeshProUGUI>(transform, "GunSlot/RemainAmmoText");
        _remainAmmoImage = Helper.Find<Image>(transform, "GunSlot/RemainAmmoImage");
        _remainAmmoBG = Helper.Find<Image>(transform, "GunSlot/RemainAmmoBG");
    }

    private void BuffInit()
    {
        _buffParent = Helper.Find<Transform>(transform, "BuffParent");
        _buffs = _buffParent.GetComponentsInChildren<BaseBuff>();

        for (int i = 0; i < _buffs.Length; i++)
            _buffs[i].Init();
    }

    private void PlayerStatusInfo()
    {
        _hp = Helper.Find<Image>(transform, "HP");
        _hpText = Helper.Find<TextMeshProUGUI>(transform, "HP/Text");
        _hungry = Helper.Find<Image>(transform, "Hungry");
        _hungryText = Helper.Find<TextMeshProUGUI>(transform, "Hungry/Text");
        _gear = Helper.Find<Image>(transform, "Gear");
        _gearText = Helper.Find<TextMeshProUGUI>(transform, "Gear/Text");
    }

    public void UpdateHpBar()
    {
        _hp.fillAmount = _player._hp / _player._status._maxHp;
        _hpText.text = _player._hp.ToString() + "/" + _player._status._maxHp;
    }

    public void UpdateHungryBar()
    {
        _hungry.fillAmount = _player._status._currentHungry / _player._status._maxHungry;
        _hungryText.text = _player._status._currentHungry.ToString() + "/" + _player._status._maxHungry;
    }

    public void UpdateGemBar()
    {
        _gear.fillAmount = _player._status._currentExp / (float)_player._status._maxExp;
        _gearText.text = _player._status._currentExp.ToString() + "/" + _player._status._maxExp.ToString();
    }

    public void UpdateGunImage(Sprite gunImage)
    {
        if (gunImage != null)
            _gunImage.sprite = gunImage;
    }

    public void UpdateRemainAmmo(BulletType type)
    {
        if (_player._gunController._equipedGun != null)
            _remainAmmoText.text = _player._gunController._equipedGun.CurrAmmo.ToString() + "/" + ItemMng.Instance.CheckValue(type).ToString();
    }

    public void UpdateRemainAmmoImage(Sprite[] remainAmmos)
    {
        if (remainAmmos != null)
        {
            _remainAmmoImage.sprite = remainAmmos[0];
            _remainAmmoBG.sprite = remainAmmos[1];

            _remainAmmoImage.rectTransform.sizeDelta = new Vector2(remainAmmos[0].rect.width, remainAmmos[0].rect.height);
            _remainAmmoBG.rectTransform.sizeDelta = new Vector2(remainAmmos[1].rect.width, remainAmmos[1].rect.height);

            _remainAmmoImage.fillAmount = _player._gunController._equipedGun.CurrAmmo / (float)_player._gunController._equipedGun.ReloadAmmo;
        }
    }

    public void UpdateReload(float delta)
    {
        _reload.value = delta;
    }

    public void SetReload(bool state)
    {
        _reload.gameObject.SetActive(state);
    }

    public void BuffOn(string path)
    {
        for (int i = 0; i < _buffs.Length; i++)
        {
            if (!_buffs[i].IsAvtive)
            {
                _buffs[i].Activate(path);
                return;
            }
        }
    }

    public void BuffOff(string str)
    {
        for (int i = 0; i < _buffs.Length; i++)
            if (_buffs[i].IsAvtive && _buffs[i].NAME == str)
                _buffs[i].Deactivate();
    }

    public void GrenadeCount(int count)
    {
        _grenadeText.text = count.ToString();
    }
}
