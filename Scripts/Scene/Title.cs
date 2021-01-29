using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : BaseScene
{
    public override void Init()
    {
        AddChannel(Channel.Current, SceneType.Title);
        AddChannel(Channel.Next, SceneType.Lobby);
    }

    public override void Exit()
    {

    }

    public override void Enter()
    {
        UIMng.Instance.FadeIn(false, 1.0f, null);
        UIMng.Instance.Add(UIList.Mouse);
        UIMng.Instance.Add(UIList.Pause);
        UIMng.Instance.Add(UIList.Sound);
        UIMng.Instance.SetActive(UIList.Title, true);
        UIMng.Instance.CallEvent(UIList.Loading, "StopText");
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
        PoolMng.Instance.Init();

        SoundMng.Instance.BgmPlay("BGM");
    }


}
