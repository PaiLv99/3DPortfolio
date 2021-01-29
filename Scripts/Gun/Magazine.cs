using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private Vector2 mmForce = new Vector2(140, 180);

    // Cartrige lifeTime : 생성 후 대기 시간
    private float _lifeTime = 2;
    // Fade 시간 
    private float _fadeTime = 1;

    public string _name;
    public int _idx;
    public string _uiName;

    public void SetInfo(MagazineInfo info)
    {
        _name = info.NAME;
        _idx = info.IDX;
        _uiName = info.UINAME;
    }

    public void ForceAdd()
    {
        _rigidbody = GetComponent<Rigidbody>();
        float force = Random.Range(mmForce.x, mmForce.y);
        _rigidbody.AddForce(transform.right * force);
        _rigidbody.AddTorque(Random.insideUnitSphere * force * 3);
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(_lifeTime);

        float percent = 0;
        float fadeSpeed = 1 / _fadeTime;

        Material mat = GetComponent<Renderer>().material;
        Color origin = mat.color;

        while (percent < 1)
        {
            percent += Time.deltaTime * fadeSpeed;
            mat.color = Color.Lerp(origin, Color.clear, percent);
            yield return null;
        }
        PoolMng.Instance.Push(PoolType.BulletItemPool, gameObject, _name);
    }


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    internal void Func()
    {
        ForceAdd();
        StartCoroutine(Fade());
    }
}
