using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField playerNameInputfield;

    public void Play()
    {
        GameCoordinator.Instance.StartPractice();
        UIManager.Instance.EnableMainMenuUI(false);
        SetPlayerName();
    }

    public void Exit()
    {
        Application.Quit();
    }


    public void SetPlayerName()
    {
        var inputName = playerNameInputfield.text;
        //PlayerPrefs.SetString("PlayerName", inputName);
        PlayerSO player = Resources.Load("Players/Human") as PlayerSO;
        if (inputName != "")
        {
            player._name = playerNameInputfield.text;
        }
        else
        {
            player._name = "Bean";
        }
    }
}
