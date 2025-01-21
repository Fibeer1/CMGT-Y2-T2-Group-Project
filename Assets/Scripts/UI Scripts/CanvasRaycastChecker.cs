using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasRaycastChecker : MonoBehaviour
{
    public bool detectingUI = false;
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    [SerializeField] private EventSystem eventSystem;
    private List<RaycastResult> results = new List<RaycastResult>();

    private void Start()
    {
        raycaster = GetComponent<GraphicRaycaster>();
    }


    private void Update()
    {
        pointerEventData = new PointerEventData(eventSystem);

        pointerEventData.position = Input.mousePosition;

        raycaster.Raycast(pointerEventData, results);

        if (results.Count == 0)
        {
            detectingUI = false;
        }
        else
        {
            detectingUI = true;
        }
        results.Clear();
    }
}
