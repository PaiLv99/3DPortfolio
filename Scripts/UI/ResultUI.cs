using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultUI : BaseUI
{
    // 내부 클래스 생성하지 않고 사용하기 위해 static 함수로 만듬 
    public static class StringType
    {
        public static string Time(float value)
        {
            int min = (int)value / 60;
            int sec = (int)value % 60;
            return string.Format("{0:D2}:{1:D2}", min, sec);
        }

        public static string KillCount(float value)
        {
            value = Mathf.Round(value);
            return value.ToString();
        }
    }

    //private Image _gameOver;
    //private Image _gameOverBG;
    //private TextMeshProUGUI _aniKeyText;

    private Animator _animator;
    private Button _button;
    private Image _uiImage;
    private TextMeshProUGUI _killCountText;
    private TextMeshProUGUI _gameTimeText;
    private TextMeshProUGUI _gameResult;
    private Slider _floorSlider;

    private int _killCount;
    private float _gameTime;
    private int _floorCount;
    public int FloorCount { get { return _floorCount; } set { _floorCount += value; } }

    public override void Init()
    {
        _animator = GetComponent<Animator>();
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(() => { Restart(); });

        _uiImage = Helper.Find<Image>(transform, "ResultUI", false);
        _killCountText = Helper.Find<TextMeshProUGUI>(transform, "ResultUI/Kill/KillCountText");
        _gameTimeText = Helper.Find<TextMeshProUGUI>(transform, "ResultUI/Time/GameTimeText");
        _gameResult = Helper.Find<TextMeshProUGUI>(transform, "ResultUI/GameEnd/Text");
        _floorSlider = Helper.Find<Slider>(transform, "ResultUI/Floor/Slider");
    }

    private void Restart()
    {
        PoolMng.Instance.PushAll();

        UIMng.Instance.Destroyer(UIList.HUD);
        UIMng.Instance.Destroyer(UIList.Inventory);
        UIMng.Instance.Destroyer(UIList.Result);
        UIMng.Instance.SetActive(UIList.Loading, true);
        TransferMapMng.Instance.RemoveAll();
        SceneMng.Instance.Event(Channel.Next);
        //SceneMng.Instance.Enable(SceneType.Lobby, true);
    }

    private void Open()
    {
        _uiImage.gameObject.SetActive(true);
        _animator.SetTrigger("RESULT");
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    private void IsGameover(bool value)
    {
        if (value)
            _gameResult.text = "Game Over...";
        else 
            _gameResult.text = "Escape";
    }

    private IEnumerator IEProgressBar()
    {
        float elapsedTime = 0;
        elapsedTime += Time.deltaTime;
        if (elapsedTime < 1)
        {
            _floorSlider.value = Mathf.Lerp(0f, FloorCount, elapsedTime);
            yield return null;
        }
    }

    public void AddMapCount()
    {
        FloorCount += 1;
    }

    // 결과창에 텍스트 애니메이션 기능 함수 
    public void TextAnimation(int num)
    {
        switch (num)
        {
            case 0: StartCoroutine(IEProgressBar()); break;
            case 1: StartCoroutine(IETextAnimation(_gameTimeText, _gameTime, num)); break;
            case 2: StartCoroutine(IETextAnimation(_killCountText, _killCount, num)); break;
        }
    }

    public IEnumerator IETextAnimation(TextMeshProUGUI text, float value, int num)
    {
        float targetTime = 1;
        float interpolation = 0;
        float currvalue = 0;

        while (interpolation < targetTime)
        {
            interpolation += Time.deltaTime;
            currvalue = Mathf.Lerp(currvalue, value, interpolation);

            if (num == 1)
                text.text = StringType.Time(currvalue);
            else if (num == 2)
                text.text = StringType.KillCount(currvalue);

            yield return null;
        }
        interpolation = 0;
    }

    // 에너미 죽을 때 마다 호출
    public void KillCount()
    {
        ++_killCount;
    }

    private void Update()
    {
        _gameTime += Time.deltaTime;
    }
}
