using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseScene : MonoBehaviour
{
    private Dictionary<Channel, SceneType> _channel = new Dictionary<Channel, SceneType>();

    protected void AddChannel(Channel channel, SceneType scene)
    {
        if (!_channel.ContainsKey(channel))
            _channel.Add(channel, scene);
    }

    public SceneType GetScene(Channel channel)
    {
        if (_channel.ContainsKey(channel))
            return _channel[channel];
        return SceneType.None;
    }

    public void FalseLoading(SceneType sceneType, float targetTime, System.Action<float> action = null)
    {
        StartCoroutine(IEFalseAsync(sceneType, targetTime, action));
    }

    public IEnumerator IEFalseAsync(SceneType scene, float targetTime = 1.0f, System.Action<float> action = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());

        bool state = false;
        float elapsedTime = 0;

        while (!state)
        {
            elapsedTime += Time.deltaTime / targetTime;
            elapsedTime = Mathf.Clamp01(elapsedTime);

            if ( elapsedTime >= 1.0f)
            {
                state = true;
                Enter();
            }
            action(elapsedTime);
            yield return null;
        }
        yield return null;
    }

    public void Load(SceneType scene, System.Action<float> action = null)
    {
        StartCoroutine(IELoad(scene, action));
    }

    public IEnumerator IELoad(SceneType scene, System.Action<float> action = null)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.ToString());

        bool state = false;

        while (!state)
        {
            if (action != null)
                action(operation.progress);
            if (operation.isDone)
            {
                state = true;
                Enter();
            }
            yield return null;
        }
        yield return null;
    }

    public virtual void Init()
    {

    }

    public void LoadScene(SceneType scene, bool falseLoading = false, float targetTime = 1.0f)
    {
        if (!falseLoading)
            Load(scene, Progress);
        else
            FalseLoading(scene, targetTime, Progress);
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Progress(float delta)
    {

    }
}


