using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour, IState
{
    private PlayerController _controller;
    private Collider _collider;

    private readonly float _dashDistance = 5;
    private float _elapsedTime = 3;

    public void Action()
    {
        DashAction();
    }

    public void Init()
    {
        _controller = GetComponent<PlayerController>();
        _collider = GetComponent<Collider>();
    }

    // dashDir * .5f : 콜라이더 겹치지 않게 하기 위한 변수
    // _dashDistance / distance : distance는 무조건 _dashDistance보다 작다 따라서 나누면 radio값이 커진다. 따라서 간극이 높아지게 된면 좀더 빠른 시간에 줄어들게 된다. 
    private void DashAction()
    {
        if (Time.time >= _elapsedTime)
        {
            _elapsedTime += Time.time;

            Vector3 dashDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Ray ray = new Ray(transform.position, dashDir.normalized);

            if (Physics.Raycast(ray, out RaycastHit hit, _dashDistance, 1 << 11))
            {
                Vector3 targetPos = hit.point;
                targetPos.y = transform.position.y;

                float distance = Vector3.Distance(transform.position, targetPos - dashDir * .5f);
                //StartCoroutine(_controller.IEDash(targetPos - (dashDir * .5f), _dashDistance / distance));
                StartCoroutine(IEDash((targetPos) - (dashDir * .5f), _dashDistance / distance));
                //_animator.SetTrigger("DASH");
            }
            else
            {
                //StartCoroutine(_controller.IEDash(transform.position + (dashDir * _dashDistance) - (dashDir * .5f), 1f));
                StartCoroutine(IEDash(transform.position + (dashDir * _dashDistance) - (dashDir * .5f)));
                //_animator.SetTrigger("DASH");
            }
            //_status._stamina -= value;
        }
    }

    private IEnumerator IEDash(Vector3 targetPos, float ratio = 1.0f)
    {
        _collider.isTrigger = true;

        float dashSpeed = 3;
        float interpolation = 0;

        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * dashSpeed * ratio;
            transform.position = Vector3.Lerp(transform.position, targetPos, interpolation);

            yield return null;
        }
        _collider.isTrigger = false;
    }

    public void Enter()
    {
        throw new System.NotImplementedException();
    }

    public void Execute()
    {
        throw new System.NotImplementedException();
    }

    public void Exit()
    {
        throw new System.NotImplementedException();
    }
}
