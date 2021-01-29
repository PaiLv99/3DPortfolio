using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    private Color _originColor = new Color(1,1,1,.5f);
    private Color _changeColor = new Color(1,0,0,.7f);
    private Image _cursorImage;
    private Vector3 _prevScale;

    private void Init()
    {
        _cursorImage = GetComponent<Image>();
        _cursorImage.color = _originColor;
    }

    public void LookInput(Ray ray, float distance)
    {
        Vector3 pos = ray.GetPoint(distance);
        GetComponent<RectTransform>().anchoredPosition = Camera.main.WorldToScreenPoint(pos);
    }

    public void IsTarget(Ray ray)
    {
        if ( Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << LayerMask.NameToLayer("Enemy")) )
        {
            transform.Rotate(Vector3.forward * -50f * Time.deltaTime);
            _cursorImage.color = _changeColor;
        }
        else
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
            _cursorImage.color = _originColor;
        }
    }

    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    private void Start()
    {
        Init();
    }
}
