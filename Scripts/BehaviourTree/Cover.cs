using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    //private Transform[] coverSpot;
    private List<Transform> coverSpot;

    private void Start()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        for (int i = 0; i < transforms.Length; i++)
        {
            if (name != transforms[i].name)
                coverSpot.Add(transforms[i]);
        }
    }

    public Transform[] GetCoverSpot()
    {
        return coverSpot.ToArray();
    }
}
