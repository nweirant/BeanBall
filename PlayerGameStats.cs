using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameStats 
{
    public PlayerName PlayerName { get; set; }
    public bool Win { get; set; }
    public int GoalsScored { get; set; } = 0;
    public int Shots { get; set; } = 0;
    public int Misses { get; set; } = 0;
    public int GoalsAgainst { get; set; } = 0;
    public int Saves { get; set; } = 0;
}
