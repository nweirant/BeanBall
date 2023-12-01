using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int _lateralSpeed;
    public int _chargeUpSpeed;
    private int _currentSpeed;
    public int _jumpForce;
    public bool _isGrounded;
    private bool _hasBall;
    MovementController _controller;
    Rigidbody2D _rb;
    PlayerAnimationController _animController;

    private float _maxMove = -67.5f;
    private float _minMove = -3.6f;

    bool _jump = false;
    float _lateralMovement = 0f;

    public GameObject _landFX;
    public GameObject _jumpFX;

    void Start()
    {
        _currentSpeed = _lateralSpeed;
        _rb = GetComponent<Rigidbody2D>();
        _animController = GetComponent<PlayerAnimationController>();
        _controller = GetComponent<MovementController>();
    }

    public void HasBall(bool has)
    {
        _hasBall = has;
    }

    public void OnLand()
    {
        Instantiate(_landFX, transform.position, Quaternion.identity);
    }


    private void FixedUpdate()
    {
        _controller.Move(_lateralMovement * Time.fixedDeltaTime, false, _jump);
        _jump = false;
    }

    public void ChargingUp(bool charging = true)
    {
        if (charging)
            _currentSpeed = _chargeUpSpeed;
        else
            _currentSpeed = _lateralSpeed;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && !_hasBall && _isGrounded || Input.GetKeyDown(KeyCode.UpArrow) && !_hasBall && _isGrounded)
        {
            _animController.ChangeAnimationState(AnimationStates.Jump);
            AudioManager.Instance.Play("jump");
            Instantiate(_jumpFX, transform.position, Quaternion.identity);
            _jump = true;
            _isGrounded = false;
        }

        if (!_hasBall)
        {
            _lateralMovement = Input.GetAxisRaw("Horizontal") * _currentSpeed;
            if(transform.position.x >= _minMove && _lateralMovement > 0)
            {
                _lateralMovement = 0;
            }
            else if (transform.position.x <= _maxMove && _lateralMovement < 0)
            {
                _lateralMovement = 0;
            }
        }
        else
            _lateralMovement = 0;
    }

    public void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        //Instantiate(_jumpFX, transform.position, Quaternion.identity);
        _isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Bounce"))
        {
            _isGrounded = true;
        }
    }
}
