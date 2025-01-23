using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WalkingSoundEmitter : MonoBehaviour
{
    [Header("FMOD Settings")]
    [SerializeField] private EventReference walkingSoundEvent; // FMOD event for walking sound

    [Header("Player Movement Settings")]
    [SerializeField] private Rigidbody playerRigidbody; // Reference to the player's Rigidbody
    [SerializeField] private float movementThreshold = 0.1f; // Minimum velocity to trigger walking sound

    [Header("Sound Timing Settings")]
    [SerializeField] private float stepInterval = 1f; // Time (in seconds) between each step sound
    private float stepTimer;

    private bool isWalking = false;

    private void Update()
    {
        HandleWalkingSound();
    }

    private void HandleWalkingSound()
    {
        if (playerRigidbody == null)
        {
            Debug.LogWarning("Player Rigidbody is not assigned.");
            return;
        }

        // Check if the player is moving
        float speed = playerRigidbody.velocity.magnitude;

        if (speed > movementThreshold)
        {
            if (!isWalking)
            {
                StartWalking();
            }

            // Update step timer to play sound at regular intervals
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayStepSound();
                stepTimer = stepInterval; // Reset the timer
            }
        }
        else if (isWalking)
        {
            StopWalking();
        }
    }

    private void StartWalking()
    {
        isWalking = true;
        stepTimer = 0f; // Ensure the first step happens immediately
    }

    private void StopWalking()
    {
        isWalking = false;
    }

    private void PlayStepSound()
    {
        // Trigger the FMOD walking sound event
        RuntimeManager.PlayOneShot(walkingSoundEvent, transform.position);
    }
}