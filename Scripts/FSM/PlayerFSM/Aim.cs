using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour, IRootState
{
    private GunController _controller;
    private Player _player;

    public void Enter()
    {
        _controller = GetComponent<GunController>();
        _player = GetComponent<Player>();
        StartCoroutine(IEAimAction());
    }

    public void Execute()
    {

    }

    public void Exit()
    {
        StopCoroutine(IEAimAction());
    }

    private void LookAt(Vector3 pos)
    {
        Vector3 fixedY = new Vector3(pos.x, _player.transform.position.y, pos.z);
        transform.LookAt(fixedY);
    }

    private IEnumerator IEAimAction()
    {
        while (!_player.IsDeath)
        {
            yield return null;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.up * _controller.GunHeight);

            if (plane.Raycast(ray, out float rayDistance))
            {
                Vector3 pos = ray.GetPoint(rayDistance);
                LookAt(pos);

                if ((new Vector2(pos.x, pos.z) - new Vector2(_player.transform.position.x, _player.transform.position.z)).sqrMagnitude > 1)
                    _controller.Aim(pos);
            }
        }
    }


}
