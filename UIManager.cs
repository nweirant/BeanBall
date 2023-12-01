using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public TextMeshPro _playerOneScore;
    public TextMeshPro _playerTwoScore;
    public TextMeshPro _timer;
    public TextMeshPro _playerName;
    public TextMeshPro _playerRecord;
    public TextMeshPro _oppName;
    public TextMeshPro _oppRecord;
    public TextMeshProUGUI _seasonGameInfo;

    [Header("Season Stats")]
    public TextMeshProUGUI _standingsStatus;

    public GameObject _seasonStats;

    [Header("Announcer UI")]
    public TextMeshProUGUI _announcerText;
    public Animator _announcerAnim;

    [Header("End Of Season Screen")]
    public GameObject _endOfSeasonScreen;
    public TextMeshProUGUI _endOfSeasonResultText;
    public TextMeshProUGUI _endOfSeasonButtonText;
    public TextMeshProUGUI _endOfSeasonStatsText;
    public TextMeshProUGUI _divisionNumberText;

    public GameObject _blurrBackground;
    public TextMeshProUGUI _divisionTitleText;
    public GameObject _championshipBanner;

    public TextMeshProUGUI[] _playerStandingsText;

    public GameObject _practiceUI;
    public GameObject _mainMenu;

    private void Start()
    {
        ResetScore();
        EnableMainMenuUI();
        _blurrBackground.SetActive(false);
        _championshipBanner.SetActive(false);
    }

    public void DisplayMessage(string msg)
    {
        _announcerText.text = msg;
        _announcerAnim.Play("show");
    }

    public void EnablePracticeUI(bool enabled = true)
    {
        _practiceUI.SetActive(enabled);
    }

    public void EnableMainMenuUI(bool enabled = true)
    {
        _mainMenu.SetActive(enabled);
    }

    public void UpdateDivisionTitleText(int division)
    {
        _divisionTitleText.text = $"Division {division}";
    }

    public void ShowEndOfSeasonScreen(bool show = true)
    {
        _blurrBackground.SetActive(show);
        _endOfSeasonScreen.SetActive(show);

    }
    public void PopulateEndOfSeasonScreen(bool advanced, PlayerSO player)
    {
        var seasonNumber = LeaugeManager.Instance.GetSeasonNumber();

        if (advanced && seasonNumber == 3)
        {
            _endOfSeasonResultText.text = "You Finished 1st! \n You are the Overall Champion!!";
            _endOfSeasonButtonText.text = "Exit Game";
        }
        else if (advanced)
        {
            if(LeaugeManager.Instance.GetSeasonNumber() >= 2)
            {
                _endOfSeasonResultText.text = "You Finished 1st! \n Advance to the FINAL division.";

            }
            else
            {
                _endOfSeasonResultText.text = "You Finished 1st! \n Advance to the next division.";
            }
            _endOfSeasonButtonText.text = "Advance to next Division!";
        }
        else
        {
            _endOfSeasonResultText.text = "You did not qualify..";
            _endOfSeasonButtonText.text = "Try Again!";
        }

        _divisionNumberText.text = $"Division {seasonNumber} / 3";
        var stats = player.GetStats();
        _endOfSeasonStatsText.text = $"Your Stats\n Record: {player.GetRecord()}\n Goals Scored: {stats.GoalsScored}\n Goals Against: {stats.GoalsAgainst}" +
            $"\n Shots: {stats.Shots}\n";
        ShowEndOfSeasonScreen();
    }

    public void UpdateStandings(List<PlayerSO> players)
    {
        var i = 0;
        foreach (var p in players)
        {
            var stats = p.GetStats();

            if(p._human)
            {
                _playerStandingsText[i].text = $"* {p._name}: {stats.Wins} - {stats.Losses} (Goals: {stats.GoalsScored})";
            }
            else
            {
                _playerStandingsText[i].text = $"{p._name}: {stats.Wins} - {stats.Losses} (Goals: {stats.GoalsScored})";
            }
            i++;
        }
    }

    public void UpdateSeasonGamesInfo(string info)
    {
        _seasonGameInfo.text = info;
    }

    public void ShowBanner(bool enabled)
    {
        _championshipBanner.SetActive(enabled);
    }

    public void ShowPlayerRecords(PlayerSO playerOne, PlayerSO playerTwo)
    {
        _playerName.text = playerOne._name;
        _playerRecord.text = playerOne.GetRecord();

        _oppName.text = playerTwo._name;
        _oppRecord.text = playerTwo.GetRecord();
    }

    public void ResetScore()
    {
        _playerOneScore.text = "0";
        _playerTwoScore.text = "0";
        _playerName.text = "";
        _playerRecord.text = "";
        _oppName.text = "";
        _oppRecord.text = "";
    }

    public void UpdateStandingsStatus(float gamesRemaining, string nextOpponentName)
    {
        if(gamesRemaining == 0)
            _standingsStatus.text = $"Congrats you made the final! \nFinals Opponent: {nextOpponentName}";
        //else if(gamesRemaining == 1)
        //    _standingsStatus.text = $"Last game. \nNext Opponent: {nextOpponentName}";
        else
            _standingsStatus.text = $"{gamesRemaining} Games Remaining \nNext Opponent: {nextOpponentName}";
    }

    public void DisplayStandings(List<PlayerSO> players = null, bool enabled = true)
    {
        if(enabled || players != null)
            UpdateStandings(players);

        _seasonStats.SetActive(enabled);
        _blurrBackground.SetActive(enabled);
    }

    public void UpdateScore(PlayerName _player, int score)
    {
        if(_player == PlayerName.PLAYERONE)
        {
            _playerOneScore.text = score.ToString();
        } else
        {
            _playerTwoScore.text = score.ToString();
        }
    }

    public void UpdateTimer(string time)
    {
        _timer.text = time;
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
