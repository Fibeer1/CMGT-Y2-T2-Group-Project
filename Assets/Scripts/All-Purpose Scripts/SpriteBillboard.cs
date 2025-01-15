using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboard : MonoBehaviour
{
    private bool shouldTurn = false;
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (!shouldTurn)
        {
            return;
        }
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }

    private void OnBecameVisible()
    {
        shouldTurn = true;
    }

    private void OnBecameInvisible()
    {
        shouldTurn = false;
    }
}
