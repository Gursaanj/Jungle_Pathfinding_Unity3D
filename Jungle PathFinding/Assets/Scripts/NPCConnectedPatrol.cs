using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCConnectedPatrol : MonoBehaviour {
    //Does the agent wait on each node 
    public bool _patrolWaiting;
    private bool _waiting;
    //the probability of wating at a certain node
    public float _waitProbability;

    //the amount of time spent at each node
    public float _totalWaitTime = 3f;

    ////The probability of switching direction during patrol
    //[SerializeField]
    //float _switchProbability = 0.2F;

    ////The list of all nodes to visit during patrol
    //[SerializeField]
    //List<Waypoint> _patrolPoints;

    //Used Private variables
    NavMeshAgent _navMeshAgent;
    //int _currentPatrolIndex;
    ConnectedWaypoint _currentWaypoint;
    ConnectedWaypoint _previouseWaypoint;
       
    bool _travelling;
    
    //bool _patrolForward;
    float _waitTimer;
    int _waypointsVisited;

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
            if (_currentWaypoint == null)
            {
                //if there is no current way point set one at random
                // grab all waypoints objects in the scene
                GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

                if (allWaypoints.Length > 0)
                {
                    while (_currentWaypoint == null)
                    {
                        int random = UnityEngine.Random.Range(0, allWaypoints.Length);
                        ConnectedWaypoint startingWaypoint = allWaypoints[random].GetComponent<ConnectedWaypoint>();

                        // when we end up finding a waypoint
                        if (startingWaypoint != null)
                        {
                            _currentWaypoint = startingWaypoint;
                        }
                    }
                }
                else
                {
                    Debug.Log("Insufficient patrol points for basic patrolling behaviour");
                }
            }

            SetDestination();

        }
    }


    // Update is called once per frame
    public void Update()
    {
        // Are we close to the destination
        if (_travelling && _navMeshAgent.remainingDistance <= 1.0F)
        {
            _travelling = false;
            _waypointsVisited++;
            SetWait();


            //If we're going to wait, then wait
            if (_patrolWaiting)
            {
                _waiting = true;
                _waitTimer = 0f;
            }
            else
            {
                //ChangePatrolPoint();
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
                SetDestination();
            }
        }
    }

    private void SetWait()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= _waitProbability)
        {
            _patrolWaiting = true;
        }
    }

    private void SetDestination()
    {
        if (_waypointsVisited > 0)
        {
            //Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;

            // check for the next waypoint making sure it isn't the same as the previous waypoint
            ConnectedWaypoint nextWaypoint = _currentWaypoint.NextWaypoint(_previouseWaypoint);
            // make sure the current waypoint becomes the new waypoint
            _previouseWaypoint = _currentWaypoint;
            _currentWaypoint = nextWaypoint;
        }

  
        _navMeshAgent.SetDestination(_currentWaypoint.transform.position);
        _travelling = true;
        }
    }
    // Doesn't need to be used
    // Selects a new patrol point in the available list but also with a small probability allows for us to move forward or backwards
    //private void ChangePatrolPoint()
    //{
    //    if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
    //    {
    //        _patrolForward = !_patrolForward;
    //    }

    //    if (_patrolForward)
    //    {
    //        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    //    }
    //    else
    //    {
    //        _currentPatrolIndex--;
    //        if (_currentPatrolIndex < 0)
    //        {
    //            _currentPatrolIndex = _patrolPoints.Count - 1;
    //        }
    //    }
    //}
