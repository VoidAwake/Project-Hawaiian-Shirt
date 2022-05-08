using UnityEngine;

namespace Hawaiian.UI.MainMenu
{
    public class ScrollingUI : MonoBehaviour
    {
        [SerializeField] float moveSpeedX;
        [SerializeField] float maxBoundsX;

        void Update()
        {
            transform.position += Vector3.right * moveSpeedX * Time.deltaTime;
            if (Mathf.Abs(transform.position.x) > maxBoundsX)
            {
                //transform.position =
                Vector3 newPosition =
                    new Vector3((-transform.position.x / Mathf.Abs(transform.position.x)) * (maxBoundsX - Mathf.Abs((transform.position.x % maxBoundsX)))
                        , transform.position.y, transform.position.z);
                if (!float.IsNaN(newPosition.x)) transform.position = newPosition;
            }
        }
    }
}
