using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public PawnData data;

    Vector3 offset;

    public float rotateSpeed = 2f;

    void Start()
    {
        offset = transform.position - data.transform.position;
    }
    void Update()
    {
        transform.position = data.transform.position + offset;


        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Horizontal") * rotateSpeed, Vector3.up);

        offset = camTurnAngle * offset;

        transform.LookAt(data.transform);
    }
}