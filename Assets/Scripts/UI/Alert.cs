using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour, IObserver
{
    public void ReceiveUpdate(ISubject subject)
    {
        Upgrade up = subject as Upgrade;
        string message = up.GetMessage();
        Debug.Log("You have picked up " + message + "!");
    }

}
