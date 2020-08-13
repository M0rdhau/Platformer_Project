using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for now, full charge will just double the damage
public class CombatCharge : MonoBehaviour, ISaveable
{
    //what percent of max charge should you lose
    [SerializeField] float chargeDecreaseRate = 0.05f;
    //decrease charge every second
    [SerializeField] float chargeDecreaseTime = 1f;
    [SerializeField] float maxGlow = 3.5f;
    //how much damage should the player do before fully charged
    [SerializeField] float maxDamage = 60f;
    
    [SerializeField] float chargeGracePeriod = 2f;

    PlayerUIHandler handler;
    SpriteRenderer _renderer;

    //shows if charge decrementation started
    bool isDecreasing = false;

    float maxCharge = 1f;
    [SerializeField] float currentCharge = 0f;
    Color color;

    IEnumerator Charging;

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        handler = GetComponent<PlayerUIHandler>();
        color = _renderer.material.GetColor("GlowColor");
        UpdateCharge();
    }

    public void AddCharge(float dmg, bool isChargingFist = false)
    {
        currentCharge = Mathf.Clamp(currentCharge + dmg / maxDamage, 0, maxCharge);
        UpdateCharge();
        if (
            //!isDecreasing &&
            !isChargingFist)
        {
            if (Charging != null) { StopCoroutine(Charging); }
            Charging = DecreaseCharge();
            StartCoroutine(Charging);
        }
    }


    private IEnumerator DecreaseCharge()
    {
        isDecreasing = true;
        yield return new WaitForSeconds(chargeGracePeriod);

        while (currentCharge > 0)
        {
            currentCharge = Mathf.Clamp(currentCharge - chargeDecreaseRate, 0, 1f);
            UpdateCharge();
            if (currentCharge == 0) isDecreasing = false;
            yield return new WaitForSeconds(chargeDecreaseTime);
        }
    }

    public void ResetCharge(float charge)
    {
        currentCharge = charge;
        UpdateCharge();
    }

    private void UpdateCharge()
    {
        float factor = currentCharge * maxGlow + 0.1f;
        Color newColor = new Color(color.r * factor, color.g * factor, color.b * factor, color.a * factor);
        handler.UpdateCharge(currentCharge);
        _renderer.material.SetColor("GlowColor", newColor);
    }

    public float GetCharge() { return currentCharge; }

    public float GetMaxDamage() { return maxDamage; }

    public object CaptureState()
    {
        return currentCharge;
    }

    public void RestoreState(object state)
    {
        currentCharge = (float)state;
        //UpdateCharge();
    }
}
