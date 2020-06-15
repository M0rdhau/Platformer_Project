using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] GameObject background;
    [SerializeField] GameObject[] firstForeground;
    [SerializeField] GameObject[] secondForeground;

    float offsetFirst = 12f;
    float offsetSecond = 24f;

    float camSpeed = 0.8f;

    float yOffsetFirst = -3f;
    float yOffsetSecond = -1f;


    // Update is called once per frame
    void Update()
    {
        Vector2 actualCamPos = camera.transform.position;
        Vector2 camPos = camera.transform.position;
        background.transform.position = actualCamPos;
        camPos.x *= camSpeed;
        camPos.y += yOffsetFirst;
        foreach (GameObject bg in firstForeground)
        {
            bg.transform.position = camPos;
            if (bg.transform.position.x < actualCamPos.x - offsetFirst)
            {
                var newPos = actualCamPos;
                actualCamPos.x += offsetFirst;
                bg.transform.position = newPos;
            }
            else if (bg.transform.position.x > actualCamPos.x + offsetFirst)
            {
                var newPos = actualCamPos;
                actualCamPos.x -= offsetFirst;
                bg.transform.position = newPos;
            }
        }
        camPos.x *= camSpeed;
        camPos.y += yOffsetSecond;
        foreach (GameObject bg in secondForeground)
        {
            bg.transform.position = camPos;
        }

    }
}
