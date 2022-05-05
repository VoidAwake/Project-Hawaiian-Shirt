using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.AI
{
    public class EnemyFSM : MonoBehaviour
    {
        public State currentState;

        public UnitEnemy unitEnemy;

        [SerializeField] public Vector2 currentDestination;

        public int nextWaypoint = 1;
        [SerializeField] public List<Waypoint> waypointList; //temp for now - I assume we want waypoints added procedurally later? 

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
    }
}
