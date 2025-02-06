using UnityEngine;

public class CubeAI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum State { Idle, Patrol, Chase }
    public State currentState = State.Idle;

    public Transform[] patrolPoints;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 3f;
    public float chaseRange = 2f;

    private int currentPatrolIndex = 0;
    private Transform sphere;
    private bool movingForward = true;

    void Start()
    {
        sphere = GameObject.FindGameObjectWithTag("Player").transform;
        if (patrolPoints.Length > 0) currentState = State.Patrol;
    }

    void Update()
    {
        float distanceToSphere = Vector3.Distance(transform.position, sphere.position);

        if (distanceToSphere < detectionRange)
        {
            currentState = State.Chase;
        }
        else if (currentState == State.Chase)
        {
            if (Vector3.Distance(transform.position, sphere.transform.position) < chaseRange)
            {
                currentState = State.Patrol;
            }
        }

        switch (currentState)
        {
            case State.Idle:
                break;

            case State.Patrol:
                Patrol();
                break;

            case State.Chase:
                Chase();
                break;
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPatrolIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentPatrolIndex = movingForward ? currentPatrolIndex + 1 : currentPatrolIndex - 1;

            if (currentPatrolIndex >= patrolPoints.Length)
            {
                currentPatrolIndex = patrolPoints.Length - 2;
                movingForward = false;
            }
            else if (currentPatrolIndex < 0)
            {
                currentPatrolIndex = 1;
                movingForward = true;
            }
        }
    }

    void Chase()
    {
        transform.position = Vector3.MoveTowards(transform.position, sphere.position, chaseSpeed * Time.deltaTime);
    }
}