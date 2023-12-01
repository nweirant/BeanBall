using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform _ballHolder;
    public GameObject _aimer;
    public bool _cpu = false;
    public PlayerMovement _movement;
    public PlayerSO _playerSO;
    public float _holdTime = 2f;
    Rigidbody2D _rb;
    Collider2D _collider;
    PlayerAnimationController _animController;
    public AimRotator _rotator;

    public float _maxThrowForce;
    public float _minThrowForce;
    [SerializeField] private float _currentThrowForce;
    public float _chargeRate;

    [SerializeField]bool _hasBall;
    Ball _ball;
    private bool _changedRotatorSpeed;
    public PlayerHUDManager _hudManager;

    private void Start()
    {
        _currentThrowForce = _minThrowForce;

        if(_hudManager != null)
            _hudManager.SetUp(_maxThrowForce, _minThrowForce);

        _rb = GetComponent<Rigidbody2D>();
        _aimer.SetActive(false);
        _collider = GetComponent<Collider2D>();
        _animController = GetComponent<PlayerAnimationController>();

    }

    public void ResetPlayer()
    {
        _movement.HasBall(false);
        _hasBall = false;
        _aimer.SetActive(false);
        _animController.PlayIdle();
        StopCoroutine(SlowMotion());
        UnFreeze();
        _movement.ChargingUp(false);
        _currentThrowForce = _minThrowForce;
    }

    private void Update()
    {
        if (_hasBall && Input.GetKeyDown(KeyCode.Space) && !_cpu)
        {
            var force = _currentThrowForce;
            ThrowBall(force);
            _hudManager.ShowChargeSlider(false);
        }

        if(!_hasBall && Input.GetKey(KeyCode.Space) && !_cpu)
        {
            _movement.ChargingUp();
            _currentThrowForce += Time.deltaTime * _chargeRate;
            _hudManager.ShowChargeSlider();
            _hudManager.UpdateChargeSlider(_currentThrowForce);
        }
        else if (!_hasBall && !_cpu)
        {
            _movement.ChargingUp(false);
            if (_currentThrowForce > _minThrowForce)
            {                
                _currentThrowForce -= Time.deltaTime * _chargeRate * 2;
                _hudManager.UpdateChargeSlider(_currentThrowForce);
            }
        }

        if(!_cpu)
        {
            var xpos = transform.position.x;

            if (xpos > -15)
            {
                _rotator.AdjustRotatorSpeed(0.75f);
            }
            else if (xpos < -16 && xpos > -34)
            {
                _rotator.AdjustRotatorSpeed(1.25f);
            }
            else if (xpos < -35 && xpos > -49)
            {
                _rotator.AdjustRotatorSpeed(1.5f);
            }
            else if (xpos < -50)
            {
                _rotator.AdjustRotatorSpeed(1.8f);
            }
        }    

    }

    public PlayerStats GetStats()
    {
        return _playerSO.GetStats();
    }

    public PlayerStats SaveGameStats(PlayerGameStats stats)
    {
        return _playerSO.SaveGameStats(stats);
    }

    public void ThrowBall(float force = 0)
    {
        AudioManager.Instance.Play("throw");
        _currentThrowForce = _minThrowForce;
        force = Mathf.Clamp(force, _minThrowForce, _maxThrowForce);
        _playerSO.AddToStats(new PlayerGameStats() { Shots = 1 });

        if (!_cpu)
            _movement.HasBall(false);

        if (_animController != null)
            _animController.ChangeAnimationState(AnimationStates.Throw);

        if (force == 0)
            force = _minThrowForce;

        _hasBall = false;
        _aimer.SetActive(false);
        _ball.Throw(force * _ballHolder.transform.up, _playerSO._human);
        StopCoroutine(SlowMotion());
        Invoke(nameof(UnFreeze), 0.2f);
    }


    public void CatchBall(Ball ball)
    {
        AudioManager.Instance.Play("catch2");
        _aimer.SetActive(true);
        _hasBall = true;
        _ball = ball;
        if (!_cpu)
            _movement.HasBall(true);

        if (_animController != null)
            _animController.ChangeAnimationState(AnimationStates.Catch);

        StartCoroutine(SlowMotion());
    }

    IEnumerator SlowMotion()
    {
        Freeze();
        yield return new WaitForSeconds(_holdTime);
        //UnFreeze();
        if (_hasBall)
            ThrowBall();
    }

    private void Freeze()
    {
        _collider.enabled = false;
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
    }

    public void UnFreeze()
    {
        _collider.enabled = true;
        if (!_cpu)
        {
            _rb.isKinematic = false;
        }
    }
}