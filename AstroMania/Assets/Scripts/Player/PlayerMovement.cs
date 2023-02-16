using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _walkSpeed;

    [Header("GroundCheck")]
    [SerializeField] private float _playerHeight;
    public bool isGrounded;

    [Header("Jump")]
    [SerializeField] private float _jumpSpeed;

    [Header("AirSpeed")]
    [SerializeField] private float _airSpeed;

    private float _speed;
    private Vector3 _move;
    private Rigidbody _rb;
    private Vector3 _moveDirection;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _movement;
    [SerializeField] private InputActionReference _jump;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    private Vector3 _direction;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        InputMove();
        Jump();

        ManageAnimation();
    }
    private void FixedUpdate()
    {
        FindDirection();
        Move();
    }

    /// <summary>
    /// Input für das Movement in der Update Methode
    /// </summary>
    private void InputMove()
    {
        _move = _movement.action.ReadValue<Vector3>();

        if (IsGrounded())
        {
            _speed = _walkSpeed;
        }
    }

    /// <summary>
    /// Methode die die Direction der Camera findet und dem Spieler gibt
    /// </summary>
    private void FindDirection()
    {
        _direction = transform.position - Camera.main.transform.position;

        _direction.y = 0;
        _direction = _direction.normalized;

        _moveDirection = _direction * _move.z + ((Quaternion.AngleAxis(90, Vector3.up) * _direction) * _move.x);
    }

    /// <summary>
    /// Das Bewegen selbst in der FixedUpdate Methode
    /// </summary>
    private void Move()
    {
        if (isGrounded)
        {
            var _rbvelocityY = _rb.velocity.y;
            var _directionVec = _speed * _moveDirection.normalized;

            _directionVec.y = _rbvelocityY;
            _rb.velocity = _directionVec;
        }
        else
        {
            var _rbvelocity = _rb.velocity;
            var _rbvelocityY = _rb.velocity.y;

            _rbvelocity.y = 0;
            _rbvelocity += _speed * _moveDirection.normalized * 0.02f;
            float _rbSpeed = _rbvelocity.magnitude;

            if (_rbSpeed < _airSpeed)
            {
                _rbvelocity.y = _rbvelocityY;
                _rb.velocity = _rbvelocity;
            }
        }
    }

    /// <summary>
    /// Jumping in der Update Methode
    /// </summary>
    private void Jump()
    {
        if (IsGrounded())
        {
            bool isJumping = _jump.action.triggered;

            if (isJumping)
            {
                _rb.AddForce(_jumpSpeed * Vector3.up, ForceMode.Force);
            }
        }
    }

    /// <summary>
    /// Abfrage ob der Spieler Grounded ist
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, (float)_playerHeight);
        return isGrounded;
    }

    /// <summary>
    /// Manage all Animations und wird in der Update Methode ausgeführt
    /// </summary>
    private void ManageAnimation()
    {
        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);

            if (_move.x != 0 || _move.z != 0)
            {
                _animator.SetBool("isWalking", true);
            }
            else
            {
                _animator.SetBool("isWalking", false);
            }
        }
        else
        {
            _animator.SetBool("isJumping", true);
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (_moveDirection * 4));
    }

    //InputActions Aktivieren und Deaktivieren
    private void OnEnable()
    {
        _movement.action.Enable();
        _jump.action.Enable();
    }

    private void OnDisable()
    {
        _movement.action.Disable();
        _jump.action.Disable();
    }
}

