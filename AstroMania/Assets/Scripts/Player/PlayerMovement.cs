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

    // Öffentlichkeit, damit andere Systeme den Grounded-Status abfragen können
    [FormerlySerializedAs("isGrounded")]
    public bool isGrounded;

    [Header("Jump")]
    [FormerlySerializedAs("_jumpSpeed")]
    [SerializeField] private float _jumpSpeed;

    [Header("AirSpeed")]
    [FormerlySerializedAs("_airSpeed")]
    [SerializeField] private float _airSpeed;

    // Laufender Speed-Wert (wird zur Laufzeit gesetzt)
    private float _speed;

    // Eingabevektor (x = seitlich, z = vor/zurück)
    private Vector3 _move;

    private Rigidbody _rb;

    // Bewegungsrichtung relativ zur Kamera
    private Vector3 _moveDirection;

    [Header("InputActions")]
    [FormerlySerializedAs("_movement")]
    [SerializeField] private InputActionReference _movement;

    [FormerlySerializedAs("_jump")]
    [SerializeField] private InputActionReference _jump;

    [Header("Animator")]
    [FormerlySerializedAs("_animator")]
    [SerializeField] private Animator _animator;

    // temporäre Richtung basierend auf Kamera
    private Vector3 _direction;

    private void Start()
    {
        // Rigidbody einmalig holen
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Input und Sprung werden in Update erfasst
        InputMove();
        Jump();

        // Animationen basierend auf aktuellem Zustand aktualisieren
        ManageAnimation();
    }

    private void FixedUpdate()
    {
        // Physikbasierte Bewegungsberechnung
        FindDirection();
        Move();
    }

    /// <summary>
    /// Liest die Movement-InputAction und setzt die aktuelle Geschwindigkeit (am Boden)
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
    /// Berechnet die Bewegungsrichtung relativ zur Kamera (forward und right)
    /// </summary>
    private void FindDirection()
    {
        // Richtung von Kamera zu Spieler (y wird ignoriert)
        _direction = transform.position - Camera.main.transform.position;
        _direction.y = 0;
        _direction = _direction.normalized;

        // Kombination aus forward (z) und right (x)
        _moveDirection = _direction * _move.z + ((Quaternion.AngleAxis(90, Vector3.up) * _direction) * _move.x);
    }

    /// <summary>
    /// Führt die eigentliche Bewegung durch. Unterscheidung zwischen Boden- und Luftbewegung.
    /// </summary>
    private void Move()
    {
        if (isGrounded)
        {
            var _rbvelocityY = _rb.linearVelocity.y;
            var _directionVec = _speed * _moveDirection.normalized;

            _directionVec.y = _rbvelocityY;
            _rb.linearVelocity = _directionVec;
        }
        else
        {
            var _rbvelocity = _rb.linearVelocity;
            var _rbvelocityY = _rb.linearVelocity.y;

            // nur horizontale Komponente beeinflussen
            _rbvelocity.y = 0;
            _rbvelocity += _speed * _moveDirection.normalized * 0.02f;
            float _rbSpeed = _rbvelocity.magnitude;

            // Solange Luftgeschwindigkeit kleiner als Limit, Geschwindigkeit setzen
            if (_rbSpeed < _airSpeed)
            {
                _rbvelocity.y = _rbvelocityY;
                _rb.linearVelocity = _rbvelocity;
            }
        }
    }

    /// <summary>
    /// Verarbeitet Sprung-Input. Nur wenn Spieler am Boden ist.
    /// </summary>
    private void Jump()
    {
        if (IsGrounded())
        {
            bool isJumping = _jump.action.triggered;

            if (isJumping)
            {
                AudioManager.Instance.PlaySound("Jump");
                _rb.AddForce(_jumpSpeed * Vector3.up, ForceMode.Force);
            }
        }
    }

    /// <summary>
    /// Prüft per Raycast, ob der Spieler den Boden berührt.
    /// </summary>
    /// <returns>True, wenn grounded</returns>
    private bool IsGrounded()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, (float)_playerHeight);
        return isGrounded;
    }

    /// <summary>
    /// Aktualisiert Animator-Parameter basierend auf Bewegung und Grounded-State.
    /// </summary>
    private void ManageAnimation()
    {
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

    // InputActions aktivieren / deaktivieren
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

