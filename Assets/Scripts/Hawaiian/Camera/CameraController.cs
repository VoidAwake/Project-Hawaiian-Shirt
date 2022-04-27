using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
   [SerializeField] private Camera _camera;
   [SerializeField] private float _smoothTime;
   [SerializeField]private Vector2 _offset;

   private List<Transform> players = new List<Transform>();
   private Vector2 _center;
   private Vector2 velocity;

   private float _size;


   private void Start()
   {
      PlayerInputManager.instance.onPlayerJoined += AddTarget;
   }
   
   private void OnDisable()
   {
      PlayerInputManager.instance.onPlayerJoined -= AddTarget;
   }

   public void AddTarget(PlayerInput input) => players.Add(input.gameObject.transform);

   private void FixedUpdate()
   {
      _center = GetCenterPoint(players.ToArray());
      var newPosition = _center + _offset;
      transform.position = Vector2.SmoothDamp(transform.position, newPosition, ref velocity, _smoothTime);
   }

   public Vector2 GetCenterPoint(Transform[] players)
   {
      
      if (players.Length <= 0)
         return Vector2.zero;

      if (players.Length == 1)
         return players[0].position;

      //creating bounds of the first player
      var bounds = new Bounds(players[0].position, Vector3.zero);
      
      //adding additional bounds to additonal players
      foreach (Transform player in players)
         bounds.Encapsulate(player.position);

      return bounds.center;
   }
   
}
