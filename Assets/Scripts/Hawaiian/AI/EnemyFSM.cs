using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.AI
{
    public class EnemyFSM : MonoBehaviour
    {
        public State currentState;

        public UnitEnemy unitEnemy;

        private int currentWaypointIndex = 1;
        [SerializeField] public List<Waypoint> waypointList; //temp for now - I assume we want waypoints added procedurally later? 

        public Waypoint CurrentWaypoint => waypointList[currentWaypointIndex];
        public Vector2 CurrentDestination => CurrentWaypoint.transform.position;
        
        private void Awake()
        {
            //viewCone = GetComponentInChildren<PolygonCollider2D>();
            //viewCone.CreateMesh(true, true);
            unitEnemy = GetComponent<UnitEnemy>();
        }

        private void Update()
        {
            currentState.UpdateState(this);
        }

        public void TransitionToState(State nextState)
        {

        }

        public void ToNextWaypoint()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypointList.Count;
        }
    }
}
