using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour, ISaveable, IObserver
{
    [SerializeField]List<Upgrade.UpgradeType> upgrades = new List<Upgrade.UpgradeType>();

    public object CaptureState()
    {
        return upgrades;
    }

    public void RestoreState(object state)
    {
        upgrades = (List<Upgrade.UpgradeType>)state;
    }

    //public void SetUpgrade(Upgrade.UpgradeType type)
    //{
    //    if (!upgrades.Contains(type))
    //    {
    //        upgrades.Add(type);
    //    }
    //}

    public bool HasUpgrade(Upgrade.UpgradeType type)
    {
        return upgrades.Contains(type);
    }

    public void ReceiveUpdate(ISubject subject)
    {
        Upgrade up = (subject as Upgrade);

        if (!upgrades.Contains(up.GetUpgradeType()))
        {
            upgrades.Add(up.GetUpgradeType());
        }
        
    }
}
