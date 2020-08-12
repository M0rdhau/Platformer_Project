using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceController : MonoBehaviour
{

    Slider healthSlider;
    Slider chargeSlider;


    private void Start()
    {
        healthSlider = transform.Find("HealthSlider").GetComponent<Slider>();
        chargeSlider = transform.Find("ChargeSlider").GetComponent<Slider>();
        Debug.Log(healthSlider);
        Debug.Log(chargeSlider);
    }

    public void SetHealth(float val)
    {
        if (healthSlider == null)
        {
            healthSlider = transform.Find("HealthSlider").GetComponent<Slider>();
        }
        healthSlider.value = val;
    }

    public void SetCharge(float val)
    {
        if (chargeSlider == null)
        {
            chargeSlider = transform.Find("ChargeSlider").GetComponent<Slider>();
        }
        chargeSlider.value = val;
    }

}
