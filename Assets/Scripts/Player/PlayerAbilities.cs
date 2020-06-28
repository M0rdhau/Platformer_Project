using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] float meditationTickTime = 3f;
    [SerializeField] float meditationRestore = 1f;

    Animator _animator;
    Rigidbody2D _rbody;
    PlayerHealth health;
    CombatCharge charge;
    PlayerUpgrades upgrades;

    bool isMeditating = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rbody = GetComponent<Rigidbody2D>();
        health = GetComponent<PlayerHealth>();
        charge = GetComponent<CombatCharge>();
        upgrades = GetComponent<PlayerUpgrades>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMeditation();
    }

    private void HandleMeditation()
    {
        if (upgrades.HasUpgrade(Upgrade.UpgradeType.Meditation))
        {
            if (Input.GetKey(KeyCode.T))
            {
                if (!isMeditating && _animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") && charge.GetCharge() > 0) StartCoroutine(Meditation());
            }
            else if (isMeditating)
            {
                SetMeditation(false);
            }
        }
    }

    private void SetMeditation(bool med)
    {
        isMeditating = med;
        _animator.SetBool("isMeditating", isMeditating);
    }

    private IEnumerator Meditation()
    {
        SetMeditation(true);
        while (isMeditating && charge.GetCharge() > 0)
        {
            health.Heal(meditationRestore*(1 + charge.GetCharge()));
            yield return new WaitForSeconds(meditationTickTime);
        }
        SetMeditation(false);
    }
}
