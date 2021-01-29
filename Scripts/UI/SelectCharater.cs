using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SelectCharater : BaseUI
{
    private Animator _animator;
    private Button[] _button;
    private CameraController _cameraController;
    public GameObject _selectPlayer;

    private Camera _camera;
    private Transform _cameraOriginPos;

    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _description;

    private TextMeshProUGUI _lifeText;
    private TextMeshProUGUI _speedText;
    private TextMeshProUGUI _hungryText;
    private TextMeshProUGUI _gunName;
    private TextMeshProUGUI _selectGunInfo;
    private Image _gunImage;

    private CharInfo _charInfo;
    private GunInfo _gunInfo;
    private bool _isOpen = false;

    public override void Init()
    {
        _animator = GetComponent<Animator>();

        _nameText = Helper.Find<TextMeshProUGUI>(transform, "Describe/NameText");
        _description = Helper.Find<TextMeshProUGUI>(transform, "Describe/DescribeText");
        _lifeText = Helper.Find<TextMeshProUGUI>(transform, "Status/LifeText");
        _hungryText = Helper.Find<TextMeshProUGUI>(transform, "Status/HungryText");
        _speedText = Helper.Find<TextMeshProUGUI>(transform, "Status/SpeedText");
        _gunName = Helper.Find<TextMeshProUGUI>(transform, "Status/GunName");
        _selectGunInfo = Helper.Find<TextMeshProUGUI>(transform, "Status/GunInfo");
        _gunImage = Helper.Find<Image>(transform, "Status/GunImage");

        _button = GetComponentsInChildren<Button>();

        _button[0].onClick.AddListener(() => { Exit(); });
        _button[1].onClick.AddListener(() => {  Chosen(); });

        _camera = FindObjectOfType<Camera>();
        _cameraController = FindObjectOfType<CameraController>();
        _cameraOriginPos = _camera.transform;
    }

    public void Open()
    {
        _isOpen = true;

        if (_isOpen)
        {
            _animator.SetBool("Open", true);
            DrawSelectPlayerInfo();
        }
    }

    private void DrawSelectPlayerInfo()
    {
        float shotSpeed = 0;

        _charInfo = DataMng.Instance.Table(TableType.CharTable, _selectPlayer.name) as CharInfo;
        _gunInfo = DataMng.Instance.Table(TableType.GunTable, _charInfo.STARTGUN) as GunInfo;

        _nameText.text = _charInfo.UINAME;
        _lifeText.text = _charInfo.HEALTH.ToString();
        _hungryText.text = _charInfo.HUNGRY.ToString();
        _speedText.text = _charInfo.SPEED.ToString();
        _gunName.text = _charInfo.STARTGUN;
        _gunImage.sprite = Resources.Load<Sprite>("Image/Guns/" + _charInfo.STARTGUN);
        shotSpeed = _gunInfo.SHOOTSPEED * 10;
        _selectGunInfo.text = "Damage : " + _gunInfo.DAMAGE.ToString() + "\r\n" + "Power : " + _gunInfo.POWER.ToString() + "\r\n" + "Speed : " + shotSpeed.ToString();
        _description.text = _charInfo.LORE;
    }

   
    private void Chosen()
    {
        SoundMng.Instance.OncePlay("Button");
        _cameraController._hasTarget = true;
        _selectPlayer.gameObject.AddComponent<Player>();
        ChosenPlayerSetUp();
        _cameraController.ClearCameraTransform();
        _animator.SetBool("Open", false);
        DontDestroyOnLoad(_selectPlayer.gameObject);
    }

    private void ChosenPlayerSetUp()
    {
        _selectPlayer.GetComponent<Player>().Init(_charInfo);
        ItemMng.Instance.Init();
        _cameraController.CharInit();
    }

    private void Exit()
    {
        SoundMng.Instance.OncePlay("Button");
        _cameraController.ClearCameraTransform();
        _animator.SetBool("Open", false);
    }
}
