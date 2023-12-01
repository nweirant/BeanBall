using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    public GameObject _env;

    public void SpawnEnv(bool spawn)
    {
        if(_env != null)
            _env.SetActive(spawn);
    }

    private void OnEnable()
    {
        SpawnEnv(true);
    }

    private void OnDisable()
    {
        SpawnEnv(false);
    }
}
