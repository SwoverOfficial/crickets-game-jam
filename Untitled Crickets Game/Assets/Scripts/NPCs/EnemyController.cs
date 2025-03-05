using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public float degrees;

    public float radius;
    [Range(0,360)] public float angle;
    public GameObject playerRef;
    public bool canSeePlayer;

    public UnityEvent onKillPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteDirectionControllerScript = transform.GetChild(0).GetComponent<SpriteDirectionalController>();
        playerRef = GameObject.FindGameObjectWithTag("Player");

        waypointIndex = 0;
        targetPosition = new Vector3(waypoints[0].position.x, transform.position.y, waypoints[0].position.z);

        //StartCoroutine(FOVRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        FieldOfViewCheck();

        if (canSeePlayer)
            KillPlayer();

        if (patrollingPaused)
            return;

        //IF AT CURRENT WAYPOINT, TRAVEL TO NEXT
        if (transform.position == targetPosition)
            NextWaypoint();

        //MOVEMENT
        targetPosition = new Vector3(waypoints[waypointIndex].position.x, transform.position.y, waypoints[waypointIndex].position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0, turnDirections[waypointIndex], 0);
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

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
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

    public void KillPlayer()
    {
        canSeePlayer = false;
        onKillPlayer.Invoke();
    }
}
