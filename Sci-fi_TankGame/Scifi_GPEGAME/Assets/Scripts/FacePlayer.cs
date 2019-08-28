using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    private Camera player1;



    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player1.transform);

    }
}
