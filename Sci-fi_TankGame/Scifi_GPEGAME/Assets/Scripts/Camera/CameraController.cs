using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    
    public Transform target;

    
    public Vector3 offsetPosition;

    
    public Space offsetPositionSpace = Space.Self;

   
    public bool lookAt = true;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        transform.position = target.transform.position + offsetPosition;
    }

    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Missing target reference!", this);

            return;
        }

        // compute position
        if (offsetPositionSpace == Space.Self)
        {
            transform.position = target.TransformPoint(offsetPosition);
        }
        else
        {
            transform.position = target.position + offsetPosition;
        }

        // compute rotation
        if (lookAt)
        {
            transform.LookAt(target);
        }
        else
        {
            transform.rotation = target.rotation;
        }
    }
}