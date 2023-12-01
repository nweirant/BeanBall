using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour
{
    public Transform _ballHolder;
    PlayerController _player;
    //Ball _ball;

    private void Start()
    {
        _player = GetComponent<PlayerController>();
        //_ball = FindFirstObjectByType<Ball>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            //Set Ball To Pause Gravity
            Ball _ball = collision.gameObject.GetComponent<Ball>();
            _ball.Freeze();
            _player.CatchBall(_ball);
            _ball.transform.position = _ballHolder.position;
            _ball.transform.parent = _ballHolder;
        }
    }
}
