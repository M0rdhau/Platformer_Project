using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform camera;
    [SerializeField]float camSpeedX = 0.8f;
    [SerializeField] float camSpeedY = 0.8f;

    Vector3 actualCamPos;
    private float texUnitSize = 0f;


    private void Start()
    {
        actualCamPos = camera.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D tex = sprite.texture;
        texUnitSize = tex.width / sprite.pixelsPerUnit;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 deltaCam = camera.position - actualCamPos;
        transform.position += new Vector3(deltaCam.x * camSpeedX, deltaCam.y*camSpeedY, 0);
        actualCamPos = camera.position;

        if (Mathf.Abs(camera.position.x - transform.position.x) >= texUnitSize)
        {
            float offsetX = (camera.position.x - transform.position.x) % texUnitSize;
            transform.position = new Vector3(camera.position.x + offsetX, transform.position.y);
        }
    }
}
