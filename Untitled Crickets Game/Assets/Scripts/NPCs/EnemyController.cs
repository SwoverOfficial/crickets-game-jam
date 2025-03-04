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

    Vector3 targetPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteDirectionControllerScript = transform.GetChild(0).GetComponent<SpriteDirectionalController>();

        waypointIndex = 0;
        targetPosition = new Vector3(waypoints[0].position.x, transform.position.y, waypoints[0].position.z);
        spriteDirectionControllerScript.SetFrontFacingAngle(turnDirections[0]);
    }

    // Update is called once per frame
    void Update()
    {
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
}
