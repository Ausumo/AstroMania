using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [FormerlySerializedAs("_walkSpeed")]
    [SerializeField] private float _walkSpeed;

    [Header("GroundCheck")]
    [FormerlySerializedAs("_playerHeight")]
    [SerializeField] private float _playerHeight;

    // Public so other systems can query the grounded status
    [FormerlySerializedAs("isGrounded")]
    public bool isGrounded;

    [Header("Jump")]
    [FormerlySerializedAs("_jumpSpeed")]
    [SerializeField] private float _jumpSpeed;

    [Header("AirSpeed")]
    [FormerlySerializedAs("_airSpeed")]
    [SerializeField] private float _airSpeed;

    [Header("Footstep Impact")]
    [Tooltip("Minimum vertical velocity (m/s) required to register a landing/impact for deformation")] 
    [SerializeField] private float _minImpactVelocity = 1.0f;

    // Current speed value (set at runtime)
    private float _speed;

    // Input vector (x = sideways, z = forward/back)
    private Vector3 _move;

    private Rigidbody _rb;

    // Movement direction relative to the camera
    private Vector3 _moveDirection;

    [Header("InputActions")]
    [FormerlySerializedAs("_movement")]
    [SerializeField] private InputActionReference _movement;

    [FormerlySerializedAs("_jump")]
    [SerializeField] private InputActionReference _jump;

    [Header("Animator")]
    [FormerlySerializedAs("_animator")]
    [SerializeField] private Animator _animator;

    [Header("Footsteps")]
    [FormerlySerializedAs("_footstepDeformer")]
    [SerializeField] private FootstepDeformer _footstepDeformer;

    // Temporary direction based on the camera
    private Vector3 _direction;

    // Track previous grounded state to detect landings
    private bool _wasGrounded;

    // track last vertical velocity to estimate impact on landing
    private float _lastVerticalVelocity;

    private void Start()
    {
        // Cache Rigidbody reference
        _rb = GetComponent<Rigidbody>();

        // Initialize grounded state
        _wasGrounded = IsGrounded();
        _lastVerticalVelocity = 0f;
    }

    private void Update()
    {
        // Update grounded state early so landing can be detected here
        bool currentlyGrounded = IsGrounded();

        // Detect landing transition (was not grounded, now grounded)
        if (currentlyGrounded && !_wasGrounded)
        {
            // Use last recorded vertical velocity (from physics) to estimate impact
            float impactForce = 0f;
            if (_rb != null)
            {
                if (Mathf.Abs(_lastVerticalVelocity) >= _minImpactVelocity)
                {
                    impactForce = _rb.mass * Mathf.Abs(_lastVerticalVelocity);
                }
            }

            // Trigger footstep deformation at player's position only if significant impact
            if (impactForce > 0f && _footstepDeformer != null)
            {
                _footstepDeformer.TriggerStepAtPosition(transform.position, impactForce);
            }
        }

        // Read input and handle jump in Update
        InputMove();
        Jump();

        // Update animations based on current state
        ManageAnimation();

        // store for next frame
        _wasGrounded = currentlyGrounded;
    }

    private void FixedUpdate()
    {
        // Record vertical velocity at the start of physics step (used for landing impact)
        if (_rb != null)
            _lastVerticalVelocity = _rb.velocity.y;

        // Physics-based movement calculations
        FindDirection();
        Move();
    }

    /// <summary>
    /// Called when the Rigidbody collides with another collider. Use collision to reliably detect landings.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (_footstepDeformer == null || _rb == null) return;

        // iterate contacts to find a mostly-upward normal (ground contact)
        foreach (var contact in collision.contacts)
        {
            // consider this a landing if the contact normal points sufficiently upward
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                // impact velocity relative to collision
                float impactVel = collision.relativeVelocity.y;

                // only consider as impact if vertical speed exceeds threshold
                if (Mathf.Abs(impactVel) < _minImpactVelocity) continue;

                float impactForce = _rb.mass * Mathf.Abs(impactVel);

                _footstepDeformer.TriggerStepAtPosition(contact.point, impactForce);
                break;
            }
        }
    }

    /// <summary>
    /// Reads the movement InputAction and sets the current speed (when grounded)
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
    /// Calculates movement direction relative to the camera (forward and right)
    /// </summary>
    private void FindDirection()
    {
        // Direction from camera to player (ignore Y)
        if (Camera.main == null) return;
        _direction = transform.position - Camera.main.transform.position;
        _direction.y = 0;
        _direction = _direction.normalized;

        // Combine forward (z) and right (x)
        _moveDirection = _direction * _move.z + ((Quaternion.AngleAxis(90, Vector3.up) * _direction) * _move.x);
    }

    /// <summary>
    /// Performs the actual movement. Differentiates between grounded and air movement.
    /// </summary>
    private void Move()
    {
        if (_rb == null) return;

        if (isGrounded)
        {
            var rbVelocityY = _rb.velocity.y;
            var directionVec = _speed * _moveDirection.normalized;

            directionVec.y = rbVelocityY;
            _rb.velocity = directionVec;
        }
        else
        {
            var rbvelocity = _rb.velocity;
            var rbvelocityY = _rb.velocity.y;

            // Only affect horizontal component
            rbvelocity.y = 0;
            rbvelocity += _speed * _moveDirection.normalized * 0.02f;
            float rbSpeed = rbvelocity.magnitude;

            // While air speed is below limit, apply velocity
            if (rbSpeed < _airSpeed)
            {
                rbvelocity.y = rbvelocityY;
                _rb.velocity = rbvelocity;
            }
        }
    }

    /// <summary>
    /// Processes jump input. Only when player is grounded.
    /// </summary>
    private void Jump()
    {
        if (IsGrounded())
        {
            bool isJumping = _jump.action.triggered;

            if (isJumping)
            {
                AudioManager.Instance.PlaySound("Jump");
                if (_rb != null) _rb.AddForce(_jumpSpeed * Vector3.up, ForceMode.Force);
            }
        }
    }

    /// <summary>
    /// Checks via Raycast whether the player is touching the ground.
    /// </summary>
    /// <returns>True when grounded</returns>
    private bool IsGrounded()
    {
        // Raycast a little below the transform to be robust against small offsets
        float originOffset = 0.1f;
        Vector3 origin = transform.position + Vector3.up * originOffset;
        float distance = _playerHeight + originOffset;
        isGrounded = Physics.Raycast(origin, Vector3.down, distance);
        return isGrounded;
    }

    /// <summary>
    /// Updates animator parameters based on movement and grounded state.
    /// </summary>
    private void ManageAnimation()
    {
        if (_animator == null) return;

        if (isGrounded)
        {
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isWalking", _move.x != 0 || _move.z != 0);
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

    // Enable / disable InputActions
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

