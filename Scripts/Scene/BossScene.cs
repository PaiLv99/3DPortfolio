using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScene : BaseScene
{
    private BossRoom _bossRoom;

    public override void Progress(float delta)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "SetProgress", delta);
    }

    public override void Enter()
    {
        UIMng.Instance.CallEvent(UIList.Loading, "StopBlick");
        UIMng.Instance.SetActive(UIList.Loading, false);
        UIMng.Instance.FadeIn(false, 1f, null);
        UIMng.Instance.CallEvent(UIList.Mouse, "Release");
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
        UIMng.Instance.SetActive(UIList.HUD, false);
        _bossRoom = FindObjectOfType<BossRoom>();
        _bossRoom.Init();

        _bossRoom.CutScene(1);
    }

    public override void Exit()
    {
        base.Exit();
    }

}
