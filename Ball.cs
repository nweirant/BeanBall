using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D _rb;
    Collider2D _collider;
    [SerializeField] bool _endGame;
    private bool _bounced;
    public bool _deadBall;
    public GameObject _outFX;
    public bool _playerOnePossession;
    public GameObject _shadow;
    public GameObject _ballMark;
    public GameObject _ballHitFx;
    public Animator _anim;
    public bool _warmUpBall;

    private string _tag = "Ball";

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        GameEvents.Instance.onPlayerScored += PlayerScored;
    }

    public bool IsPlayerOnePossession()
    {
        return _playerOnePossession;
    }

    private void InstantiateOutFX()
    {
        Instantiate(_outFX, transform.position, Quaternion.identity);
    }

    public void ResetBall()
    {
        _deadBall = false;
        _rb.angularVelocity = 0;
        Freeze();
        UnFreeze();
        BallIn();
    }

    private void PlayerScored(PlayerName name)
    {
        _deadBall = true;
        DeadBall();
    }

    private void BallOut()
    {
        AudioManager.Instance.Play("whistle");
        InstantiateOutFX();
        _deadBall = true;

        if(_warmUpBall)
        {
            GameEvents.Instance.WarmUpScored(this);
        }
        else
        {
            GameEvents.Instance.BallOut();
        }

    }

    public void DeadBall()
    {
        gameObject.tag = "Untagged";
    }

    public void BallIn()
    {
        _deadBall = false;
        _shadow.SetActive(true);
        //_shadow.SetActive(false);
        gameObject.tag = _tag;

    }

    public void EndGame()
    {
        _endGame = true;
        ResetBall();
        //_bounced = false;
    }

    public void StartGame()
    {
        _endGame = false;
        gameObject.SetActive(true);
        _shadow.SetActive(true);
        UnFreeze();
    }

    public void Freeze()
    {
        if (_endGame)
            gameObject.SetActive(false);
        else
        {
            _bounced = false;
            _collider.enabled = false;
            _rb.velocity = Vector2.zero;
            _rb.isKinematic = true;
            _shadow.SetActive(false);
        }

    }
    public void DisableShadow()
    {
        //Destroy(_shadow);
        //_shadow.SetActive(false);
    }
    public void UnFreeze()
    {
        if (_endGame)
            return;
        transform.parent = null;
        _bounced = false;
        if (_rb == null)
            return;
        _rb.velocity = Vector2.zero;
        _collider.enabled = true;
        _shadow.SetActive(true);
        _rb.isKinematic = false;
    }

    public void Throw(Vector2 force, bool isPlayerOne = true)
    {
        _playerOnePossession = isPlayerOne;

        if (_endGame)
        {
            UnFreeze();
            return;
        }

        _deadBall = false;
        UnFreeze();
        _rb.AddForce(force, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Boundry"))
        {
            _deadBall = true;
            Freeze();
            BallOut();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bounce") || collision.gameObject.CompareTag("Ground"))
        {
            if (!_bounced && !_deadBall)
                _bounced = true;
            else if (!_deadBall)
            {
                Freeze();
                BallOut();
                // deadball
                _bounced = false;
            }
            AudioManager.Instance.Play("thump");
            _anim.Play("bounce");
            Instantiate(_ballMark, new Vector3(transform.position.x, -17), Quaternion.identity);
            Instantiate(_ballHitFx, new Vector3(transform.position.x, -17), Quaternion.identity);

        }
    }

    private void OnDisable()
    {
        if(gameObject != null && _shadow.gameObject != null)
            _shadow.SetActive(false);
    }
}

// idea to save velocity of the ball. like tenis..
// Save the current velocity before freezing
//Vector2 savedVelocity = rb.velocity;

//// Freeze the rigidbody's constraints
//rb.constraints = RigidbodyConstraints2D.FreezeAll;

//// Restore the saved velocity (optional)
//rb.velocity = savedVelocity;