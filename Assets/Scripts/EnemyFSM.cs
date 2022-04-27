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
    public EnemyFOV fov;
    //public PolygonCollider2D viewCone;

    public UnitEnemy unitEnemy;

    [SerializeField] public Vector2 currentDestination;

    public int nextWaypoint = 1;
    [SerializeField] public List<Waypoint> waypointList; //waypoints should come from a set list from the generated map

    public float cautionMeter
    {
        get { return cautionMeter; }
        set { cautionMeter = Mathf.Clamp(2, 2, 100); } //guard transitions to alert if caution hits 100
    }
    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        controls = GetComponent<PlayerInput>();
        fov = GetComponent<EnemyFOV>();
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
