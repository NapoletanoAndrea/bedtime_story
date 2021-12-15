using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    
    
    //farlo fermare dopo x waypoints, continua a roteare mentre si muove lentamente, 
    
    
     public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.1f; //If the distance between the enemy and the waypoint is less than this, then it has reacehd the waypoint
    private int lastWaypointIndex;
    [SerializeField]  int waypointCheck=0;

    [SerializeField] public float movementSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public int waypointsCounter;
    [SerializeField] public float timer = 0.0f;
    [SerializeField] public float timeStop;

	// Use this for initialization
	void Start () {
        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex]; //Set the first target waypoint at the start so the enemy starts moving towards a waypoint
	}
	
	// Update is called once per frame
	void Update () {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;
        //Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget); 

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);

        transform.Rotate(Vector3.up,rotationStep);
        
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        CheckDistanceToWaypoint(distance);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

        Stop();
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if(currentDistance <= minDistance)
        {
            targetWaypointIndex=Random.Range(0,waypoints.Count);
            UpdateTargetWaypoint();
        }
    }

    
    void UpdateTargetWaypoint()
    {
        if(targetWaypointIndex > lastWaypointIndex)
        {
            targetWaypointIndex = 0;
        }

        waypointCheck++;
        timer = 0;
        
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    void Stop ()
    {
        if (waypointCheck == waypointsCounter)
        {
            Debug.Log("Mi sono fermato");

            movementSpeed = 0;
            timer += Time.deltaTime;
            if (timer>= timeStop)
            {
                movementSpeed = 10;
                waypointCheck = 0;
            }
            
        }
    }

}
