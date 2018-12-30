using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreator : MonoBehaviour {

    //Objects to Instantiate
    public GameObject[] objects;
    private Collider floorCollider;
    private GameObject objectToCreate;

    //List needed to collect data
    private List<ConnectedWaypoint> _connections;
    private List<GameObject> waypointsInBounds;

    //Varaibles to assign
    private Vector3 size;

    

	// Use this for initialization
	void Start () {
        floorCollider = GetComponent<Collider>();
        size = GetComponent<Collider>().bounds.size;
        GameObject[] allWayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        _connections = new List<ConnectedWaypoint>();
        waypointsInBounds = new List<GameObject>();

        for (int i = 0; i < allWayPoints.Length; i++)
        {
            // project the waypoint to y = 0
            if (floorCollider.bounds.Contains(Vector3.ProjectOnPlane((allWayPoints[i].transform.position), Vector3.up)))
            {
                waypointsInBounds.Add(allWayPoints[i]);
            }
            
        }
        for (int i = 0; i < waypointsInBounds.Count; i++)
        {
            ConnectedWaypoint connected = waypointsInBounds[i].GetComponent<ConnectedWaypoint>();
            _connections.Add(connected);

        }
        // Find the positions of all the Waypoints so that later the objects dont instantiate at these positions


    }
	
	// Update is called once per frame
	void Update () {

        if (_connections.Count == 0)
        {
            StartCoroutine(Waity(1.5f));
        }
        else
        {
            //foreach (ConnectedWaypoint connected in _connections)
            //{
            //    if (connected.Mylight.enabled == true)
            //    {
            //        PlaceObject();
            //        _connections.Remove(connected);
            //    }
            //}
            for (int i = 0; i < _connections.Count; i++)
            {
                ConnectedWaypoint waypoint = _connections[i];
                if (waypoint.Mylight.enabled)
                {
                    _connections.Remove(waypoint);
                    PlaceObject();
                }
            }
       }
		
	}

    IEnumerator Waity(float wait)
    {
        yield return new WaitForSeconds(wait);
        Unlight();
    }

    public void Unlight()
    {
        // unlight one of the patrolpoints and add the unlight patrolpoint to _connections;
        GameObject[] allWayPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        int cont = UnityEngine.Random.Range(0, allWayPoints.Length);
        ConnectedWaypoint connected = allWayPoints[cont].GetComponent<ConnectedWaypoint>();
        connected.Mylight.enabled = false;
        _connections.Add(connected);
    }

    public void PlaceObject()
    {
        Debug.Log(_connections.Count);
        //Place an object in the plane that does not place ove the lightpoint or go over the bounds of the floor itself
        int index = UnityEngine.Random.Range(0, objects.Length);
        objectToCreate = objects[index];
        Vector3 objectBounds = objectToCreate.GetComponent<Collider>().bounds.size;
        float xInitiate = UnityEngine.Random.Range((-1 * size.x / 2) + (objectBounds.x / 2), size.x / 2 - objectBounds.x / 2);
        float zInitiate = UnityEngine.Random.Range((-1 * size.z / 2) + (objectBounds.z / 2), size.z / 2 - objectBounds.z / 2);
        Vector3 positionPlace = new Vector3(xInitiate, 0, zInitiate);
        objectToCreate.transform.position = positionPlace;
        // gotta find a way to find change the rotation along the y direction
        Instantiate(objectToCreate, objectToCreate.transform);
        Debug.Log(objectToCreate.name);
        Debug.Log(objectToCreate.transform.position);


    }
}
