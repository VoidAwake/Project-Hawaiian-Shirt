
using UI.Core;
using UnityEngine;

namespace Hawaiian.UI
{
    public class Cursor : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private float _radius;

        [SerializeField] private float _offset;

        private void Update()
        {
            
           var  playerInput = UnityEngine.Input.mousePosition;
            var position = new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f, _player.transform.position.z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(playerInput);
            worldPosition.z = 0f; // set to zero since its a 2d game
            var mouseDirection = (worldPosition - position).normalized;
            var mousePosition = position + mouseDirection * _offset;

            transform.position = mousePosition;

            //OLD SYSTEM JUST KEEPING IN CASE
            // Vector2 center = _player.transform.position;
            // Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // float actualDistance = Vector2.Distance(center, position);
            //
            // if (actualDistance > _radius)
            // {
            //     Vector2 centerToPosition = position - center;
            //     centerToPosition.Normalize();
            //     position = center + centerToPosition * _radius;
            //     transform.position = position;
            // }
            // else
            // {
            //     transform.position = position;
            // }


        }
    }

}