using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDManager : MonoBehaviour
{
    public Slider _chargeUpSlider;

    public void ShowChargeSlider(bool show = true)
    {
        _chargeUpSlider.gameObject.SetActive(show);
    }

    public void UpdateChargeSlider(float value)
    {
        _chargeUpSlider.value = value;
        if (_chargeUpSlider.value <= _chargeUpSlider.minValue)
            ShowChargeSlider(false);
    }

    public void SetUp(float max, float min)
    {
        _chargeUpSlider.maxValue = max;
        _chargeUpSlider.minValue = min;
        _chargeUpSlider.value = min;
    }

    void Start()
    {
        ShowChargeSlider(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
