using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{

    public static GameEvents Instance;

    public event Action onBallOut;
    public event Action onTimeExpired;
    public event Action<PlayerName> onPlayerScored;
    public event Action<SeasonManager> onSeasonStart;
    public event Action<Ball> onWarmupScored;

    public void BallOut()
    {
        if(onBallOut != null)
        {
            onBallOut();
        }
    }

    public void TimeExpired()
    {
        if (onTimeExpired != null)
        {
            onTimeExpired();
        }
    }

    public void WarmUpScored(Ball ball)
    {
        if (onWarmupScored != null)
        {
            onWarmupScored(ball);
        }
    }

    public void PlayerScored(PlayerName playerName)
    {
        if (onPlayerScored != null)
        {
            onPlayerScored(playerName);
        }
    }

    public void SeasonStart(SeasonManager season)
    {
        if (onSeasonStart != null)
        {
            onSeasonStart(season);
        }
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
