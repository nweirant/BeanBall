using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    public TMP_InputField playerNameInputfield;
    
    public void SetPlayerName()
    {
        PlayerPrefs.SetString("PlayerName", playerNameInputfield.text);
    }
}
