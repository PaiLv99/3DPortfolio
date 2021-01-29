using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShowGun : MonoBehaviour
{
    // 업그레이드 이미지
    public Image _selectGunImage;

    private TextMeshProUGUI _damage;
    private TextMeshProUGUI _power;
    private TextMeshProUGUI _speed;

    private TextMeshProUGUI _damageUp;
    private TextMeshProUGUI _powerUp;
    private TextMeshProUGUI _speedUp;
    private TextMeshProUGUI _itemName;

    public void Init()
    {
        _damage = Helper.Find<TextMeshProUGUI>(transform, "DamageText/Origin");
        _damageUp = Helper.Find<TextMeshProUGUI>(transform, "DamageText/Up");

        _power = Helper.Find<TextMeshProUGUI>(transform, "PowerText/Origin");
        _powerUp = Helper.Find<TextMeshProUGUI>(transform, "PowerText/Up");

        _speed = Helper.Find<TextMeshProUGUI>(transform, "SpeedText/Origin");
        _speedUp = Helper.Find<TextMeshProUGUI>(transform, "SpeedText/Up");
        _itemName = Helper.Find<TextMeshProUGUI>(transform, "ItemName");
    }

    public void Clear()
    {
        _selectGunImage.color = Color.clear;

        _damage.text = "";
        _power.text = "";
        _speed.text = "";

        _damageUp.text = "";
        _powerUp.text = "";
        _speedUp.text = "";
        _itemName.text = "";
    }

    public void DrawGunInfo(Gun gun)
    {
        GunInfo info = DataMng.Instance.Table(TableType.GunTable, gun.GetComponent<Item>()._name) as GunInfo;
        int speed = (int)(info.SHOOTSPEED * 10);
        _selectGunImage.color = Color.white;
        _selectGunImage.sprite = gun.GetComponent<Item>()._sprite;
        _damage.text = info.POWER.ToString();
        _power.text = info.DAMAGE.ToString();
        _speed.text = speed.ToString();

        int falseSpeed = (int)(gun.AddSpeed * 10);
        _damageUp.text = "(+" + gun.AddDamage.ToString() + ")";
        _powerUp.text = "(+" + gun.AddSpeed.ToString() + ")";
        _speedUp.text = "(+" + falseSpeed.ToString() + ")";

        _itemName.text = info.NAME;
    }

    public void UpdateAddGemInfo(Gun gun)
    {
        int falseSpeed = (int)(gun.AddSpeed * 10);
        _damageUp.text = "(+" + gun.AddDamage.ToString() + ")";
        _powerUp.text = "(+" + gun.AddPower.ToString() + ")";
        _speedUp.text = "(+" + falseSpeed.ToString() + ")";
    }
}
