using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public Transform _playerOneSpawner;
    public Transform _playerTwoSpawner;
    public GameObject _ballInFx;

    private bool _playerOneTurn = true;

    private void Start()
    {
        GameEvents.Instance.onWarmupScored += ResetPlayerBall;
    }

    private void ResetPlayerBall(Ball ball)
    {
        StartCoroutine(ResetPlayerBallAsync(ball));

    }

    private IEnumerator ResetPlayerBallAsync(Ball ball)
    {
        yield return new WaitForSeconds(1);
        ball.BallIn();
        AudioManager.Instance.Play("serve");
        ball.transform.position = _playerOneSpawner.position;
        Instantiate(_ballInFx, ball.transform.position, Quaternion.identity);
        ball.ResetBall();
        ball.Freeze();
        ball.UnFreeze();

    }

    public void DropBall(Ball _ball)
    {
        _ball.BallIn();
        AudioManager.Instance.Play("serve");   
        StartCoroutine(Drop(_ball));
    }

    public void SetPositionOfCPUSpawner(Vector3 pos)
    {
        _playerTwoSpawner.position = new Vector3(pos.x, _playerTwoSpawner.position.y);
    }

    private IEnumerator Drop(Ball _ball)
    {
        if (!_ball._playerOnePossession)
        {
            _ball.transform.position = _playerOneSpawner.position;
        }
        else
        {
            _ball.transform.position = _playerTwoSpawner.position;
        }

        Instantiate(_ballInFx, _ball.transform.position, Quaternion.identity);

        _ball._playerOnePossession = !_ball._playerOnePossession;
        _playerOneTurn = !_playerOneTurn;

        if (_ball != null)
            yield return null;

        _ball.Freeze();
        yield return new WaitForSeconds(0.5f);
        _ball.UnFreeze();

    }
}


//how do you comment in this language
//ok so here's the stotry"
//there's some cubs ok
//and they're clackin around and clackin around
//clack clack clack
//and they're livin cub life and livin la vida loca
//and they're in love
//and that's the whole story
//and I'm going to take this keyboard so that I can do evrything on it
//so I hope you have noise canceling headphones
//ok
//so
//ya know

