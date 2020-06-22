using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerUIHandler : MonoBehaviour
{
    float health;
    float charge;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] TextMeshProUGUI chargeText;


    public void UpdateHealth(float health)
    {
        this.health = health;
        healthText.text = "HP: " + this.health;
    }

    public void UpdateCharge(float charge)
    {
        this.charge = charge;
        chargeText.text = "Charge: " + this.charge*100;
    }
}
