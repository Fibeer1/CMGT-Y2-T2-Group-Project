using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class WalkingSoundEmitter : MonoBehaviour
{
    [SerializeField] private EventReference footstepSound; // FMOD event for walking sound

    public void PlayStepSound()
    {
        RuntimeManager.PlayOneShot(footstepSound, transform.position);
    }
}