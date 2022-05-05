using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawaiian.AI
{
    public class EnemyFOV : MonoBehaviour
    {
        [Range(0, 360)]
        public float viewRadius; //how far forward the fov is
        [Range(0, 360)]
        public float viewAngle; //how wide the fov is

        [Range(0, 8)]
        public float viewDistance;

        public float meshResolution;

        public int edgeItr;
        public float edgeDst;
        public float maskCutaway = 0.1f;

        public EnemyFSM fsm;

        public LayerMask targetMask;
        public LayerMask obstacleMask;

        public List<Transform> visibleTargets = new List<Transform>();

        public Mesh viewMesh;
        public MeshFilter meshFilter;
        public bool isSus;
    
        void Start()
        {

            viewMesh = new Mesh()
            {
                name = "Mesh"
            };
            meshFilter.mesh = viewMesh;
            /*StartCoroutine("FindTargetsDelay", 0.2f);
        DrawFOV();
       */
        
        }

        IEnumerator FindTargetsDelay(float delay)
        {
            while (true)
            {
                yield return new WaitForSeconds(delay);
                FindTargets();
            }
        }
        
        void LateUpdate()
        {
            //DrawFOV();
            int sections = Mathf.RoundToInt(viewRadius * meshResolution); //number of rays the fov cone will use
            int rayCount = 50;
            float angle = viewAngle;
            float angleIncrease = viewAngle / rayCount;

            Vector3[] vertices = new Vector3[rayCount + 1 + 1];
            Vector2[] uv = new Vector2[vertices.Length];
            int[] triangles = new int[rayCount * 3];

            vertices[0] = Vector3.zero;

            Vector3 destination = new Vector3(fsm.transform.position.x - fsm.currentDestination.x, fsm.transform.position.y - fsm.currentDestination.y);
            int vertexIndex = 1;
            int triangleIndex = 0;
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                RaycastHit2D raycastHit2D = Physics2D.Raycast(Vector3.zero, DirFromAngle(angle, true), viewDistance, targetMask);
                if (raycastHit2D.collider == null)
                {
                    vertex = Vector3.zero + DirFromAngle(angle, true) * viewDistance;
                }
                else
                {
                    vertex = raycastHit2D.point;
                }
                vertices[vertexIndex] = vertex;

                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex++;
                angle -= angleIncrease;
            }


            viewMesh.vertices = vertices;
            viewMesh.uv = uv;
            viewMesh.triangles = triangles;
            //viewMesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);
        }
        public Vector3 DirFromAngle(float angle, bool isGlobalAngle)
        {
            if (!isGlobalAngle)
            {
                angle -= transform.eulerAngles.z;
            }
            return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            //return new Vector3(fsm.transform.position.x - fsm.currentDestination.x, fsm.transform.position.y - fsm.currentDestination.y);
        }
        void FindTargets()
        {
            visibleTargets.Clear();
            Collider2D[] targetsInRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

            for (int i = 0; i < targetsInRadius.Length; i++)
            {
                Transform target = targetsInRadius[i].transform;
                Vector3 targetDir = (target.position - transform.position).normalized;
                if (Vector3.Angle(transform.right, targetDir) < viewAngle / 2)
                {
                    float targetDist = Vector3.Distance(transform.position, target.position);
                    if(!Physics2D.Raycast(transform.position, targetDir, targetDist, obstacleMask))
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
            List<Vector3> viewpoints = new List<Vector3>();
            ViewCastInfo oldViewCast = new ViewCastInfo();

            for(int i = 0; i <= sections; i++)
            {
                float angle = transform.eulerAngles.z - viewAngle / 2 + sectionAngleSize * i;
                ViewCastInfo newViewCast = ViewCast(angle);

                if(i > 0)
                {
                    bool edgeDstExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDst;
                    if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstExceeded))
                    {
                        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                        if(edge.pointA != Vector3.zero)
                        {
                            viewpoints.Add(edge.pointA);

                        }
                        if(edge.pointB != Vector3.zero)
                        {
                            viewpoints.Add(edge.pointB);
                        }
                    }

                }

                viewpoints.Add(newViewCast.point);
                oldViewCast = newViewCast;
            }

            //Construct mesh

            int vertexCount = viewpoints.Count + 1;
            Vector3[] vertices = new Vector3[vertexCount];
            int[] triangles = new int[(vertexCount - 2) * 3];

            vertices[0] = Vector3.zero;
            for(int i = 0; i < vertexCount - 1; i++)
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewpoints[i]) + Vector3.up;

                if(i < vertexCount - 2)
                {
                    triangles[i * 3] = 0;
                    triangles[i * 3 + 1] = i + 1;
                    triangles[i * 3 + 2] = i + 2;
                }
            }

            viewMesh.Clear();

            viewMesh.vertices = vertices;
            viewMesh.triangles = triangles;
            viewMesh.uv = new Vector2[vertices.Length];
            viewMesh.RecalculateNormals();
        
        }

        EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
        {
            float minAngle = minViewCast.angle;
            float maxAngle = maxViewCast.angle;
            Vector3 minPoint = Vector3.zero;
            Vector3 maxPoint = Vector3.zero;

            for (int i = 0; i < edgeItr; i++)
            {
                float angle = (minAngle + maxAngle) / 2;
                ViewCastInfo newViewCast = ViewCast(angle);

                bool edgeDstExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDst;
                if (newViewCast.hit == minViewCast.hit && !edgeDstExceeded)
                {
                    minAngle = angle;
                    minPoint = newViewCast.point;
                }
                else
                {
                    maxAngle = angle;
                    maxPoint = newViewCast.point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        ViewCastInfo ViewCast(float globalAngle)
        {
            Vector3 dir = DirFromAngle(globalAngle, true);
            RaycastHit2D hit = new();

            if (Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask))
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
            public Vector3 point;
            public float distance;
            public float angle;

            public ViewCastInfo(bool _hit, Vector3 _point, float _distance, float _angle)
            {
                hit = _hit;
                point = _point;
                distance = _distance;
                angle = _angle;

            }
        }

        public struct EdgeInfo
        {
            public Vector3 pointA;
            public Vector3 pointB;

            public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
            {
                pointA = _pointA;
                pointB = _pointB;
            }
        }
    }
}
