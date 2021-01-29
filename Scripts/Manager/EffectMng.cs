using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectMng : TSingleton<EffectMng>
{
    public void StartEffect(GameObject obj, string str, Vector3 position, Quaternion rotation, Transform parant = null, object value = null)
    {
        obj.SetActive(true);

        obj.transform.parent = parant;
        obj.transform.position = position;
        obj.transform.rotation = rotation;

        if (str == EffectType.DamagePopUp.ToString())
        {
            obj.GetComponent<TextMeshPro>().text = value.ToString();
            obj.GetComponent<DamagePopUp>().Play();
            StartCoroutine(IEPush(obj, str, 1f));
        }
        else if (str != EffectType.DustEffect.ToString())
        {
            obj.GetComponent<ParticleSystem>().Play();
            StartCoroutine(IEPush(obj, str, obj.GetComponent<ParticleSystem>().main.startLifetimeMultiplier));
        }
    }

    public IEnumerator IEPush(GameObject obj, string str, float time)
    {
        yield return new WaitForSeconds(time);
        if (obj != null)
            PoolMng.Instance.Push(PoolType.EffectPool, obj, str);
    }
}
