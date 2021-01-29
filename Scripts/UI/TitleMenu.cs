using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : BaseUI
{
    Button button;
    //private bool IsSelect { get; set; } = false;
    //private int Index { get; set; }
    //private Image _selectImage;
    //private Transform _buttonParent;
    //private Transform[] _buttonPos;


    public override void Init()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => { UIMng.Instance.SetActive(UIList.Loading, true); 
                                           SceneMng.Instance.Enable(SceneType.Lobby, true); 
                                           SoundMng.Instance.OncePlay("Button"); });

        //_buttonParent = Helper.Find<RectTransform>(transform, "Parent");
        //_buttonPos = _buttonParent.GetComponentsInChildren<RectTransform>();
        //_selectImage = Helper.Find<Image>(transform, "Select");
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.W))
    //    {
    //        if (Index <= _buttonPos.Length)
    //        {
    //            Index++;
    //            _selectImage.transform.position = _buttonPos[Index].position;
    //            IsSelect = true;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.S))
    //    {
    //        if (Index <= _buttonPos.Length)
    //        {
    //            Index--;
    //            _selectImage.transform.position = _buttonPos[Index].position;
    //            IsSelect = true;
    //        }
    //    }

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (IsSelect)
    //        {
    //            UIMng.Instance.SetActive(UIList.Loading, true);
    //            SceneMng.Instance.Enable(SceneType.Lobby, true);
    //        }
    //    }
    //}
}
