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
            float t = 0f;
            Vector2 current = new Vector2(0, 0);

            for (int i = 0; i < renderer.positionCount; i++)
            {
                current = (1 - t) * (1 - t) * a + 2 * (1 - t) * t * b + t * t * c;
                renderer.SetPosition(i,current);
                t += (1 / (float) renderer.positionCount);
            }

            return renderer;
        }
    }
}
