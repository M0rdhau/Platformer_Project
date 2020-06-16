using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField] GameObject firstForeground;
    [SerializeField] GameObject secondForeground;
    [SerializeField]float camSpeed = 0.8f;

    Vector3 actualCamPos;


    private void Start()
    {
        actualCamPos = camera.position;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 deltaCam = actualCamPos - camera.position;

    }
}
