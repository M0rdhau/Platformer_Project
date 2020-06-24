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
    //how much time should the charge be kept at 100%
    [SerializeField] float maxChargeTime = 5f;

    PlayerUIHandler handler;
    SpriteRenderer _renderer;

    //shows if charge decrementation started
    bool isDecreasing = false;

    float maxCharge = 1f;
    [SerializeField] float currentCharge = 0f;
    Color color;

    private void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        handler = GetComponent<PlayerUIHandler>();
        handler.UpdateCharge(currentCharge);
        color = _renderer.material.GetColor("GlowColor");
        UpdateRenderer();
    }

    public void AddCharge(float dmg)
    {
        currentCharge = Mathf.Clamp(currentCharge + dmg / maxDamage, 0, maxCharge);
        handler.UpdateCharge(currentCharge);
        UpdateRenderer();
        if (!isDecreasing) StartCoroutine(DecreaseCharge());
    }


    private IEnumerator DecreaseCharge()
    {
        isDecreasing = true;
        if (currentCharge == maxCharge)
        {
            yield return new WaitForSeconds(maxChargeTime);
            currentCharge = Mathf.Clamp(currentCharge - Mathf.Epsilon, 0, 1f);
        }
        while (currentCharge > 0)
        {
            currentCharge = Mathf.Clamp(currentCharge - chargeDecreaseRate, 0, 1f);
            handler.UpdateCharge(currentCharge);
            UpdateRenderer();
            yield return new WaitForSeconds(chargeDecreaseTime);
        }
    }

    private void UpdateRenderer()
    {
        float factor = currentCharge * maxGlow + 0.1f;
        Color newColor = new Color(color.r*factor, color.g * factor, color.b * factor, color.a * factor);
        _renderer.material.SetColor("GlowColor", newColor);
    }

    public float GetCharge() { return currentCharge; }

    public object CaptureState()
    {
        return currentCharge;
    }

    public void RestoreState(object state)
    {
        currentCharge = (float)state;
        UpdateRenderer();
    }
}
