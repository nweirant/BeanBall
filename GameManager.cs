using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int _playerOneScore;
    public int _playerTwoScore;
    private float _gameDuration;
    public Ball _ball;
    public PlayerController _player;
    [SerializeField] PlayerController _cpu;
    public BallSpawner _ballSpawner;

    private float _currentGameTime;
    private bool _overtime;

    private SeasonManager _currentSeason;
    private bool _pausedTime;

    private void ListenToEvents()
    {
        GameEvents.Instance.onPlayerScored += PlayerScored;
        GameEvents.Instance.onSeasonStart += SeasonStarted;
    }

    private void Start()
    {
        AudioManager.Instance.Play("practice");
        ListenToEvents();
    }

    public void PauseTime(bool pause)
    {
        _pausedTime = pause;
    }

    private void StartGame()
    {
        _player.ResetPlayer();
        var playerTwo = GetNextOpponent();
        _cpu = playerTwo;
        _overtime = false;
        _currentGameTime = _gameDuration;
        GameCoordinator.Instance.StartMatch(_player, playerTwo);
    }

    private void SetGameDuration(float amount)
    {
        _gameDuration = amount;
    }

    private void SeasonStarted(SeasonManager seasonManager)
    {
        GameCoordinator.Instance.ResetPractice();
        _currentSeason = seasonManager;
        SetGameDuration(_currentSeason.GetSecondsPerGame());
        StartGame();
    }

    private PlayerController GetNextOpponent()
    {
        _cpu = _currentSeason.SpawnNextOpponent();
        if (_cpu == null)
            return null;
        GameCoordinator.Instance.SetPlayerTwo(_cpu);
        DropBall();
        return _cpu;
    }

    private void DisplayStandings(List<PlayerSO> players = null, bool enabled = true)
    {
        var nextOpp = _currentSeason.GetNextOpponent();
        var gamesRemainings = _currentSeason.GetGamesRemaining();
        if (nextOpp != null && enabled)
        {
            UIManager.Instance.UpdateStandingsStatus(gamesRemainings, nextOpp._name);
        }
        UIManager.Instance.DisplayStandings(players, enabled);
    }

    private void Update()
    {
        if(_currentGameTime > 0 && !_overtime && !_pausedTime)
        {
            _currentGameTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        string minutes = Mathf.Floor(_currentGameTime / 60).ToString("00");
        string seconds = (_currentGameTime % 60).ToString("00");
        if(_currentGameTime <= 0)
        {
            UIManager.Instance.UpdateTimer($"0:00");
            EndGame();
        }
        else
        {
            UIManager.Instance.UpdateTimer($"{minutes}:{seconds}");
        }

        if (_overtime)
        {
            UIManager.Instance.UpdateTimer("Overtime!");
        }
    }

    void EndGame()
    {
        
        if(_playerOneScore == _playerTwoScore)
        {
            AudioManager.Instance.Play("heartBeat");
            _overtime = true;
        }
        else
        {
            AudioManager.Instance.Pause("heartBeat");
            AudioManager.Instance.Play("whistle");
            _ball.EndGame();
            var playerWin = _playerOneScore > _playerTwoScore;
            if (playerWin)
                UIManager.Instance.DisplayMessage($"Victory!! {_playerOneScore} - {_playerTwoScore}");
            else
                UIManager.Instance.DisplayMessage($"Defeated. {_playerTwoScore} - {_playerOneScore}");

            Invoke(nameof(EndOfGameEvents), 2f);
        }
    }

    [SerializeField]bool _advanced = false;


    void EndOfGameEvents()
    {
        if(_playerOneScore > _playerTwoScore)
        {
            AudioManager.Instance.Play("win");
        }

        GameCoordinator.Instance.EnableHumanGameObject(false);
        SaveGameStats();

        var nextOpponent = _currentSeason.GetNextOpponent();

        if (_currentSeason._inFinals && nextOpponent == null)
        {
            _advanced = _playerOneScore > _playerTwoScore;
            UIManager.Instance.PopulateEndOfSeasonScreen(_advanced, _player._playerSO);
        }
        else if (!_currentSeason._inFinals && nextOpponent == null)
        {
            _advanced = false;
            UIManager.Instance.PopulateEndOfSeasonScreen(false, _player._playerSO);
        }
        else if (nextOpponent != null)
        {
            DisplayStandings(_currentSeason.GetPlayers());
        }
    }


    public void StartNextMatch()
    {
        if(LeaugeManager.Instance.GetSeasonNumber() > 3)
        {
            Application.Quit();
            return;
        }
        var p1Score = _playerOneScore;
        var p2Score = _playerTwoScore;

        ResetMatch();

        var playerTwo = GetNextOpponent();
        if(playerTwo != null)
        {
            _cpu = playerTwo;
            GameCoordinator.Instance.StartMatch(_player, playerTwo);
        }
        else
        {
            LeaugeManager.Instance.EndSeason(_advanced);
            //_currentSeason = LeaugeManager.Instance.GetCurrentSeason();
        }

        UIManager.Instance.ShowEndOfSeasonScreen(false);
        _advanced = false;
    }



    void ResetMatch()
    {
        GameCoordinator.Instance.ResetHumanPosition();
        GameCoordinator.Instance.EnableHumanGameObject();
        DisplayStandings(null, false);
        _overtime = false;
        _cpu.gameObject.SetActive(false);
        UIManager.Instance.ResetScore();
        _playerOneScore = 0;
        _playerTwoScore = 0;
        _currentGameTime = _gameDuration;
    }

    void SaveGameStats()
    {
        var win = _playerOneScore > _playerTwoScore;

        var playerStats = new PlayerGameStats()
        {
            Win = win,
            GoalsScored = _playerOneScore,
            GoalsAgainst = _playerTwoScore 
        };

        var cpuStats = new PlayerGameStats()
        {
            Win = !win,
            GoalsScored = _playerTwoScore,
            GoalsAgainst = _playerOneScore
        };

        _cpu.SaveGameStats(cpuStats);
        _player.SaveGameStats(playerStats);
    }


    public void PlayerScored(PlayerName _player)
    {
        if (_player == PlayerName.PLAYERONE)
        {
            _playerOneScore += 1;
            UIManager.Instance.DisplayMessage($"Goal!! {_playerOneScore} - {_playerTwoScore}");
            UIManager.Instance.UpdateScore(_player, _playerOneScore);
        }
        else
        {
            _playerTwoScore += 1;
            UIManager.Instance.DisplayMessage($"Goal {_playerOneScore} - {_playerTwoScore}");
            UIManager.Instance.UpdateScore(_player, _playerTwoScore);
        }

        if (_overtime)
            EndGame();
        else
        {
            PauseTime(true);
            Invoke(nameof(DropBall), 1.3f);
        }
    }

    void DropBall()
    {
        GameCoordinator.Instance.DropBall();
        PauseTime(false);
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