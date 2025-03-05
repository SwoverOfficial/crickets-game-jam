using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteDirectionalController spriteDirectionControllerScript;

    public Transform[] waypoints;
    public float[] turnDirections;
    public float speed = 10f;
    private int waypointIndex;

    public bool waitBeforeLooping;
    public float waitTimeBetweenLoops = 1f;
    private bool patrollingPaused;

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstructionMask;

    Vector3 targetPosition;

    public float radius;
    [Range(0,360)] public float angle;
    public GameObject playerRef;
    public bool canSeePlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteDirectionControllerScript = transform.GetChild(0).GetComponent<SpriteDirectionalController>();
        playerRef = GameObject.FindGameObjectWithTag("Player");

        waypointIndex = 0;
        targetPosition = new Vector3(waypoints[0].position.x, transform.position.y, waypoints[0].position.z);
        spriteDirectionControllerScript.SetFrontFacingAngle(turnDirections[0]);

        StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 direction = Vector3.forward;

        if (turnDirections[waypointIndex] == 0)
            direction = new Vector3(0, 0, 1);
        else if (turnDirections[waypointIndex] == 90)
            direction = new Vector3(-1, 0, 0);
        else if (turnDirections[waypointIndex] == 180)
            direction = new Vector3(0, 0, -1);
        else if (turnDirections[waypointIndex] == 270)
            direction = new Vector3(1, 0, 0);

        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.TransformDirection(direction), out hitInfo, 20f, playerLayer, QueryTriggerInteraction.Ignore))
            Debug.Log("hit player");

        Debug.DrawRay(transform.position, transform.TransformDirection(direction) * 20, Color.red);

        if (patrollingPaused)
            return;

        //IF AT CURRENT WAYPOINT, TRAVEL TO NEXT
        if (transform.position == targetPosition)
            NextWaypoint();

        //MOVEMENT
        targetPosition = new Vector3(waypoints[waypointIndex].position.x, transform.position.y, waypoints[waypointIndex].position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        spriteDirectionControllerScript.SetFrontFacingAngle(turnDirections[waypointIndex]);
    }

    public void NextWaypoint()
    {
        waypointIndex++;

        if (waypointIndex < waypoints.Length)
            return;

        waypointIndex = 0; //Reset index

        if (!waitBeforeLooping) //If not waiting in between loop, skip
            return;

        patrollingPaused = true;
        Invoke("UnpausePatrol", waitTimeBetweenLoops); //Wait before patrolling again
    }

    public void UnpausePatrol()
    {
        patrollingPaused = false;
    }

    private IEnumerator FOVRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerLayer);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.position, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
