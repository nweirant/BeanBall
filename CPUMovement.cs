using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUMovement : MonoBehaviour
{
    public int _speed;
    public Transform _start;
    public Transform _end;

    private Transform _target;

    private void Start()
    {
        _target = _start;
    }

    private void Update()
    {
        // Move towards the target
        if(_target == _start)
            transform.Translate(Vector2.up * _speed * Time.deltaTime);

        if (_target == _end)
            transform.Translate(-Vector2.up * _speed * Time.deltaTime);

        // Check if the enemy has reached the current target
        if (Vector2.Distance(transform.position, _target.position) < 0.1f)
        {
            // Switch the target to the other point
            _target = (_target == _start) ? _end : _start;
        }
    }
}
