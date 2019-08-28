using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public PawnData pawn;

    public enum PATROL_TYPES
    {
        Loop,
        Stop,
        PingPong,
        Random
    };

    public enum AI_STATES
    {
        Patrol,
        Chase,
        Investigate,
        Attack
    };

    public enum AI_AVOID_STATES
    {
        None,
        TurnToAvoid,
        MoveToAvoid
    };

    //Variables for patroling
    public PATROL_TYPES patrolType;
    public List<GameObject> waypoints;
    public int currentWaypointIndex;

    //Variables for Chasing
    public float chaseSpeed;
    public GameObject chaseTarget;

    //Variables for Investigating
    private Vector3 investigateSpot;
    public float investigateWait = 5;

    //Variables for Sight
    public float heightMultiplier = 1.36f;
    public float sightDistance = 10;



    public AI_STATES currentState;
    public AI_AVOID_STATES currentAvoidState = AI_AVOID_STATES.None;

    

    public float cutoff;
    public float stateStartTime;
    public float feelerDistance;
    public float avoidMoveTime;
    public float startAvoidTime;

    public bool isForward;

    public bool seesPlayer = false;

    public bulletMover bm;

    void Start()
    {
        
    }
    public void ChangeState(AI_STATES newState)
    {
        //Note the time I entered the state
        stateStartTime = Time.time;
        currentState = newState;

        //Reset the avoidance state
        currentAvoidState = AI_AVOID_STATES.None;
    }

    public void ChangeAvoidState(AI_AVOID_STATES newState)
    {
        startAvoidTime = Time.time;
        currentAvoidState = newState;
    }

    public void Idle()
    {
        //Do nothing;
    }

    public bool isBlocked()
    {
        if (Physics.Raycast(pawn.transform.position, pawn.transform.forward, feelerDistance))
        {
            return true;
        }

        return false;
    }

    public void Seek(Transform target)
    {
        switch (currentAvoidState)
        {
            case AI_AVOID_STATES.None:
                //Do Chase
                Vector3 targetVector = (target.position - pawn.transform.position).normalized;
                pawn.mover.RotateTowards(targetVector);
                pawn.mover.Move(Vector3.forward);

                //if blocked
                if (isBlocked())
                {
                    //Change to TurnToAvoid
                    ChangeAvoidState(AI_AVOID_STATES.TurnToAvoid);
                }
                break;
            case AI_AVOID_STATES.TurnToAvoid:
                //Rotate
                pawn.mover.Rotate(1);

                if (!isBlocked())
                {
                    ChangeAvoidState(AI_AVOID_STATES.MoveToAvoid);
                }
                break;
            case AI_AVOID_STATES.MoveToAvoid:
                // Move forward
                pawn.mover.Move(Vector3.forward);
                // if blocked
                if (isBlocked())
                {
                    ChangeAvoidState(AI_AVOID_STATES.TurnToAvoid);
                }
                // If time avoiding is up
                if (Time.time > startAvoidTime + avoidMoveTime)
                {
                    ChangeAvoidState(AI_AVOID_STATES.None);
                }
                break;
        }
    }

    public void SeekPoint(Vector3 targetPoint)
    {
        //Find vector to target
        Vector3 targetVector = (targetPoint - pawn.transform.position).normalized;
        //Rotate towards it
        pawn.mover.RotateTowards(targetVector);
        //move forward
        pawn.mover.Move(Vector3.forward);
    }

    public void Flee(Transform target)
    {
        //find vector to target
        Vector3 targetVector = (target.position - pawn.transform.position);
        //find the oposite of the vector
        Vector3 awayVector = -targetVector;
        //rotate towards the opposite vector of your target
        pawn.mover.RotateTowards(awayVector);
        //move forward
        pawn.mover.Move(Vector3.forward);
    }

    public void Patrol()
    {
        //Seek the current waypoint
        Seek(waypoints[currentWaypointIndex].transform);

        //If it is close enough to the waypoint
        if(Vector3.Distance(pawn.transform.position, waypoints[currentWaypointIndex].transform.position) <= cutoff)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Count || currentWaypointIndex < 0)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    public void Investigate()
    {
        RaycastHit hit;

        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, transform.forward * sightDistance, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized * sightDistance, Color.red);
        Debug.DrawRay(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized * sightDistance, Color.red);

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, transform.forward, out hit,
            sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player1" || hit.collider.gameObject.tag == "Player2")
            {
                ChangeState(AI_STATES.Chase);
                chaseTarget = hit.collider.gameObject;
                seesPlayer = true;
            }
            
        }

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward + transform.right).normalized, out hit,
            sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player1" || hit.collider.gameObject.tag == "Player2")
            {
                ChangeState(AI_STATES.Chase);
                chaseTarget = hit.collider.gameObject;
                seesPlayer = true;
            }
            
        }

        if (Physics.Raycast(transform.position + Vector3.up * heightMultiplier, (transform.forward - transform.right).normalized, out hit,
            sightDistance))
        {
            if (hit.collider.gameObject.tag == "Player1" || hit.collider.gameObject.tag == "Player2")
            {
                ChangeState(AI_STATES.Chase);
                chaseTarget = hit.collider.gameObject;
                seesPlayer = true;
            }
           
        }

        transform.position = transform.position;
        pawn.mover.Move(Vector3.zero);

        transform.LookAt(investigateSpot);


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player1" || other.tag == "Player2")
        {
            ChangeState(AI_STATES.Investigate);
            investigateSpot = other.gameObject.transform.position;
        }
    }

    public void Chase()
    {
        if (seesPlayer == true)
        {
            Seek(chaseTarget.transform);

        }
        
    }

    public void Attack()
    {
        transform.position = transform.position;
        pawn.mover.Move(Vector3.zero);
        transform.LookAt(chaseTarget.transform);

        if (Time.time > stateStartTime + pawn.shootWait)
        {
            GameObject bullet = Instantiate(pawn.bulletPrefab, pawn.bulletSpawn.transform.position,
                pawn.bulletSpawn.transform.rotation);

            bulletMover bm = bullet.GetComponent<bulletMover>();

            bm.SetPlayer(pawn);
        }
        
    }


}
