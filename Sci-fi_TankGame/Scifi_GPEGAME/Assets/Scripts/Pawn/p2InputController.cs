using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class p2InputController : MonoBehaviour
{
    public PawnData data;

    // Update is called once per frame
    void Update()
    {
        //Tell the mover that I am not moving
        Vector3 directionToMove = Vector3.zero;

        //if the player presses W
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //tell the move to move in a forward direction
            directionToMove += Vector3.forward;
        }

        //if the player presses S
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //tell the move to move in a backward direction
            directionToMove -= Vector3.forward;
        }

        //if the player presses A
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            //tell the mover to rotate in a leftward direction
            data.mover.Rotate(-data.rotateSpeed * Time.deltaTime);
        }

        //if the player presses D
        if (Input.GetKey(KeyCode.RightArrow))
        {
            //tell the move to rotate in a rightward direction
            data.mover.Rotate(data.rotateSpeed * Time.deltaTime);
        }

        //if the player presses space bar
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            GameObject bullet =
                Instantiate(data.bulletPrefab, data.bulletSpawn.transform.position, data.bulletSpawn.transform.rotation);

            bulletMover bm = bullet.GetComponent<bulletMover>();

            bm.SetPlayer(data);
        }


        data.mover.Move(directionToMove);
    }
}
