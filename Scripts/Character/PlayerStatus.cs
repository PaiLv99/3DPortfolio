using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private Player _player;

    private Sprite _powerGearSprite, _damageGearSprite, _speedGearSprtie;
    private Light _light;

    // selectPlayer에서 변경할 변수들! 
    public float _maxHp=0;
    public float _speed;
    public float _maxHungry;
    public float _stamina;

    public int _currentExp = 0;
    public int _maxExp = 5;

    public float _currentHungry;
    private float _hungryElapsedTime;
    private bool _isSpeedUp;
    private readonly float _hungryTargetTime = 100;

    public void MakeGem()
    {
        if (_currentExp == _maxExp)
        {
            _currentExp = 0;
            _maxExp += 5;

            int rand = Random.Range(0, 3);

            switch (rand)
            {
                case 0:
                    Gem powerGem = new Gem(_powerGearSprite, "PowerGem", GemType.Power);
                    UIMng.Instance.CallEvent(UIList.GunUpgrade, "AddGem", powerGem);
                    break;
                case 1:
                    Gem damageGem = new Gem(_damageGearSprite, "DamageGem", GemType.Damage);
                    UIMng.Instance.CallEvent(UIList.GunUpgrade, "AddGem", damageGem);
                    break;
                case 2:
                    Gem speedGem = new Gem(_speedGearSprtie, "SpeedGem", GemType.Speed);
                    UIMng.Instance.CallEvent(UIList.GunUpgrade, "AddGem", speedGem);
                    break;
            }
            UIMng.Instance.CallEvent(UIList.HUD, "UpdateGemBar");
            UIMng.Instance.CallEvent(UIList.ActionUI, "MakeGemText");
            SoundMng.Instance.OncePlay("Gear");
        }
    }

    public void Hunger()
    {
        _hungryElapsedTime += Time.deltaTime;

        if (_currentHungry > 0)
        {
            if (_hungryElapsedTime > _hungryTargetTime)
            {
                _hungryElapsedTime = 0;
                --_currentHungry;
                UIMng.Instance.CallEvent(UIList.HUD, "UpdateHungryBar");
            }
        }
        else
        {
            if (_hungryElapsedTime > _hungryTargetTime)
            {
                _hungryElapsedTime = 0;
                HpUpdate(-1);

                if (_player._hp <= 0)
                    _player.Die();
            }
        }
    }

    public void SpeedUp(float count)
    {
        if (!_isSpeedUp)
        {
            _isSpeedUp = true;
            _player._moveSpeed += count;
            StartCoroutine(IESpeedUp(20f));
            UIMng.Instance.CallEvent(UIList.HUD, "BuffOn", "SpeedUp");
  
            //GameObject effcect = PoolMng.Instance.Pop(PoolType.EffectPool, EffectType.SpeedUp.ToString());
            //EffectMng.Instance.StartEffect(effcect, "SpeedUp", _player.transform.position, Quaternion.identity);
        }
    }

    private IEnumerator IESpeedUp(float targetTime)
    {
        yield return new WaitForSeconds(targetTime);
        _player._moveSpeed = _speed;
        UIMng.Instance.CallEvent(UIList.HUD, "BuffOff", "SpeedUp");
        _isSpeedUp = false;
    }

    public int AddExp(int count)
    {
        _currentExp += count;
        GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool, EffectType.GemEffect.ToString());
        EffectMng.Instance.StartEffect(obj, EffectType.GemEffect.ToString(), transform.position, transform.rotation);

        return _currentExp;
    }

    public void Food(float count)
    {
        if (_currentHungry >= _maxHungry)
            return;

        _currentHungry += count;
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHungryBar");
    }

    public void HpUpdate(float count)
    {
        if (_player._hp >= _maxHp)
            return;

        _player._hp += count;
        UIMng.Instance.CallEvent(UIList.HUD, "UpdateHpBar");
    }

    public void HandLight(float count)
    {
        //_light.gameObject.SetActive(true);
        //StartCoroutine(IEHandLight(count));
    }

    private IEnumerator IEHandLight(float count)
    {
        yield return new WaitForSeconds(count);
        _light.gameObject.SetActive(false);
    }

    public void Init(CharInfo info)
    {
        _maxHp = info.HEALTH;
        _maxHungry = info.HUNGRY;
        _speed = info.SPEED;

        _player = GetComponent<Player>();

        _player._hp = _maxHp;
        _currentHungry = _maxHungry;
        _player._moveSpeed = _speed;

        _light = FindObjectOfType<Light>();

        _powerGearSprite = Resources.Load<Sprite>("Image/Gear/PowerGear");
        _damageGearSprite = Resources.Load<Sprite>("Image/Gear/DamageGear");
        _speedGearSprtie = Resources.Load<Sprite>("Image/Gear/SpeedGear");
    }
}
