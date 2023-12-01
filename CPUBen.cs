using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBen : MonoBehaviour, ICPUController
{

    VerticalBallFollow _movement;

    private void Start()
    {
        _movement = GetComponent<VerticalBallFollow>();
    }


    public void OnThrow()
    {
        StartCoroutine(PauseMovement());
    }

    private IEnumerator PauseMovement()
    {
        _movement.FollowBall(false);

        yield return new WaitForSeconds(2);

        _movement.FollowBall(true);
    }
}
