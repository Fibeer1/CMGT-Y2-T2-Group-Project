using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0, 0.5f, -10f);
    [SerializeField] private float smoothTime = 0.05f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    private void Start()
    {
        if (target == null)
        {
            target = FindObjectOfType<Player>().transform;
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
