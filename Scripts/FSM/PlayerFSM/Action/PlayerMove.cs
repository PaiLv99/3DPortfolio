using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IState
{
    //private PlayerController _controller;
    private Animator _animator;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;

    public float _moveSpeed = 5;

    public void Init()
    {
        //_controller = GetComponent<PlayerController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = 1;
        _animator = GetComponent<Animator>();
    }

    public void Action()
    {
        MoveAction();
    }

    private void MoveAction()
    {
        // enter
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        Vector3 moveVelocity = moveInput.normalized * _moveSpeed;

        // Process
        _velocity = moveVelocity;
        //_controller.Move(moveVelocity);

        //Exit
        if (moveVelocity.magnitude < 0.1f)
            _animator.SetBool("IDLE", true);
        else
            _animator.SetBool("IDLE", false);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
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
