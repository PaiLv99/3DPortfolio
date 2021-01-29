using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cartridge : MonoBehaviour
{
    Rigidbody _rigidbody;

    private readonly float minforce = 90;
    private readonly float maxforce = 120;
    private Vector2 mmForce;

    private readonly float _lifeTime = 1;
    private readonly float _fadeTime = 1;

    void Init()
    {
        mmForce = new Vector2(minforce, maxforce);
        _rigidbody = GetComponent<Rigidbody>();
    }

    void ForceAdd()
    {
        float force = Random.Range(mmForce.x, mmForce.y);
        _rigidbody.AddForce(transform.right * force);
        _rigidbody.AddTorque(Random.insideUnitSphere * force);
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

        PoolMng.Instance.Push(PoolType.ProjectilePool, gameObject, "Cartridge");
    }

    void Start()
    {
        Init();
        ForceAdd();
        StartCoroutine(Fade()); 
    }

}
