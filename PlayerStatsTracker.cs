using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsTracker : MonoBehaviour
{
    private PlayerGameStats _stats;
    public PlayerName _playerName;

    private void Start()
    {
        _stats = new PlayerGameStats();
        _stats.PlayerName = _playerName;
    }


    public PlayerGameStats GetGameStats()
    {
        return _stats;
    }

    public void RecordGoalScored()
    {
        _stats.GoalsScored += 1;
    }

    public void RecordShot()
    {
        _stats.Shots += 1;
    }

    public void RecordMiss()
    {
        _stats.Misses += 1;
    }

    public void RecordSave()
    {
        _stats.Saves += 1;
    }

    public void RecordGoalAgainst()
    {
        _stats.GoalsAgainst += 1;
    }
}
