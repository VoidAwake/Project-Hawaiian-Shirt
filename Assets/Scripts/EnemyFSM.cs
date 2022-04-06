using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public State currentState;
    public State remainState;
    public float stateTimeElapsed;

    public Rigidbody2D enemy;
    public UnitPhysics unit;

    [SerializeField] public Vector2 currentDestination;

    public int nextWaypoint = 1;
    [SerializeField] public List<Waypoint> waypointList; //temp for now - I assume we want waypoints added procedurally later? 

    public float cautionMeter
    {
        get { return cautionMeter; }
        set { cautionMeter = Mathf.Clamp(0, 0, 100); } //guard transitions to alert if caution hits 100
    }
    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
        unit = GetComponent<Unit>();
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToState(State nextState)
    {

    }

    public void OnMove(Vector2 value) {
        
    }
}
