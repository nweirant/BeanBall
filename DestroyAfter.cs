using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float _time = 0.3f;

    private void Start()
    {
        Invoke(nameof(DestroyGO), _time);
    }

    void DestroyGO()
    {
        Destroy(gameObject);
    }
}
