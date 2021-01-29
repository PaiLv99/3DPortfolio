using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player에서 입력받은 수치들을 실제로 동작하게 하는 스크립트
// 함수는 행동이다!!
[RequireComponent (typeof (Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    // 대시 중 무적 판정
    private Collider _collider;
    private Player _player;
    private LayerMask _layer;


    public void Move(Vector3 velocity)
    {
        _velocity = velocity;    
    }

    // 마우스방향으로 Player를 바라보게 만드는 함수!
    public void LookAt(Vector3 pos)
    {
        // Y를 고정시키기 위함
        Vector3 fixedY = new Vector3(pos.x, transform.position.y, pos.z);
        transform.LookAt(fixedY);
    }

    public IEnumerator IEDash(Vector3 targetPos, float ratio = 1.0f)
    {
        //_collider.isTrigger = true;

        Vector3 pos = transform.position;

        float dashSpeed = 3;
        float interpolation = 0;

        while (interpolation <= 1)
        {
            interpolation += Time.deltaTime * dashSpeed * ratio;
            transform.position = Vector3.Lerp(pos, targetPos, interpolation);

            yield return null;
        }
        _player.IsDash = false;
        //_collider.isTrigger = false;
    }

    public void KonckBack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2, 1 << LayerMask.NameToLayer("Enemy"));
  
        for (int i = 0; i < colliders.Length; ++i)
        {
            Vector3 dir = colliders[i].transform.position - transform.position; 
            dir = dir.normalized;

            float angle = Vector3.Angle(transform.forward, dir);
            if (angle < 45f)
                colliders[i].GetComponent<Rigidbody>().AddForce(transform.forward * 20f, ForceMode.Impulse);
        }
    }

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _rigidbody.mass = 1;
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
    }
}
