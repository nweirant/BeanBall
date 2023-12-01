using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalBallFollow : MonoBehaviour, ICPUMovement
{
    public float _speed;
    public Transform _target;
    public int _maxFollow;
    public int _minFollow;

    private bool _follow = true;

    public void FollowBall(bool follow)
    {
        _follow = follow;
    }

    void Update()
    {
        if (_target.position.y >= _minFollow && _target.position.y <= _maxFollow && _follow)
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, _target.position.y), _speed * Time.deltaTime); 
    }
}
