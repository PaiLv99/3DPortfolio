using UnityEngine;

public enum ItemType { Use, Equipment, Projectile, Itembox } // ... more

public class Item : MonoBehaviour
{
    public string _name, _uiName, _lore;
    public string _prefabPath, _imagePath;
    public string[] _parts;
    public float[] _values;
    public ItemType _itemType;
    public Sprite _sprite;
    public int _idx;
    public int _uniqueID;
    public int _itemCount = 1;
    public int _equipmentUpgrade = 0;

    public void SetItem(ItemInfo info)
    {
        _idx = info.IDX;
        _name = info.NAME;
        _uiName = info.UINAME;
        _prefabPath = info.PREFABPATH;
        _imagePath = info.IMAGEPATH;
        _values = info.ITEMVALUES;
        _parts = info.ITEMPARTS;
        _lore = info.LORE;
        _itemType = info.ITEMTYPE;
        _sprite = Resources.Load<Sprite>(_imagePath);
        _uniqueID = GetInstanceID();
    }
}
