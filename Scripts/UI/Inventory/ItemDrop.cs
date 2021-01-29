using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    private Text _input;
    private Text _preview;

    private Transform _base;
    private Player _player;

    private Button[] _button;
    private InputField _inputText;
    private Image _itemImage;
    private GunController _gunController;

    private void Awake()
    {
        _base = Helper.Find<Transform>(transform, "InputField");
        _input = Helper.Find<Text>(transform, "InputField/Input");
        _preview = Helper.Find<Text>(transform, "InputField/Preview");
        _player = FindObjectOfType<Player>();

        _button = GetComponentsInChildren<Button>();
        _button[0].onClick.AddListener(() => { OK(); });
        _button[1].onClick.AddListener(() => { Cancel(); });

        _inputText = GetComponentInChildren<InputField>();
        _itemImage = Helper.Find<Image>(transform, "ItemImage", false);

        _base.gameObject.SetActive(false);

        _gunController = FindObjectOfType<GunController>();
    }

    public void CallEvent()
    {
        if (DragSlot.instance._itemSlot._item._uniqueID == _player._gunController._equipedGun.GetComponent<Item>()._uniqueID)
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance._itemSlot = null;
            return;
        }

        _base.gameObject.SetActive(true);
        _inputText.text = "";
        _preview.text = DragSlot.instance._itemSlot._itemCount.ToString();
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = DragSlot.instance._itemSlot._item._sprite;

        DragSlot.instance.SetColor(0);
        DragSlot.instance._itemSlot = null;
    }

    public void Cancel()
    {
        _base.gameObject.SetActive(false);

        _itemImage.gameObject.SetActive(false);
        DragSlot.instance.SetColor(0);
        DragSlot.instance._itemSlot = null;
    }

    public void OK()
    {
        DragSlot.instance.SetColor(0);

        int num;
        if (_input.text != string.Empty)
        {
            if (CheckInput(_input.text))
            {
                num = int.Parse(_input.text);
                if (num > DragSlot.instance._itemSlot._itemCount)
                    num = DragSlot.instance._itemSlot._itemCount;
            }
            else
                num = 1;
        }
        else
            num = int.Parse(_preview.text);

        // object Create 
        StartCoroutine(IEDropItem(num));

        DragSlot.instance._itemSlot = null;
        _itemImage.gameObject.SetActive(false);
        _base.gameObject.SetActive(false);
    }

    private IEnumerator IEDropItem(int num)
    {
        yield return new WaitForSeconds(.1f);
        Item item = DragSlot.instance._itemSlot._item;
        switch (item._itemType)
        {
            case ItemType.Equipment: DropGun(item); break;
            case ItemType.Use: CreateItem(item._name, num); break;
            case ItemType.Projectile: break;
        }

        DragSlot.instance._itemSlot.SetSlotCount(-num);
    }

    private IEnumerator IEDropItemO(int num)
    {
        int count = 0;

        while (num < count )
        {
            yield return new WaitForSeconds(.1f);
            Item item = DragSlot.instance._itemSlot._item;
            switch (item._itemType)
            {
                case ItemType.Equipment: DropGun(item); break;
                case ItemType.Use: CreateItem(item._name); break;
                case ItemType.Projectile: break;
            }
        }
        //DragSlot.instance._itemSlot.SetSlotCount(num);
    }

    private void DropGun(Item item)
    {
        if (_gunController._gunDic.ContainsKey(item._uniqueID))
        {
            Gun dropGun = _gunController._gunDic[item._uniqueID];
            dropGun.transform.position = _player.transform.position;
            dropGun.GetComponent<Rigidbody>().AddForce((_player.transform.forward + new Vector3(0, 0.1f, 0)) * 100);

            _gunController._gunDic.Remove(item._uniqueID);
        }
    }

    private void CreateItem(string itemName, int num = 1)
    {
        if (string.IsNullOrEmpty(itemName))
            return;

        GameObject obj = PoolMng.Instance.Pop(PoolType.ItemPool, itemName);
        obj.transform.position = _player.transform.position;
        obj.transform.rotation = _player.transform.rotation;
        obj.GetComponent<Item>()._itemCount = num;
        //obj.GetComponent<Rigidbody>().AddForce((_player.transform.forward + new Vector3(0, 0.1f, 0)) * 100);
    }

    private bool CheckInput(string str)
    {
        char[] _tempArr = str.ToCharArray();

        for (int i =0; i < _tempArr.Length; i++)
        {
            if (_tempArr[i] >= 48 && _tempArr[i] <= 57)
                continue;
            return false;
        }
        return true;
    }
}