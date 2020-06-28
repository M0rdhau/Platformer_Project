using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform followingCamera;
    [SerializeField]float camSpeedX = 0.8f;
    [SerializeField] float camSpeedY = 0.8f;

    Vector3 actualCamPos;
    private float texUnitSize = 0f;


    private void Start()
    {
        actualCamPos = followingCamera.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D tex = sprite.texture;
        texUnitSize = tex.width / sprite.pixelsPerUnit;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 deltaCam = followingCamera.position - actualCamPos;
        transform.position += new Vector3(deltaCam.x * camSpeedX, deltaCam.y*camSpeedY, 0);
        actualCamPos = followingCamera.position;

        if (Mathf.Abs(followingCamera.position.x - transform.position.x) >= texUnitSize)
        {
            float offsetX = (followingCamera.position.x - transform.position.x) % texUnitSize;
            transform.position = new Vector3(followingCamera.position.x + offsetX, transform.position.y);
        }
    }
}
