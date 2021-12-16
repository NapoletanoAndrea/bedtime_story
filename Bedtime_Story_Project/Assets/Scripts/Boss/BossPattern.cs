using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossPattern : MonoBehaviour
{
    
    
    //farlo fermare dopo x waypoints, continua a roteare mentre si muove lentamente, 
    
    
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private float minDistance = 0.5f; //If the distance between the enemy and the waypoint is less than this, then it has reacehd the waypoint
    [SerializeField]  int waypointCheck=0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private float startSpeed;

    [SerializeField] public float movementSpeed;
    [SerializeField] public float rotationSpeed;
    [SerializeField] public int waypointsCounter;
    [SerializeField] public float timer = 0.0f;
    [SerializeField] public float timeStop;

    private bool isEnabled;
    
    private Animator animator;

    private void Awake() {
        startPosition = transform.position;
        startRotation = transform.rotation;
        animator = GetComponent<Animator>();
    }

    // Use this for initialization
	void Start () {
        startSpeed = movementSpeed;
        targetWaypoint = waypoints[targetWaypointIndex]; //Set the first target waypoint at the start so the enemy starts moving towards a waypoint
        EventManager.Instance.bossDeathEvent += Die;
    }

    // Update is called once per frame
	void Update () {
        if (!isEnabled) {
            return;
        }
        
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotationStep);
        
        Vector3 targetPosition = new Vector3(targetWaypoint.position.x, transform.position.y, targetWaypoint.position.z);
        float distance = Vector3.Distance(transform.position, targetPosition);
        CheckDistanceToWaypoint(distance);

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementStep);

        Stop();
    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if(currentDistance <= minDistance) {
            int randomIndex = targetWaypointIndex;
            while (randomIndex == targetWaypointIndex) {
                randomIndex = Random.Range(0, waypoints.Count);
            }
            targetWaypointIndex = randomIndex;
            UpdateTargetWaypoint();
        }
    }
    
    void UpdateTargetWaypoint()
    {
        waypointCheck++;
        timer = 0;
        targetWaypoint = waypoints[targetWaypointIndex];
    }

    void Stop()
    {
        if (waypointCheck == waypointsCounter)
        {
            movementSpeed = 0;
            timer += Time.deltaTime;
            if (timer>= timeStop)
            {
                movementSpeed = startSpeed;
                waypointCheck = 0;
            }
            
        }
    }

    private void Reallocate() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        animator.SetTrigger("Die");
        isEnabled = false;
        this.enabled = false;
    }
    
    private void OnEnable() {
        EventManager.Instance.fadedScreenEvent += Reallocate;
        StartCoroutine(EnableCoroutine());
    }

    private IEnumerator EnableCoroutine() {
        animator.SetTrigger("Wake");
        yield return new WaitForSeconds(2);
        isEnabled = true;
    }

    private void OnDisable() {
        EventManager.Instance.fadedScreenEvent -= Reallocate;
    }

    private void Die() {
        animator.SetTrigger("Die");
        isEnabled = false;
        this.enabled = false;
    }
}
