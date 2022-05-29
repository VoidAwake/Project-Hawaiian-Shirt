using System.Collections.Generic;
using System.Linq;
using Hawaiian.Unit;
using MoreLinq;
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _smoothTime;
    [SerializeField] private Vector2 _offset;

    [Header("Zoom Settings")] [SerializeField]
    private float _maxZoom;

    [SerializeField] private float _minZoom;
    [SerializeField] private float _maximumDistance;
    [SerializeField] private float _zoomExponent = 0.65f;

    [SerializeField]  private List<Transform> players = new List<Transform>();
    private Vector2 _center;
    private Vector2 velocity;

    private float _size;
    
    
    private bool flag;

    public void AddTarget()
    {
        //just dont look at it and its going to be okay
        var players = FindObjectsOfType<UnitPlayer>();

       foreach (UnitPlayer player in players)
       {
           this.players.Add(player.transform);
       }
       

    }
    
    
    //https://gist.github.com/unitycoder/58f4b5d80f423d29e35c814a9556f9d9
    void DrawBounds(Bounds b, float delay=0)
    {
        // bottom
        var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
        var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
        var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
        var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

        Debug.DrawLine(p1, p2, Color.blue, delay);
        Debug.DrawLine(p2, p3, Color.red, delay);
        Debug.DrawLine(p3, p4, Color.yellow, delay);
        Debug.DrawLine(p4, p1, Color.magenta, delay);

        // top
        var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
        var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
        var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
        var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

        Debug.DrawLine(p5, p6, Color.blue, delay);
        Debug.DrawLine(p6, p7, Color.red, delay);
        Debug.DrawLine(p7, p8, Color.yellow, delay);
        Debug.DrawLine(p8, p5, Color.magenta, delay);

        // sides
        Debug.DrawLine(p1, p5, Color.white, delay);
        Debug.DrawLine(p2, p6, Color.gray, delay);
        Debug.DrawLine(p3, p7, Color.green, delay);
        Debug.DrawLine(p4, p8, Color.cyan, delay);
    }

    private void FixedUpdate()
    {
        if (players.Count <= 0)
            return;
        
        _center = GetCenterPoint(players.ToArray());

        //Debug.Log(_center);
      
        var newPosition = _center;
        transform.position = Vector2.SmoothDamp(transform.position, newPosition, ref velocity, _smoothTime);

        var zoom = Mathf.Lerp(_maxZoom, _minZoom, Mathf.Pow(GetGreatestDistance() / _maximumDistance, _zoomExponent));
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoom, Time.deltaTime);
    }

    public Bounds EncapsulateTargets()
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);
        
        //adding additional bounds to additonal players
        for (var i = 1; i < players.Count; i++)
        {
            Transform player = players[i];
            bounds.Encapsulate(player.position);
        }

        DrawBounds(bounds);
        return bounds;
    }

    private Vector2 GetCenterPoint(Transform[] players)
    {
        if (players.Length <= 0)
            return Vector2.zero;

        if (players.Length == 1)
            return players[0].position;

        Bounds  bounds = EncapsulateTargets();
        Vector3 center = bounds.center;
        center.z = 0;
        return center;
    }

    private float GetGreatestDistance()
    {
        var bounds = EncapsulateTargets();
        return bounds.size.x > bounds.size.y ? bounds.size.x : bounds.size.y;
    }
}