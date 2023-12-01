using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoordinator : MonoBehaviour
{
    public static GameCoordinator Instance;

    public BallSpawner _ballSpawner;
    public Ball _ball;
    public Ball _practiceBall;

    private bool _timeExpired;

    public PlayerController _playerOne;
    private PlayerController _playerTwo;
    public Animator _lightsAnimator;

    public Transform _playerOneSpawnPosition;
    public GameObject _practiceLight;

    private void ListenToEvents()
    {
        GameEvents.Instance.onBallOut += DeadBall;
        GameEvents.Instance.onTimeExpired += TimeExpired;
        GameEvents.Instance.onPlayerScored += PlayerScored;
        //GameEvents.Instance.onSeasonStart += SeasonStart;

    }

    private void Start()
    {
        ListenToEvents();
        _ball.gameObject.SetActive(false);
        _timeExpired = false;
        _practiceBall.gameObject.SetActive(false);

    }

    public void StartPractice()
    {
        _practiceBall.DisableShadow();
        UIManager.Instance.EnablePracticeUI();
        _practiceBall.gameObject.SetActive(true);
        EnableHumanGameObject(true);
    }

    public void ResetPractice()
    {
        _practiceLight.SetActive(false);
        _practiceBall.UnFreeze();
        _practiceBall.gameObject.SetActive(false);
    }

    //private void SeasonStart(SeasonManager manager)
    //{
    //    _practiceLight.SetActive(false);
    //    _practiceBall.UnFreeze();
    //    _practiceBall.gameObject.SetActive(false);
    //}

    public void PlayerScored(PlayerName _player)
    {
        AudioManager.Instance.Play("goal");

        if (_player == PlayerName.PLAYERONE)
        {
            _lightsAnimator.Play("playerScored");
        }

    }

    public void ResetHumanPosition()
    {
        _playerOne.transform.position = _playerOneSpawnPosition.position;
    }

    public void EnableHumanGameObject(bool enabled = true)
    {
        _playerOne.gameObject.SetActive(enabled);
    }

    public void StartMatch(PlayerController playerOne, PlayerController playerTwo)
    {
        _playerOne = playerOne;
        SetPlayerTwo(playerTwo);
        UpdatePlayerRecordsUI();
        _ball.gameObject.SetActive(true);
        _ball.StartGame();
        UIManager.Instance.DisplayMessage($"{playerOne._playerSO._name} vs. {playerTwo._playerSO._name}");
        AudioManager.Instance.Play("whistle");
    }

    public void UpdatePlayerRecordsUI()
    {
        UIManager.Instance.ShowPlayerRecords(_playerOne._playerSO, _playerTwo._playerSO);
    }

    public void SetPlayerTwo(PlayerController playerTwo)
    {
        _ballSpawner.SetPositionOfCPUSpawner(playerTwo.gameObject.transform.position);
        _playerTwo = playerTwo;
    }

    public void DropBall()
    {
        _ballSpawner.DropBall(_ball);
    }

    private void DeadBall()
    {
       _ballSpawner.DropBall(_ball);
    }

    private void TimeExpired()
    {
        _timeExpired = true;
    }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
