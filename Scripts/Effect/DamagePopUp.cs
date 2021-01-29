using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Play()
    {
        _animator.SetTrigger("START");
        StartCoroutine(IEBezier(transform.position, 1));
    }

    private IEnumerator IEBezier(Vector3 a, float t)
    {
        Vector3 b = Vector3.up * 2f + a;
        Vector3 c = (Vector3.right * 2f) + (Vector3.forward * 2f) + b;
        Vector3 d = Vector3.down * 2f + c;

        float interporiation = 0;
        yield return null;

        while (interporiation <= t)
        {
            interporiation += Time.deltaTime;

            Vector3 resurt = Helper.Bezier(a, b, c, d, interporiation);

            transform.position = resurt;

            yield return null;
        }
    }
}
