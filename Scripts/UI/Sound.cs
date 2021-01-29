using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Sound : BaseUI
{
    private Image _bg;
    private Slider _bgmHander;
    private Slider _sfxHander;
    private Button _close;

    public override void Init()
    {
        _bg = Helper.Find<Image>(transform, "BG");
        _bgmHander = Helper.Find<Slider>(transform, "BG/BGM");
        _sfxHander = Helper.Find<Slider>(transform, "BG/SFX");
        _close = Helper.Find<Button>(transform, "BG/Image/Close");

        _close.onClick.AddListener(() => { Close(); });

        _bgmHander.value = 1.0f;
        _sfxHander.value = 1.0f;

        _bgmHander.onValueChanged.AddListener( delegate { SoundMng.Instance.SetVolumeBGM(_bgmHander.value); });
        _sfxHander.onValueChanged.AddListener(delegate { SoundMng.Instance.SetVolumeSFX(_sfxHander.value); });

        _bg.gameObject.SetActive(false);
    }

    private void Open()
    {
        _bg.gameObject.SetActive(true);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
        SoundMng.Instance.OncePlay("PopUp");
    }

    private void Close()
    {
        _bg.gameObject.SetActive(false);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
        SoundMng.Instance.OncePlay("Cancel");
    }
}
