using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnMover : MonoBehaviour
{
    public PawnData data;
    public CharacterController cc;

    void Start()
    {
        data = GetComponent<PawnData>();
        cc = GetComponent<CharacterController>();
    }
    

    public void Move(Vector3 worldDirection)
    {
        //calculate the direction based on the rotation
        Vector3 directionToMove = data.transform.TransformDirection(worldDirection);
        //now move
        cc.SimpleMove(directionToMove * data.moveSpeed);
    }

    public void Rotate(float direction)
    {
        data.transform.Rotate(new Vector3(0f, direction * data.rotateSpeed * Time.deltaTime, 0f));
    }

    public void RotateTowards(Vector3 lookVector)
    {
        //Find the vector to the target
        Vector3 vectorToTarget = lookVector;

        //Find the quaternion to look down that vector
        Quaternion targetQuaternion = Quaternion.LookRotation(vectorToTarget, Vector3.up);

        //Set the rotation to partway towards that quaternion
        data.transform.rotation = Quaternion.RotateTowards(data.transform.rotation, targetQuaternion, data.rotateSpeed * Time.deltaTime);
    }
}
