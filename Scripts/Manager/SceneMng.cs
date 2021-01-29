using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneType { None, Logo, Title, Lobby, Game, Boss }
public enum Channel { Current, Next }

public class SceneMng : TSingleton<SceneMng>
{
    public Dictionary<SceneType, BaseScene> _sceneDic = new Dictionary<SceneType, BaseScene>();
    private SceneType _currScene = SceneType.Logo;

    public T AddScene<T>(SceneType sType, bool state = false) where T : BaseScene
    {
        if (!_sceneDic.ContainsKey(sType))
        {
            T t = Helper.CreateObject<T>(transform, true);
            t.enabled = state;
            _sceneDic.Add(sType, t);
            return t;
        }
        return null;
    }

    public void Event(Channel channel, bool falseLoading = false)
    {
        if (_sceneDic.ContainsKey(_currScene))
        {
            SceneType scene = _sceneDic[_currScene].GetScene(channel);
            Enable(scene, falseLoading);
        }
    }

    public void Enable(SceneType scene, bool falseLoading = false)
    {
        if (_sceneDic.ContainsKey(_currScene))
            _sceneDic[_currScene].Exit();

        if (_sceneDic.ContainsKey( scene))
        {
            _currScene = scene;
            _sceneDic[_currScene].LoadScene(scene, falseLoading);
        }
    }
}



