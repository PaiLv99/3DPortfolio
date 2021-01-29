using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseBuff : MonoBehaviour
{
    public Image _icon;
    public string NAME { get; set; }
    public bool IsAvtive { get; set; } = false;

    private readonly float Enable = 1.0f, Disable = 0.0f; 

    public void Init()
    {
        _icon = GetComponent<Image>();
        SetColor(Disable);
    }

    public void Activate(string path)
    {
        IsAvtive = true;
        _icon.sprite = Resources.Load<Sprite>("Image/UI/Buff" + path);
        NAME = path;
        SetColor(Enable);
    }

    public void Deactivate()
    {
        IsAvtive = false;
        _icon.sprite = null;
        SetColor(Disable);
    }

    private void SetColor(float value)
    {
        Color alpha = _icon.color;
        alpha.a = value;
        _icon.color = alpha;
    }
}
