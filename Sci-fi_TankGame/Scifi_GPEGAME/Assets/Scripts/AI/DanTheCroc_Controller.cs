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

                if (Time.time >= stateStartTime + investigateWait)
                {
                    ChangeState(AI_STATES.Patrol);
                }

                if (seesPlayer == true)
                {
                    ChangeState(AI_STATES.Chase);
                }
                break;
            case AI_STATES.Chase:
                Chase();

                if (Vector3.Distance(transform.position, chaseTarget.transform.position) > sightDistance)
                {
                    seesPlayer = false;
                    ChangeState(AI_STATES.Investigate);
                }

                if (Vector3.Distance(transform.position, chaseTarget.transform.position) < 5)
                {
                    ChangeState(AI_STATES.Attack);
                }
                break;
            case AI_STATES.Attack:
                Attack();

                if (seesPlayer == false)
                {
                    ChangeState(AI_STATES.Investigate);
                }

                if (Vector3.Distance(transform.position, chaseTarget.transform.position) > sightDistance)
                {
                    ChangeState(AI_STATES.Chase);
                }

                if (Time.time > stateStartTime + pawn.shootWait)
                {
                    if (Vector3.Distance(transform.position, chaseTarget.transform.position) < 5)
                    {
                        ChangeState(AI_STATES.Attack);
                    }
                }
                break;
        }

    }
}
