using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorM : BaseUI
{
    private List<Image> _targetImage = new List<Image>();
    private Image _cursor;
    private RectTransform _cursorRect;
    private Sprite _weaponSprite, _cursorSprite;

    private Color _originColor = new Color(1, 1, 1, .5f);
    private Color _changeColor = new Color(1, 0, 0, .7f);

    private Vector3 _pos;
    private Quaternion _weaponRotation;
    private Vector2 _weaponScale, _cursorScale;

    public override void Init()
    {
        _cursor = GetComponentInChildren<Image>();
        _cursorRect = _cursor.GetComponent<RectTransform>();
        _weaponSprite = Resources.Load<Sprite>("Image/UI/Cursor/Weapon");
        _cursorSprite = Resources.Load<Sprite>("Image/UI/Cursor/Cursor");

        _weaponRotation = Quaternion.Euler(new Vector3(45, 0, 0));
        _weaponScale = new Vector2(100, 100);
        _cursorScale = new Vector2(30, 45);
    }

    public void SetUp()
    {
        _targetImage.Clear();

        GameObject[] objects = GameObject.FindGameObjectsWithTag("UI");

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].GetComponent<Image>() != null)
                _targetImage.Add(objects[i].GetComponent<Image>());
        }
    }

    public void Update()
    {
        if (_cursor == null)
            return;

        _pos = Input.mousePosition;
        _pos.z = _cursorRect.position.z;
        // cursor move
        _cursorRect.position = _pos;

        bool _state = false;

        foreach(Image image in _targetImage)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(image.rectTransform, Input.mousePosition))
                _state = true;
        }

        if (_state)
        {
            _cursor.sprite = _cursorSprite;
            _cursorRect.pivot = new Vector2(0,1);
            _cursorRect.localRotation = Quaternion.identity;
            _cursorRect.sizeDelta = _cursorScale;
        }
        else
        {
            _cursor.sprite = _weaponSprite;
            _cursorRect.pivot = new Vector2(0.5f, .5f);
            _cursorRect.localRotation = _weaponRotation;
            _cursorRect.sizeDelta = _weaponScale;
            //IsTarget();
        }
    }


    public void IsTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, 1 << LayerMask.NameToLayer("Enemy")))
        {
            _cursorRect.Rotate(Vector3.forward * -50f * Time.deltaTime);
            _cursor.color = _changeColor;
        }
        else
        {
            _cursorRect.rotation = Quaternion.Euler(Vector3.zero);
            _cursor.color = _originColor;
        }
    }

}
