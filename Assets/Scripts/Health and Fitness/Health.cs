using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Health
{
    void DamageHealth(float dmg);

    bool IsDead();

    void Die();

    void KnockBackHit(float dmg, bool knockedRight);

    float GetHealth();

    float GetMaxHealth();

}
