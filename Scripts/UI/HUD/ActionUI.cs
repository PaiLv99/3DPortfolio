using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class ActionUI : BaseUI
{
    private TextMeshProUGUI[] _text;
    private Image _image;
    private bool _isAction = false;

    private const int Item = 0, Gem = 1, ReloadText = 2;

    public override void Init()
    {
        _text = GetComponentsInChildren<TextMeshProUGUI>();
        _image = GetComponentInChildren<Image>();
    }

    public void ShowItemOpen(RaycastHit hit)
    {
        _text[Item].gameObject.SetActive(true);

        if (hit.transform.GetComponent<Item>() != null)
            _text[Item].text = hit.transform.GetComponent<Item>()._uiName + " GET " + "<color=#808080>(F)</color>";
    }

    public void ShowOpen()
    {
        _text[Item].gameObject.SetActive(true);
        _text[Item].text = "OPEN " + "<color=#808080>(F)</color>";
    }

    public void Disappear()
    {
        _text[Item].text = "";
        _text[Item].gameObject.SetActive(false);
    }

    public void ReloadDis()
    {
        _text[ReloadText].text = "";
        _text[ReloadText].gameObject.SetActive(false);
    }

    public void Reload()
    {
        _text[ReloadText].gameObject.SetActive(true);
        _text[ReloadText].text = "<color=white>RELOAD</color>" + "<color=#808080>(R)</color>";
    }

    public void MakeGemText()
    {
        if (! _isAction)
        {
            _text[Gem].gameObject.SetActive(true);
            _text[Gem].text = "<color=white>Make Gem!!</color>";
            StartCoroutine(IETextAnimation());
            GameObject obj = PoolMng.Instance.Pop(PoolType.EffectPool, EffectType.GemEffect.ToString());
            EffectMng.Instance.StartEffect(obj, EffectType.GemEffect.ToString() , transform.position + Vector3.up, transform.rotation);
        }
    }

    public void ItemToInventory(Item item)
    {
        _image.color = Color.white;
        _image.sprite = item._sprite;

        StartCoroutine(ToInventory(_image.transform.position, 1));
    }

    private IEnumerator ToInventory(Vector3 a, float t)
    {
        Vector3 originPos = _image.transform.position;

        Vector3 b = Vector3.up * 50f + a;
        Vector3 c = Vector3.right * 360f + b;
        Vector3 d = Vector3.down * 450f + c;

        Vector3 scale = _image.transform.localScale;
        float interporiation = 0;
        yield return null;

        while (interporiation <= t)
        {
            interporiation += Time.deltaTime * 1.5f; 
            Vector3 resurt = Helper.Bezier(a, b, c, d, interporiation);
            _image.transform.position = resurt;
            _image.transform.localScale = Vector3.Lerp(scale, new Vector3(.3f, .3f, .3f), interporiation);
            yield return null;
        }
        yield return null;

        _image.transform.localScale = scale;
        _image.transform.position = originPos;
        _image.color = Color.clear;
    }

    private IEnumerator IETextAnimation()
    {
        _isAction = true;

        Vector3 originPos = _text[1].transform.position;
        Vector3 offset = new Vector3(0, 2, 0);
        Vector3 scale = _text[Gem].transform.localScale;

        float interpoliation = 0;
        float colorInter = 0;
        
        while (interpoliation <= 1.5f)
        {
            interpoliation += Time.deltaTime;

            _text[Gem].transform.position = Vector3.Lerp(_text[Gem].transform.position, _text[Gem].transform.position +  Vector3.up * 2f, interpoliation);

            _text[Gem].transform.localScale = Vector3.Lerp(_text[Gem].transform.localScale, new Vector3(1.2f, 1.2f, 1.2f), interpoliation);

            if (interpoliation >= 1f)
            {
                colorInter += Time.deltaTime;
                _text[Gem].color = Color.Lerp(_text[Gem].color, new Color(1, 1, 1, 0), colorInter);
            }
            yield return null;
        }

        _text[Gem].transform.localScale = scale;
        _text[Gem].transform.position = originPos;
        _text[Gem].color = Color.white;
        _text[Gem].gameObject.SetActive(false);

        _isAction = false;
    }

    public void UIClear()
    {
        for (int i = 0; i < _text.Length; i++)
            _text[i].text = "";
    }
}
