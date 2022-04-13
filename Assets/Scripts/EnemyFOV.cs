using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius;[Range(0, 360)] //how far forward the fov is
    public float viewAngle; //how wide the fov is
    public float meshResolution;

    public List<Transform> visibleTargets = new List<Transform>();
    public Vector3 DirFromAngle(float angle, bool isGlobalAngle)
    {
        if (!isGlobalAngle)
        {
            angle += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Deg2Rad));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInRadius = Physics2D.OverlapBoxAll(transform.position, new Vector2(viewAngle, 0), viewRadius);
        for(int i = 0; i < targetsInRadius.Length; i++)
        {
            Transform target = targetsInRadius[i].transform;
            Vector3 targetDir = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, targetDir) < viewAngle / 2)
            {
                float targetDist = Vector2.Distance(transform.position, target.position);
                if(!Physics2D.Raycast(transform.position, targetDir, targetDist))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }
    void DrawFOV()
    {
        int sections = Mathf.RoundToInt(viewAngle * meshResolution); //number of rays the fov cone will use
        float sectionAngleSize = viewAngle / sections;
        List<Vector2> viewpoints = new List<Vector2>();
        for(int i = 0; i <= sections; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + sectionAngleSize * i;
            
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector2 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit;

        if(Physics2D.Raycast(transform.position, dir, viewRadius))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, hit.point, hit.distance, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _distance, float _angle)
        {
            hit = _hit;
            point = _point;
            distance = _distance;
            angle = _angle;

        }
    }
}
