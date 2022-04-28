using System.Collections;
using System.Collections.Generic;
using Hawaiian.Unit;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyFSM : MonoBehaviour
{
    public State currentState;
    public State remainState;
    public float stateTimeElapsed;

    public Rigidbody2D enemy;
    public PlayerInput controls;

    public UnitEnemy unitEnemy;

    [SerializeField] public Vector2 currentDestination;

    public int nextWaypoint = 1;
    [SerializeField] public List<Waypoint> waypointList; //waypoints should come from a set list from the generated map

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerInput>();
        //viewCone = GetComponentInChildren<PolygonCollider2D>();
        //viewCone.CreateMesh(true, true);
        unitEnemy = GetComponent<UnitEnemy>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {

    }

}
