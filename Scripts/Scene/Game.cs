using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : BaseScene
{
    //private Player _player;
    private CameraController _cameraController;

    public override void Init()
    {
        AddChannel(Channel.Current, SceneType.Game);
        AddChannel(Channel.Next, SceneType.Lobby);
    }

    public override void Progress(float delta)
    {
        UIMng.Instance.CallEvent(UIList.Loading, "SetProgress", delta);
    }

    public override void Enter()
    {
        #region UI Process
        UIMng.Instance.CallEvent(UIList.Loading, "StopText");
        UIMng.Instance.SetActive(UIList.Loading, false);
        UIMng.Instance.Add(UIList.Result);
        UIMng.Instance.Add(UIList.GameOver, false);
        UIMng.Instance.FadeIn(true, .5f, null);
        UIMng.Instance.CallEvent(UIList.Mouse, "Release");
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
        #endregion

        #region Map Process
        //LevelMng.Instance.Init();
        LevelMng.Instance.ReInit();
        LevelMng.Instance.Generator();
        TransferMapMng.Instance.SetPoint();
        TransferMapMng.Instance.SetMaps();
        TransferMapMng.Instance.ActiveMap();
        SpawnMng.Instance.DoorInit();
        #endregion

        //_player = FindObjectOfType<Player>();
        _cameraController = FindObjectOfType<CameraController>();
        //_player.Init();
        _cameraController.CharInit();
        _cameraController._hasTarget = true;

        FindObjectOfType<Player>().transform.position = TransferMapMng.Instance.GetStartPosition(0) + new Vector3(1.5f,0,.5f);
        TransferMapMng.Instance.StartPointSet(FindObjectOfType<Player>().GetComponent<Collider>());
        TransferMapMng.Instance.RemoveStartPoint(0);
    }

}
