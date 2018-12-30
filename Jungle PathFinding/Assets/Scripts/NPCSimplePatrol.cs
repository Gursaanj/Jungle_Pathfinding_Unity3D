using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCSimplePatrol : MonoBehaviour {

    //Does the agent wait on each node 
    [SerializeField]
    bool _patrolWaiting;

    //the amount of time spent at each node
    [SerializeField]
    float _totalWaitTime = 3f;

    //The probability of switching direction during patrol
    [SerializeField]
    float _switchProbability = 0.2F;

    //The list of all nodes to visit during patrol
    [SerializeField]
    List<Waypoint> _patrolPoints;

    //Used Private variables
    NavMeshAgent _navMeshAgent;
    int _currentPatrolIndex;
    bool _travelling;
    bool _waiting;
    bool _patrolForward;
    float _waitTimer;

    // Use this for initialization
    public void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh agent is not attached to" + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("Insufficient patrol points for basic patrolling behaviour");
            }
        }

    }

    // Update is called once per frame
    public void Update()
    {
        // Are we close to the destination
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0F)
        {
            _travelling = false;

            //If we're going to wait, then wait
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                ChangePatrolPoint();
                SetDestination();
            }
        }

        //Instead if we're waiting
        if (_waiting)
        {
            _waitTimer += Time.deltaTime;
            if (_waitTimer >= _totalWaitTime)
            {
                _waiting = false;
                _patrolWaiting = false;
                _travelling = true;
            }
        }
    }

    private void SetDestination()
    {
        if (_patrolPoints != null)
        {
            Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
            _navMeshAgent.SetDestination(targetVector);
            _travelling = true;
        }
    }
    // Selects a new patrol point in the available list but also with a small probability allows for us to move forward or backwards
    private void ChangePatrolPoint()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            _currentPatrolIndex--;
            if (_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
}
