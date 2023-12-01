using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerName{ PLAYERONE, PLAYERTWO }

public class Goal : MonoBehaviour
{
    public PlayerName _playerName;
    public BallSpawner _spawner;
    public GameObject _scoreFX;
    public Animator _anim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            
            Ball ball = collision.GetComponent<Ball>();

            if(!ball._warmUpBall)
            {
                _anim.Play("score");
                Instantiate(_scoreFX, collision.gameObject.transform.position, Quaternion.identity);
                GameEvents.Instance.PlayerScored(_playerName);
            }
            else
            {
                GameEvents.Instance.WarmUpScored(ball);
            }

        }
    }
}
