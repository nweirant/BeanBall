using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class VerticalMovement : MonoBehaviour
{
    public float _speed;

    public float _topPos;
    public float _bottomPos;
    [SerializeField] private bool _movingUp;

    private void Start()
    {
        
    }

    void Update()
    {
        if(_movingUp)
        {
            transform.Translate(Vector2.up * _speed * Time.deltaTime);
            if (transform.position.y >= _topPos)
            {
                _movingUp = !_movingUp;
            }
        }
        else
        {
            transform.Translate(-Vector2.up * _speed * Time.deltaTime);
            if (transform.position.y <= _bottomPos)
            {
                _movingUp = !_movingUp;
            }
        }
    }
        //// Move towards the target
        //transform.position = Vector2.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        //// Check if the object has reached the current target
        //if (Vector2.Distance(transform.position, _target.position) < 0.1f)
        //{
        //    // Switch the target to the other point
        //    _target = (_target == _topPos) ? _bottomPos : _topPos;

        //    // Flip the object's direction by flipping its scale on the X-axis
        //    //Vector3 newScale = transform.localScale;
        //    //newScale.x *= -1;
        //    //transform.localScale = newScale;
        //}
    
}
