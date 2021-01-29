using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    private void Register()
    {
        SceneMng.Instance.AddScene<Game>(SceneType.Game);
        SceneMng.Instance.AddScene<Title>(SceneType.Title);
        SceneMng.Instance.AddScene<Lobby>(SceneType.Lobby);
        SceneMng.Instance.AddScene<Logo>(SceneType.Logo);
        SceneMng.Instance.AddScene<BossScene>(SceneType.Boss);

        DataMng.Instance.AddTable<CharTable>(TableType.CharTable);
        DataMng.Instance.AddTable<GunTable>(TableType.GunTable);
        DataMng.Instance.AddTable<ZombiesTable>(TableType.ZombiesTable);
        DataMng.Instance.AddTable<ItemTable>(TableType.ItemTable);
        DataMng.Instance.AddTable<BulletTable>(TableType.BulletTable);
        DataMng.Instance.AddTable<MagazineTable>(TableType.MagazineTable);

        SoundMng.Instance.Init();
    }

    void Awake()
    {
        Cursor.visible = false;
        UIMng.Instance.FadeIn(false, 1f, null);
        Register();
        Invoke("Enter", 2);
        //SoundMng.Instance.OncePlay("LogoStart");
        StartCoroutine(IEDelay(1.0f));
    }

    void Enter()
    {
        UIMng.Instance.FadeOut(true, 1.0f, ()=> { SceneMng.Instance.Enable(SceneType.Title, true); });
    }

    private IEnumerator IEDelay(float targetTime)
    {
        yield return new WaitForSeconds(targetTime);

        SoundMng.Instance.OncePlay("LogoStart");
    }
}
