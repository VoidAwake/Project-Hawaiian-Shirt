using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    public float viewRadius;[Range(0, 360)] //how far forward the fov is
    public float viewAngle; //how wide the fov is

<<<<<<< Updated upstream
=======
    public float meshResolution;

>>>>>>> Stashed changes
    public List<Transform> visibleTargets = new List<Transform>();
    public Vector3 DirFromAngle(float angle)
    {
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
=======

    void DrawFOV()
    {
        int sections = Mathf.RoundToInt(viewAngle * meshResolution);
        float sectionAngleSize = viewAngle / sections;

    }
>>>>>>> Stashed changes
=======

    void DrawFOV()
    {

    }
>>>>>>> Stashed changes
}
