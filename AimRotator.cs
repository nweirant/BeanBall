using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class AimRotator : MonoBehaviour
{
    public float _baseRotatorSpeed;
    private float _rotationAdjusted = 1;
    public int _min = 5;
    public int _max = -185;

    public void AdjustRotatorSpeed(float amount)
    {
        _rotationAdjusted = amount;
    }

    void Update()
    {
        var rotationSpeed = _rotationAdjusted * _baseRotatorSpeed;
        float angle = Mathf.PingPong(Time.time * rotationSpeed, _max - _min) + _min;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
