using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public static class Helper
{
    public static T CreateObject<T>(Transform parent, bool callInit = false) where T : Component
    {
        GameObject obj = new GameObject(typeof(T).Name, typeof(T));
        obj.transform.SetParent(parent);
        T t = obj.GetComponent<T>();

        if (callInit)
            t.SendMessage("Init", SendMessageOptions.DontRequireReceiver);
        return t;
    }

    public static T Instantiate<T>(string path, Vector3 pos, Quaternion rot, bool callInit = true, Transform parent = null) where T : Component
    {
        T t = Object.Instantiate(Resources.Load<T>(path), pos, rot, parent);

        if (callInit)
            t.gameObject.SendMessage("Init", SendMessageOptions.DontRequireReceiver);

        return t;
    }

    public static T Find<T>(Transform t, string path, bool active = true) where T : Component
    {
        Transform fObj = t.Find(path);
        if (fObj != null)
        {
            fObj.gameObject.SetActive(active);
            return fObj.GetComponent<T>();
        }
        return null;
    }

    public static Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        float u = 1f - t;
        float t2 = t * t;
        float u2 = u * u;
        float u3 = u2 * u;
        float t3 = t2 * t;

        Vector3 result = (u3) * p1 + (3f * u2 * t) * p2 + (3f * u * t2) * p3 + (t3) * p4;

        return result;
    }

    public static T[] SuffleArray<T>(T[] array, int seed)
    {
        System.Random rand = new System.Random(seed);

        for (int i = 0; i < array.Length - 1; ++i)
        {
            int randomIndex = rand.Next(i, array.Length);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }

        return array;
    }

    // 가속도를 적용하는 함수
    public static float EaseOutExpo(float start, float end, float delta)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * delta / 1) + 1) + start;
    }

    public static Vector3 EaseOutExpo(Vector3 start, Vector3 end, float delta)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * delta / 1) + 1) + start;
    }

    // Text 깜빡임
    public static IEnumerator IEBlink(TextMeshProUGUI text)
    {
        float percent = 0;
        Color originColor = text.color;

        while (true)
        {
            percent += Time.deltaTime;

            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            text.color = Color.Lerp(originColor, Color.white, interpolation);

            if (percent >= 1)
                percent = 0;
            yield return null;
        }
    }

    public static float Choose(float[] prob)
    {
        float total = 0;

        foreach (float value in prob)
            total += value;

        float rand = Random.value * total;

        for (int i = 0; i < prob.Length; i++)
        {
            Debug.Log("ININ");

            if (rand < prob[i])
            {
                Debug.Log(i);
                return i;
            }
            rand -= prob[i];
        }
        return prob.Length - 1;
    }

    public static IEnumerator DamageBlink(Transform transform)
    {
        int count = 0;
        float blinkSpeed = 4;
        while (count <= 3)
        {
            float percent = 0;
            while (percent <= 1)
            {
                percent += Time.deltaTime * blinkSpeed;
                float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                transform.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.Lerp(Color.white, Color.clear, interpolation);
                yield return null;
            }
            count++;
            yield return null;
        };
    }

    public static bool IsPause()
    {
        if (System.Math.Abs(Time.timeScale) <= 0)
            return false;

        return true;
    }

}
