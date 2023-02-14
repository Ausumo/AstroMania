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
    [SerializeField] private float _maxSpeed;

    [Header("GroundCheck")]
    [SerializeField] private float _playerHeight;
    private bool _isGrounded;

    [Header("Jump")]
    [SerializeField] private float _jumpSpeed;

    private float _speed;
    private Vector3 _move;
    private Rigidbody _rb;
    private Vector3 _moveDirection;
    [SerializeField] private Transform _orientation;

    [Header("InputActions")]
    [SerializeField] private InputActionReference _movement;
    [SerializeField] private InputActionReference _jump;

    [Header("Animator")]
    [SerializeField] private Animator _animator;


    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, _moveDirection.normalized * 4, Color.green);
        
        InputMove();
        Jump();

        ManageAnimation();
    }
    private void FixedUpdate()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        dir.y = 0;
        Vector3 forward = Quaternion.LookRotation(dir) * Vector3.forward;
        Vector3 right = Quaternion.LookRotation(dir) * Vector3.right;

        _moveDirection = /*_orientation.*/ forward * _move.z + /*_orientation.*/ right * _move.x;
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
    /// Das Bewegen selbst in der FixedUpdate Methode
    /// </summary>
    private void Move()
    {
        _rb.AddForce(_speed * _moveDirection.normalized, ForceMode.Force);
        //transform.position += _moveDirection.normalized * 0.1f;
        _rb.velocity = new Vector3(Mathf.Clamp(_rb.velocity.x, -_maxSpeed, _maxSpeed), _rb.velocity.y, Mathf.Clamp(_rb.velocity.z, -_maxSpeed, _maxSpeed));
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
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, (float)_playerHeight);
        return _isGrounded;
    }

    /// <summary>
    /// Manage all Animations und wird in der Update Methode ausgeführt
    /// </summary>
    private void ManageAnimation()
    {
        if(_isGrounded)
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

