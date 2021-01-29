using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public GameObject _muzzleFlashHolder;
    private float _flashTime = .05f;
    public Sprite[] _sprites;
    public SpriteRenderer[] _spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        DeActivate();
    }

    public void Activate()
    {
        _muzzleFlashHolder.SetActive(true);

        int rand = Random.Range(0, _sprites.Length);
        for (int i = 0; i < _spriteRenderer.Length; ++i)
        {
            _spriteRenderer[i].sprite = _sprites[rand];
        }

        Invoke("DeActivate", _flashTime);
    }

    public void DeActivate()
    {
        _muzzleFlashHolder.SetActive(false);
    }
}
