using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUController : MonoBehaviour
{
    public ICPUController _controller;

    public void OnThrow()
    {
        if(_controller != null)
            _controller.OnThrow();
    }
}
