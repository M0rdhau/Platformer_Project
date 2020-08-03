using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour, ISubject
{
    public enum UpgradeType
    {
        Meditation,
        DoubleJump,
        FireFist,
        Shadow
    }

    [SerializeField] UpgradeType type;
    [SerializeField] string message;
    private bool isPickedUp = false;
    Animator anim;

    private List<IObserver> upgradeObservers = new List<IObserver>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        Attach(FindObjectOfType<Alert>());
        Attach(FindObjectOfType<PlayerUpgrades>());
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        if (!isPickedUp)
        {
            ProcessUpgrade(collision.gameObject);
        }
    }

    private void ProcessUpgrade(GameObject player)
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            isPickedUp = true;
            GetComponent<ParticleSystem>().Play();
            //GetComponentInChildren<ParticleSystem>().Stop();
            Notify();
        }
    }

    public string GetMessage()
    {
        return message;
    }

    public bool GetIsPicked()
    {
        return isPickedUp;
    }

    public UpgradeType GetUpgradeType()
    {
        return type;
    }

    public void Attach(IObserver observer)
    {
        this.upgradeObservers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this.upgradeObservers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in upgradeObservers)
        {
            observer.ReceiveUpdate(this);
        }
    }
}
