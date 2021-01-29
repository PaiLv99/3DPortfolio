using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GemType { Damage, Power, Speed }

public class Gem
{
    public Sprite _gemImage;
    public string _gemName;
    public GemType _gemType;

    public Gem(Sprite image, string str, GemType type)
    {
        _gemImage = image;
        _gemName = str;
        _gemType = type;
    }
}
