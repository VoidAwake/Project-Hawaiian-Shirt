using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Hawaiian.Utilities
{
    /// <summary>
    /// Simple 4 point Bezier Curve System
    /// </summary>
    public static class BezierCurve 
    {
        public static LineRenderer DrawQuadraticBezierCurve(LineRenderer renderer, Vector2 a, Vector2 b, Vector2 c)
        {
            renderer.positionCount = 200;

            var points = QuadraticBezierCurvePoints(a, b, c, renderer.positionCount);
            
            renderer.SetPositions(points.Select(p => (Vector3) p).ToArray());
            
            return renderer;
        }

        public static List<Vector2> QuadraticBezierCurvePoints(Vector2 a, Vector2 b, Vector2 c, int numOfPoints)
        {
            var output = new List<Vector2>();
            
            float t = 0f;
            
            for (int i = 0; i < numOfPoints; i++)
            {
                output.Add((1 - t) * (1 - t) * a + 2 * (1 - t) * t * b + t * t * c);
                t += (1 / (float) numOfPoints);
            }

            return output;
        }
    }
}
