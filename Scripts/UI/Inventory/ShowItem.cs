using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShowItem : BaseUI, IBeginDragHandler, IDragHandler
{
    // UI Status 표현 
    private TextMeshProUGUI _itemName;
    private TextMeshProUGUI _itemLore;
    private TextMeshProUGUI _itemType;
    private Image _itemImage;

    private RectTransform _BG;
    private Button _button;


    public override void Init()
    {
        _button = GetComponentInChildren<Button>();
        _button.onClick.AddListener(() => { Close(); });

        _itemName = Helper.Find<TextMeshProUGUI>(transform, "BG/ItemName");
        _itemLore = Helper.Find<TextMeshProUGUI>(transform, "BG/ItemLore");
        _itemType = Helper.Find<TextMeshProUGUI>(transform, "BG/ItemType");
        _itemImage = Helper.Find<Image>(transform, "BG/ItemImage");

        _BG = Helper.Find<RectTransform>(transform, "BG");
        _BG.gameObject.SetActive(false);
    }

    private void Close()
    {
        _BG.gameObject.SetActive(false);
        _itemName.text = null;
        _itemLore.text = null;
        _itemType.text = null;
        _itemImage.sprite = null;
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");
    }

    public void Open(Item item)
    {
        if (item == null)
            return;

        _BG.gameObject.SetActive(true);
        UIMng.Instance.CallEvent(UIList.Mouse, "SetUp");

        _itemName.text = item._uiName;
        _itemLore.text = item._lore;
        _itemType.text = item._itemType.ToString();
        _itemImage.sprite = item._sprite;

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _BG.pivot = new Vector2(.5f, .95f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _BG.transform.position = eventData.position;
    }
}
