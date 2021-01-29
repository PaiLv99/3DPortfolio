using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIMng : TSingleton<UIMng>
{
    private Dictionary<UIList, BaseUI> _uiDic;
    private readonly string path = "UI/";

    public override void Init()
    {
        _uiDic = new Dictionary<UIList, BaseUI>();
        gameObject.AddComponent<StandaloneInputModule>();
        Add(UIList.Fade, false);
        Add(UIList.Loading, false);
        Add(UIList.Title, false);
    }

    public BaseUI Add(UIList ui, bool state = true)
    {
        if (_uiDic.ContainsKey(ui))
            return _uiDic[ui];
        
        BaseUI baseUI = Helper.Instantiate<BaseUI>(path + ui.ToString(), transform.position, Quaternion.identity, true, transform);

        if (baseUI != null)
        {
            baseUI.gameObject.SetActive(state);
            _uiDic.Add(ui, baseUI);
        }
        return baseUI;
    }

    public T Get<T>(UIList ui) where T : BaseUI
    {
        if (_uiDic.ContainsKey(ui))
        {
            return _uiDic[ui].GetComponent<T>();
        }
        return null;
    }

    public void SetActive(UIList ui, bool state)
    {
        if (_uiDic.ContainsKey(ui))
            _uiDic[ui].gameObject.SetActive(state);
    }

    public void Destroyer(UIList ui, float time = 0.0f)
    {
        if (_uiDic.ContainsKey(ui))
        {
            Destroy(_uiDic[ui].gameObject);
            _uiDic.Remove(ui);
        }
    }

    public void CallEvent(UIList ui, string funcName, object obj = null)
    {
        if (_uiDic.ContainsKey(ui))
        {
            _uiDic[ui].SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void CallEvent(UIList ui, string funcName, Sprite[] images)
    {
        if (_uiDic.ContainsKey(ui))
        {
            _uiDic[ui].SendMessage(funcName, images, SendMessageOptions.DontRequireReceiver);
        }
    }

    #region Fade
    public void FadeIn(bool state, float targetTime, System.Action action)
    {
        Fade fadeIn = Get<Fade>(UIList.Fade);

        if (fadeIn != null)
            fadeIn.FadeIn(state, targetTime, action);
    }

    public void FadeOut(bool state, float targetTime, System.Action action)
    {
        Fade fadeOut = Get<Fade>(UIList.Fade);
        if (fadeOut != null)
        {
            fadeOut.FadeOut(state, targetTime, action);
        }
    }
    #endregion
}









public enum UIList { Fade, Loading, HUD, Inventory, GunUpgrade, Title, Result, Mouse, Select, ActionUI, ShowItem, Sound, Pause, GameOver, }