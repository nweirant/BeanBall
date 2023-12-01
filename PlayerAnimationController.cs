using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public string _currentState;
    public string _previousState;
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string state)
    {
        if (_currentState == state)
            return;

        _anim.Play(state);
        _currentState = state;
    }

    public void PlayIdle()
    {
        ChangeAnimationState(AnimationStates.Idle);
    }

    public void PlayHoldingAnimation()
    {
        ChangeAnimationState(AnimationStates.Holding);
    }
}
