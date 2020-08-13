using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class PlayerUIHandler : MonoBehaviour
{
    float health;
    float charge;

    ResourceController _contr;
    PlayerHealth pHealth;
    CombatCharge pCharge;

    private void Start()
    {
        _contr = FindObjectOfType<ResourceController>();
    }

    private void Awake()
    {
        pHealth = FindObjectOfType<PlayerHealth>();
        pCharge = FindObjectOfType<CombatCharge>();
    }

    

    public void UpdateHealth(float health)
    {
        this.health = health;
        if (_contr == null)
        {
            _contr = FindObjectOfType<ResourceController>();
        }
        _contr.SetHealth(health / pHealth.GetMaxHealth());
        //healthText.text = "HP: " + this.health;
    }

    public void UpdateCharge(float charge)
    {
        this.charge = charge;
        if (_contr == null)
        {
            _contr = FindObjectOfType<ResourceController>();
        }
        _contr.SetCharge(pCharge.GetCharge());
        //chargeText.text = "Charge: " + this.charge*100;
    }
}
