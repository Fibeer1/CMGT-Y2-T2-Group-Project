using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class Pickupable : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private Rigidbody rb;
    [SerializeField] private EventReference collectSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private protected void HandleMoving()
    {
        if (target == null)
        {
            return;
        }
        Vector3 diff = target.position - transform.position;
        Vector3 targetVelocity = new Vector3(diff.x, 0, diff.z).normalized * moveSpeed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Player collisionPlayer = collision.transform.GetComponent<Player>();

        if (collisionPlayer != null && collision.transform == target)
        {
            OnCollision(collisionPlayer);
            AudioManager.instance.PlayOneShot(collectSound, transform.position);
        }
    }

    public virtual void OnCollision(Player player)
    {

    }
}
