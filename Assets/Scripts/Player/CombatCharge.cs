using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//for now, full charge will just double the damage
public class CombatCharge : MonoBehaviour
{
    //what percent of max charge should you lose
    [SerializeField] float chargeDecreaseRate = 0.05f;
    //decrease charge every second
    [SerializeField] float chargeDecreaseTime = 1f;
    //how much damage should the player do before fully charged
    [SerializeField] float maxDamage = 60f;

    PlayerUIHandler handler;

    //shows if charge decrementation started
    bool isDecreasing = false;

    float maxCharge = 1f;
    float currentCharge = 0f;

    private void Start()
    {
        handler = GetComponent<PlayerUIHandler>();
        handler.UpdateCharge(currentCharge);
    }

    public void AddCharge(float dmg)
    {
        currentCharge = Mathf.Clamp(currentCharge + dmg / maxDamage, 0, maxCharge);
        handler.UpdateCharge(currentCharge);
        if (!isDecreasing) StartCoroutine(DecreaseCharge());
    }


    private IEnumerator DecreaseCharge()
    {
        isDecreasing = true;
        while (currentCharge > 0)
        {
            currentCharge = Mathf.Clamp(currentCharge - chargeDecreaseRate, 0, 1);
            handler.UpdateCharge(currentCharge);
            yield return new WaitForSeconds(chargeDecreaseTime);
        }
    }

    public float GetCharge() { return currentCharge; }
}
