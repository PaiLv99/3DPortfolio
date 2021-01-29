using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverUI : BaseUI
{
    private Image _gameOver;
    private Image _BG;
    private TextMeshProUGUI _anikey;

    public override void Init()
    {
        _gameOver = Helper.Find<Image>(transform, "Image");
        _BG = Helper.Find<Image>(transform, "BG");
        _anikey = Helper.Find<TextMeshProUGUI>(transform, "Anikey");
        FindObjectOfType<Player>().OnDeath += GameOver;
    }

    public void GameOver()
    {
        gameObject.SetActive(true);
        //StartCoroutine(IEColor(.5f));
        //StartCoroutine(IEInputAnikey());
        StartCoroutine(Helper.IEBlink(_anikey));
        UIMng.Instance.CallEvent(UIList.Result, "IsGameover", true);
    }

    private IEnumerator IEColor(float targetTime)
    {
        float time = 0;

        while (time <= targetTime)
        {
            time += Time.deltaTime;
            _gameOver.color = Color.Lerp(_gameOver.color, Color.white, time);

            yield return null;
        }
    }

    private IEnumerator IEInputAnikey()
    {
        float percent = 0;
        Color origin = _anikey.color;

        while (true)
        {
            percent += Time.deltaTime;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            _anikey.color = Color.Lerp(origin, Color.white, interpolation);

            if (percent >= 1)
                percent = 0;
            yield return null;
        }
    }

    private void Clear()
    {
        StopCoroutine(IEInputAnikey());
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            Clear();
            UIMng.Instance.CallEvent(UIList.Result, "Open");
        }
    }
}
