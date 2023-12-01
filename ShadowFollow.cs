using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowFollow : MonoBehaviour
{
    public Transform _target;
    SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void ShowShadow(bool show = true)
    {
        _sprite.enabled = show;
    }

    void Update()
    {
        if(transform.position.x < -95 || transform.position.x > 95)
        {
            ShowShadow(false);
        }
        else
        {
            ShowShadow(true);
        }
        transform.position = new Vector3(_target.position.x, transform.position.y);
    }
}
