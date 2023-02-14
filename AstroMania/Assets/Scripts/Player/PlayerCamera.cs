using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    [Header("InputActions")]
    [SerializeField] private InputActionReference _movement;

    [Header("Camera")]
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _playerObj;

    [SerializeField] private float _rotationSpeed;

    private Vector3 move;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _movement.action.Enable();
    }

    private void OnDisable()
    {
        _movement.action.Disable();
    }

    private void Update()
    {
        InputMove();
        CameraLook();
    }

    private void InputMove()
    {
        move = _movement.action.ReadValue<Vector3>();
    }

    private void CameraLook()
    {
        Vector3 viewDir = _player.position - new Vector3(transform.position.x, _player.position.y, transform.position.z);
        _orientation.forward = viewDir.normalized;

        Vector3 inputDir = _orientation.forward * move.z + _orientation.right * move.x;

        if(inputDir != Vector3.zero)
        {   
            _playerObj.forward = Vector3.Slerp(_playerObj.forward, inputDir.normalized, Time.deltaTime * _rotationSpeed);
        }
    }
}
