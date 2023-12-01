using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SeasonStatus { INPROGRESS, COMPLETED, FAILED}
public class SeasonManager : MonoBehaviour
{
    public string SeasonName;

    List<PlayerController> _opponents = new List<PlayerController>();
    GameObject[] _seasonOpponents;

    [SerializeField] private int _loopsPerSeason = 0;
    [SerializeField] private int _secondsPerGame = 30;

    public SeasonStatus _seasonStatus = SeasonStatus.INPROGRESS;
    private int _currentLoopCount = 0;
    [SerializeField]private int _gameIndex = 0;
    private int _gamesPlayedThisSeason = 0;
    private int _numberOfOpponents;
    public bool _inFinals;

    public GameObject[] _seasonFX;

    private int GamesPerSeason()
    {
        //return _opponents.Count;
        return _numberOfOpponents * (_loopsPerSeason + 1);
    }

    public int GetSecondsPerGame()
    {
        return _secondsPerGame;
    }

    public void SetUp()
    {
        ClearPlayerStats();
        _opponents.Clear();
        _numberOfOpponents = transform.childCount;

        //for (int x = 0; x < _loopsPerSeason + 1; x++)
        //{
            for (int i = 0; i < _numberOfOpponents; i++)
            {
                _opponents.Add(transform.GetChild(i).GetComponent<PlayerController>());
            }
        //}

        foreach (var go in _seasonFX)
        {
            go.SetActive(true);
        }
    }

    public void ClearPlayerStats()
    {
        _inFinals = false;
        _gameIndex = 0;
        _seasonStatus = SeasonStatus.INPROGRESS;
        _gamesPlayedThisSeason = 0;
        _currentLoopCount = 0;

        var players = GetPlayers();

        foreach (var p in players)
        {
            p.ClearStats();
        }

        PlayerSO player = Resources.Load("Players/Human") as PlayerSO;
        player.ClearStats();
    }

    public List<PlayerSO> GetPlayers()
    {
        List<PlayerSO> players = new List<PlayerSO>();
        var numberOfOpponents = transform.childCount;
        
        for (int i = 0; i < numberOfOpponents; i++)
        {
            players.Add(transform.GetChild(i).GetComponent<PlayerController>()._playerSO);
        }

        PlayerSO player = Resources.Load("Players/Human") as PlayerSO;
        players.Add(player);

        players.Sort((p1, p2) =>
        {
            return p2.Rating() - p1.Rating();
        });

        return players;
    }

    public void SimulateOtherMatches()
    {
        var firstWinner = Random.Range(1, 3) == 2;

        var winnerScore = Random.Range(1, 4);
        var loserScore = Mathf.Clamp(winnerScore - Random.Range(1, 3), 0, 3);

        switch (_gameIndex)
        {
            case 0:
                _opponents[1].SaveGameStats(new PlayerGameStats() { Win = firstWinner, GoalsScored = firstWinner ? winnerScore : loserScore });
                _opponents[2].SaveGameStats(new PlayerGameStats() { Win = !firstWinner, GoalsScored = !firstWinner ? winnerScore : loserScore });
                break;
            case 1:
                _opponents[0].SaveGameStats(new PlayerGameStats() { Win = firstWinner, GoalsScored = firstWinner ? winnerScore : loserScore });
                _opponents[2].SaveGameStats(new PlayerGameStats() { Win = !firstWinner, GoalsScored = !firstWinner ? winnerScore : loserScore });
                break;
            case 2:
                _opponents[0].SaveGameStats(new PlayerGameStats() { Win = firstWinner, GoalsScored = firstWinner ? winnerScore : loserScore });
                _opponents[1].SaveGameStats(new PlayerGameStats() { Win = !firstWinner, GoalsScored = !firstWinner ? winnerScore : loserScore });
                break;
            default:
                break;
        }
    }

    public PlayerSO GetNextOpponent()
    {
        var nextIndex = _gameIndex;
        //if (nextIndex >= _opponents.Count && _currentLoopCount < _loopsPerSeason)
        //{
        //    nextIndex = 0;
        //}


        if (nextIndex > _opponents.Count)
        {
            return null;
        }

        if (nextIndex == _numberOfOpponents)
        {
            if (PlayerMadeFinals())
            {
                //_gameIndex += 1;
                _inFinals = true;
                return GetFinalsOpponent()._playerSO;
            }
            else
            {
                return null;
            }
        }

        //_gameIndex += 1;

        //if (_gameIndex >= _opponents.Count && _currentLoopCount < _loopsPerSeason)
        //{
        //    _gameIndex = 0;
        //    _currentLoopCount += 1;
        //}

        return _opponents[nextIndex]._playerSO;
    }

 
    public SeasonStatus GetSeasonStatus(bool wonLastGame)
    {
        if (_inFinals && wonLastGame)
            return SeasonStatus.COMPLETED;

        return SeasonStatus.FAILED;
    }

    public PlayerController SpawnNextOpponent()
    {
        UIManager.Instance.ShowBanner(false);

        if (_gameIndex > _opponents.Count)
        {
            foreach (var go in _seasonFX)
            {
                go.SetActive(false);
            }
            return null;
        }

        if (_gameIndex == _numberOfOpponents)
        {
            if (PlayerMadeFinals())
            {
                _inFinals = true;
                UIManager.Instance.UpdateSeasonGamesInfo($"Division Championship");
                UIManager.Instance.ShowBanner(true);
                var finalsOpponent = GetFinalsOpponent();
                finalsOpponent.gameObject.SetActive(true);
                _gameIndex += 1;
                CheckSeasonLoop();
                return finalsOpponent;
            }
            else
            {
                foreach (var go in _seasonFX)
                {
                    go.SetActive(false);
                }
                return null;
            }
        }

        var opponent = _opponents[_gameIndex];
        opponent.gameObject.SetActive(true);
        SimulateOtherMatches();
        _gameIndex += 1;
        _seasonStatus = SeasonStatus.INPROGRESS;
        _gamesPlayedThisSeason += 1;
        UIManager.Instance.UpdateSeasonGamesInfo($"Division Game {_gamesPlayedThisSeason} / {GamesPerSeason()}");

        CheckSeasonLoop();

        return opponent;
    }

    void CheckSeasonLoop()
    {
        if (_gameIndex >= _opponents.Count && _currentLoopCount < _loopsPerSeason)
        {
            _gameIndex = 0;
            _currentLoopCount += 1;
        }
    }
    public int GetGamesRemaining()
    {
        return GamesPerSeason() - _gamesPlayedThisSeason;
    }

    public bool PlayerMadeFinals()
    {
        var players = GetPlayers();
        return players[0]._human || players[1]._human;
    }

    PlayerController GetFinalsOpponent()
    {
        var players = GetPlayers();

        if (!players[0]._human)
        {
            return SearchOpponents(players[0]);
        } else
        {
            return SearchOpponents(players[1]);
        }
    }

    PlayerController SearchOpponents(PlayerSO player)
    {
        PlayerController opp = null;

        foreach (var opponent in _opponents)
        {
            if (opponent._playerSO._name == player._name)
            {
                opp = opponent;
                break;
            }   
        }

        return opp;
    }



    //private void Awake()
    //{
    //    if (Instance != null && Instance != this)
    //    {
    //        Destroy(this);
    //    }
    //    else
    //    {
    //        Instance = this;
    //    }
    //}
}
