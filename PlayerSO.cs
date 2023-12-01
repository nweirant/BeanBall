using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerSO : ScriptableObject
{
    PlayerStats _stats = new PlayerStats();

    public string _name;
    public int _division = 3;
    public bool _human = false;

    public PlayerStats SaveGameStats(PlayerGameStats game)
    {
        if (game.Win)
            _stats.Wins += 1;
        else
            _stats.Losses += 1;
        _stats.Saves += game.Saves;
        _stats.Misses = _stats.Shots - _stats.GoalsScored;
        _stats.GoalsAgainst += game.GoalsAgainst;
        _stats.GoalsScored += game.GoalsScored;
        _stats.Shots += game.Shots;
        _stats.GamesPlayed += 1;

        return _stats;
    }

    public void AddToStats(PlayerGameStats stats)
    {
        _stats.Saves += stats.Saves;
        _stats.Misses += stats.Misses;
        _stats.GoalsAgainst += stats.GoalsAgainst;
        _stats.GoalsScored += stats.GoalsScored;
        _stats.Shots += stats.Shots;
    }

    public PlayerStats GetStats()
    {
        return _stats;
    }

    public int Rating()
    {
        return (_stats.Wins * 10) + _stats.GoalsScored;
    }

    public void ClearStats()
    {
        _stats = new PlayerStats();
    }

    public string GetRecord()
    {
        return $"{_stats.Wins}-{_stats.Losses}";
    }
}
