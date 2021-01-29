using System.Collections.Generic;
using UnityEngine;

public class SoundMng : TSingleton<SoundMng>
{
    private AudioClip[] _effect;
    private AudioClip[] _bgm;

    private AudioSource _sfxPlayer;
    private AudioSource _bgmPlayer;

    private Dictionary<string, AudioClip> _soundDic;

    public float _masterVolumeBGM = 1.0f;
    public float _maseterVolumeSFX = 1.0f;

    public override void Init()
    {
        gameObject.AddComponent<AudioSource>();

        if (_sfxPlayer == null && _bgmPlayer == null)
        {
            _sfxPlayer = Helper.CreateObject<AudioSource>(transform);
            _bgmPlayer = Helper.CreateObject<AudioSource>(transform);
        }

        _soundDic = new Dictionary<string, AudioClip>();

        _effect = Resources.LoadAll<AudioClip>("Sounds/Effect");
        _bgm = Resources.LoadAll<AudioClip>("Sounds/BGM");

        AddDictionary(_effect);
        AddDictionary(_bgm);
    }

    private void AddDictionary(AudioClip[] audioClips)
    {
        for (int i = 0; i < audioClips.Length; i++)
            _soundDic.Add(audioClips[i].name, audioClips[i]);
    }

    //public void Play(string str)
    //{
    //    if (_soundDic.ContainsKey(str))
    //    {
    //        _sfxPlayer.clip = _soundDic[str];
    //        _sfxPlayer.Play();
    //    }
    //}
    
    public void OncePlay(string str, float volume = 1.0f)
    {
        if (_soundDic.ContainsKey(str))
        {
            _sfxPlayer.PlayOneShot(_soundDic[str], volume * _maseterVolumeSFX);
        }
    }

    public void BgmPlay(string str)
    {
        if (_soundDic.ContainsKey(str))
        {
            _bgmPlayer.clip = _soundDic[str];
            _bgmPlayer.volume = _masterVolumeBGM;
            _bgmPlayer.loop = true;
            _bgmPlayer.Play();
        }
    }

    public void SetBgmMasterVolume(float value)
    {
        _masterVolumeBGM = value;
    }

    // BossRoom, Intro, etc...
    public void StopBgm()
    {
        _bgmPlayer.Stop();
    }

    public void SetVolumeBGM(float value)
    {
        _bgmPlayer.volume = _masterVolumeBGM * value;
    }

    public void SetVolumeSFX(float value)
    {
        _maseterVolumeSFX = value;
    }
}
