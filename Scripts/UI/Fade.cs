using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : BaseUI
{
    private Image _image;
    private bool _state = false;

    public override void Init()
    {
        _image = Helper.Find<Image>(transform, "image", true );
    }

    private IEnumerator IEFade(Color start, Color end, bool state, float targetTime, System.Action action = null)
    {
        float interpoliate = 0;
        
        while (_state)
        {
            interpoliate += Time.deltaTime / targetTime;
            Mathf.Clamp01(interpoliate);
            _image.color = Color.Lerp(start, end, interpoliate);   

            if (interpoliate >= 1)
            {
                _state = false;
                _image.raycastTarget = false;
                gameObject.SetActive(state);
                if (action != null)
                    action();
            }
            yield return null;
        }
    }

    private void StartFade(Color start, Color end, bool state, float targetTime, System.Action action = null)
    {
        if (_state)
            return;

        gameObject.SetActive(true);

        _state = true;
        StartCoroutine(IEFade(start, end, state, targetTime, action));
    }

    public void FadeIn(bool state, float targetTime, System.Action action = null)
    {
        Color start = Color.black;
        Color end = Color.black;
        end.a = 0;
        StartFade(start, end, state, targetTime, action);
    }

    public void FadeOut(bool state, float targetTime, System.Action action = null)
    {
        Color start = Color.black;
        Color end = Color.black;
        start.a = 0;
        StartFade(start, end, state, targetTime, action);
    }
}
