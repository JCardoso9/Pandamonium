using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.15f;
    public Vector2 cameraOffset;
    private Vector2 velocity = Vector2.zero;
    public GameObject sun;
    public Vector2 sunOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 desiredPosition = new Vector2(target.position.x + cameraOffset.x, target.position.y + cameraOffset.y);
        Vector2 smoothedPosition = Vector2.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        sun.transform.position = new Vector3(Camera.main.transform.position.x * 0.95f +20, Camera.main.transform.position.y * 0.8f -4 , sun.transform.position.z);
        
    }
}
