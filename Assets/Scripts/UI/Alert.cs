using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Alert : MonoBehaviour, IObserver
{
    [SerializeField] float alertWaitTime = 3f;

    private GameObject alertSign;
    private TextMeshPro alertText;

    IEnumerator showAlert;

    private void Awake()
    {
        alertSign = transform.GetChild(0).gameObject;
        Debug.Log(alertSign.name);
        alertText = alertSign.transform.GetChild(0).GetComponent<TextMeshPro>();
        Debug.Log(alertText);
        alertSign.SetActive(false);
    }

    public void ReceiveUpdate(ISubject subject)
    {
        Upgrade up = subject as Upgrade;
        string message = up.GetMessage();
        if (showAlert != null) { StopCoroutine(showAlert); }
        showAlert = DisplayAlert(message);
        StartCoroutine(showAlert);
        //Debug.Log("You have picked up " + message + "!");
    }

    private IEnumerator DisplayAlert(string message)
    {
        alertText.text = "You have picked up " + message + "!";
        alertSign.SetActive(true);
        yield return new WaitForSeconds(alertWaitTime);
        alertSign.SetActive(false);
    }
}
