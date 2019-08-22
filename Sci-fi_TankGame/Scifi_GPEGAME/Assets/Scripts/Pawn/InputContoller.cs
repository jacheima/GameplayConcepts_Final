using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputContoller : MonoBehaviour
{
    public PawnData data;

    void Update()
    {
        //Tell the mover that I am not moving
        Vector3 directionToMove = Vector3.zero;

        //if the player presses W
        if (Input.GetKey(KeyCode.W))
        {
            //tell the move to move in a forward direction
            directionToMove += Vector3.forward;
        }

        //if the player presses S
        if (Input.GetKey(KeyCode.S))
        {
            //tell the move to move in a backward direction
            directionToMove -= Vector3.forward;
        }

        //if the player presses A
        if (Input.GetKey(KeyCode.A))
        {
            //tell the mover to rotate in a leftward direction
            data.mover.Rotate(-data.rotateSpeed * Time.deltaTime);
        }

        //if the player presses D
        if (Input.GetKey(KeyCode.D))
        {
            //tell the move to rotate in a rightward direction
            data.mover.Rotate(data.rotateSpeed * Time.deltaTime);
        }

        //if the player presses space bar


        data.mover.Move(directionToMove);

    }
}
