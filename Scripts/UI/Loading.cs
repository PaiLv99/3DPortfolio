using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Loading : BaseUI
{
    private Image _loadingBar;
    private TextMeshProUGUI _loadingText;

    private bool _isBlick = false;

    private float _mapProgress;
    private float _sceneProgress;

    public override void Init()
    {
        _loadingBar = Helper.Find<Image>(transform, "LoadingBar/LoadingValue");
        _loadingText = Helper.Find<TextMeshProUGUI>(transform, "LoadingBar/LoadingText");
    }

    public void SetProgress(float delta)
    {
        delta = Mathf.Round(delta * 25) / 25;
        _loadingBar.fillAmount = delta;

        if (!_isBlick)
        {
            _isBlick = true;
            StartCoroutine(IETextBlink(_loadingText));
        }
    }

    private void GameProgress(float delta)
    {
        delta = Mathf.Round(delta * 25) / 25;
        _sceneProgress = delta / 2;

        if (!_isBlick)
        {
            _isBlick = true;
            StartCoroutine(IETextBlink(_loadingText));
            StartCoroutine(IEMapProgress());
        }
    }

    public IEnumerator IETextBlink(TextMeshProUGUI text)
    {
        float percent = 0;
        Color originColor = text.color;

        while (_isBlick)
        {
            percent += Time.deltaTime * 1.2f;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            text.color = Color.Lerp(originColor, Color.white, interpolation);

            if (percent >= 1)
                percent = 0;
            yield return null;
        }
    }

    private void StopText()
    {
        _isBlick = false;
    }

    private IEnumerator IEMapProgress()
    {
        while (!LevelMng.Instance.IsDone)
        {
            _mapProgress = LevelMng.Instance.Progress;

            float totalProgress = (_mapProgress + _sceneProgress) / 2.0f;
            _loadingBar.fillAmount = totalProgress;
            yield return null;
        }

        _isBlick = false;
        UIMng.Instance.FadeIn(false, 0.5f, null);
        gameObject.SetActive(false);
    }

}


