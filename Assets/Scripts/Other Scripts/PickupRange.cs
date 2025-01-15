using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRange : MonoBehaviour
{
    [SerializeField] private Transform targetToMoveTo;

    private void OnTriggerEnter(Collider other)
    {
        Pickupable pickupableScript = other.transform.GetComponent<Pickupable>();

        if (pickupableScript != null)
        {
            pickupableScript.target = targetToMoveTo;
        }
    }
}
