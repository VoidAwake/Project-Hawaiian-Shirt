using System.Collections.Generic;
using Hawaiian.Unit;
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


    private List<Transform> players = new List<Transform>();
    private Vector2 _center;
    private Vector2 velocity;

    private float _size;


    private void Start()
    {
    }

    private void OnDisable()
    {
        
     
    }

    public void AddTarget()
    {
       var players =  Resources.FindObjectsOfTypeAll<UnitPlayer>();

       foreach (UnitPlayer player in players)
       {
           this.players.Add(player.transform);
       }

    }

    private void FixedUpdate()
    {
        if (players.Count <= 0)
            return;
        
        _center = GetCenterPoint(players.ToArray());

        var newPosition = _center + _offset;
        transform.position = Vector2.SmoothDamp(transform.position, newPosition, ref velocity, _smoothTime);

        var zoom = Mathf.Lerp(_maxZoom, _minZoom, GetGreatestDistance() / _maximumDistance);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, zoom, Time.deltaTime);
    }

    public Bounds EncapsulateTargets()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);

        //adding additional bounds to additonal players
        foreach (Transform player in players)
            bounds.Encapsulate(player.position);

        return bounds;
    }

    private Vector2 GetCenterPoint(Transform[] players)
    {
        if (players.Length <= 0)
            return Vector2.zero;

        if (players.Length == 1)
            return players[0].position;

        var bounds = EncapsulateTargets();

        return bounds.center;
    }

    private float GetGreatestDistance()
    {
        var bounds = EncapsulateTargets();
        return bounds.size.x > bounds.size.y ? bounds.size.x : bounds.size.y;
    }
}