using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : BaseScene
{
    public override void Init()
    {
        AddChannel(Channel.Current, SceneType.Logo);
        AddChannel(Channel.Next, SceneType.Title);
    }

    public override void Enter()
    {
        SoundMng.Instance.OncePlay("LogoStart");
    }
}
