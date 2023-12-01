using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaugeManager : MonoBehaviour
{
    public static LeaugeManager Instance;
    public SeasonManager[] _seasons;
    SeasonManager _currentSeason;
    int _seasonIndex;

    private void Start()
    {
        //Invoke(nameof(StartNextSeason), 2f);
    }

    public int GetSeasonNumber()
    {
        return _seasonIndex + 1;
    }

    public void StartSeasonOne()
    {
        AudioManager.Instance.Play("song-1");
        Invoke(nameof(StartNextSeason), 0.2f);
        AudioManager.Instance.Pause("practice");
    }

    public SeasonManager StartNextSeason()
    {
        return StartSeason(_seasonIndex);
    }

    public void EndSeason(bool advanced)
    {
        if (advanced)
            _seasonIndex += 1;

        UIManager.Instance.UpdateDivisionTitleText(_seasonIndex + 1);

        Invoke(nameof(StartNextSeason), 0.2f);

    }

    public SeasonManager StartSeason(int i)
    {
        UIManager.Instance.EnablePracticeUI(false);
        _currentSeason = _seasons[i];
        _seasonIndex = i;
        _currentSeason.gameObject.SetActive(true);
        _currentSeason.SetUp();
        GameEvents.Instance.SeasonStart(_currentSeason);
        return _currentSeason;
    }

    public SeasonManager GetCurrentSeason()
    {
        return _currentSeason;
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
