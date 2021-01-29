using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    private Image _bg;
    private Button[] _buttons;

    public override void Init()
    {
        _bg = Helper.Find<Image>(transform, "BG");

        _buttons = GetComponentsInChildren<Button>();

        _buttons[0].onClick.AddListener(() => { OpenOption(); SoundMng.Instance.OncePlay("PopUp"); });
        _buttons[1].onClick.AddListener(() => { GoToLobby(); SoundMng.Instance.OncePlay("Button"); });
        _buttons[2].onClick.AddListener(() => { Continue(); SoundMng.Instance.OncePlay("Cancel"); });
        _buttons[3].onClick.AddListener(() => { EndGame(); SoundMng.Instance.OncePlay("Button"); });

        _bg.gameObject.SetActive(false);
    }

    public void Open()
    {
        _bg.gameObject.SetActive(true);
        Pause(true);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    private void OpenOption()
    {
        UIMng.Instance.CallEvent(UIList.Sound, "Open");
    }

    private void Continue()
    {
        _bg.gameObject.SetActive(false);
        Pause(false);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    private void GoToLobby()
    {
        if (FindObjectOfType<Player>())
        {
            Pause(false);
            _bg.gameObject.SetActive(false);

            UIMng.Instance.Destroyer(UIList.HUD);
            UIMng.Instance.Destroyer(UIList.Inventory);
            UIMng.Instance.Destroyer(UIList.Result);

            FindObjectOfType<Player>().IsDeath = true;
            Destroy(FindObjectOfType<Player>().gameObject);

            PoolMng.Instance.PushAll();
            TransferMapMng.Instance.RemoveAll();

            UIMng.Instance.SetActive(UIList.Loading, true);
            SceneMng.Instance.Enable(SceneType.Lobby, true);

        }
    }

    private void EndGame()
    {
        Application.Quit();
    }

    private void Pause(bool state)
    {
        if (state)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
