using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class DanTheCroc_Controller : AIController
{
    void Start()
    {
        currentWaypointIndex = 0;
        ChangeState(AI_STATES.Patrol);
    }

    void Update()
    {
        //switch depending on the current state of the AI
        switch (currentState)
        {
            case AI_STATES.Patrol:
                Patrol();
                break;
            case AI_STATES.Investigate:
                Investigate();
                break;
            case AI_STATES.Chase:
                Chase();
                break;
        }

    }
}
