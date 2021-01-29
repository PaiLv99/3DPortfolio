using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void Start()
    {
        //CheckObstacle();
    }

    private void CheckObstacle()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit,  1, LayerMask.NameToLayer("Obstacle")))
            hit.transform.gameObject.SetActive(false);
        if (Physics.Raycast(transform.position, transform.right, out  hit, 1, LayerMask.NameToLayer("Obstacle")))
            hit.transform.gameObject.SetActive(false);
        if (Physics.Raycast(transform.position, -transform.right, out hit, 1, LayerMask.NameToLayer("Obstacle")))
            hit.transform.gameObject.SetActive(false);
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 1, LayerMask.NameToLayer("Obstacle")))
            hit.transform.gameObject.SetActive(false);
    }
}
