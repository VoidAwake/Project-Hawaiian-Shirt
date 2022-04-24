using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingUI : MonoBehaviour
{
    [SerializeField] float moveSpeedX;
    [SerializeField] float maxBoundsX;

    // Update is called once per frame
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
