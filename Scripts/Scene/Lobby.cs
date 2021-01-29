using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : BaseScene
{
    public override void Init()
    {
        AddChannel(Channel.Current, SceneType.Lobby);
        AddChannel(Channel.Next, SceneType.Game);
    }

    public override void Progress(float delta)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "SetProgress", delta);
    }

    public override void Enter()
    {
        UIMng.Instance.CallEvent(UIList.Loading, "StopBlink");
        UIMng.Instance.SetActive(UIList.Loading, false);
        UIMng.Instance.FadeIn(false, .5f, null);
        UIMng.Instance.Add(UIList.Select);
        UIMng.Instance.SetActive(UIList.Title, false);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");

        FindObjectOfType<CameraController>().Init();
    }
}
