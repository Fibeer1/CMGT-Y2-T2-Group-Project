using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;

    private protected void HandleMoving()
    {
        if (target == null)
        {
            return;
        }
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
