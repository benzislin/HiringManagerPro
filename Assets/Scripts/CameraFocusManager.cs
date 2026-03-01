using UnityEngine;
using System.Collections.Generic;

// This allows us to create a custom grouping of settings in the Inspector
[System.Serializable]
public class FocusTarget
{
    public string targetName = "New Target"; // Just for organizing in the Inspector
    public Collider targetCollider;
    public Outline targetOutline;
    public Transform focusPoint;
    public GameObject uiScreen;
}

public class CameraFocusManager : MonoBehaviour
{
    [Header("List of Clickable Objects")]
    // This creates an expandable list in your Inspector
    public List<FocusTarget> targets; 

    [Header("Settings")]
    public float moveTime = 0.6f;

    private Vector3 startPos;
    private Quaternion startRot;
    private FocusTarget currentFocus; // Keeps track of what we are currently looking at
    private bool moving;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        // Ensure all UI and outlines are turned off at the start
        foreach (var target in targets)
        {
            if (target.uiScreen != null) target.uiScreen.SetActive(false);
            if (target.targetOutline != null) target.targetOutline.enabled = false;
        }
    }

    void Update()
    {
        if (moving)
        {
            // Turn off all outlines while the camera is flying
            foreach (var t in targets)
                if (t.targetOutline != null) t.targetOutline.enabled = false;
            return;
        }

        // 1. Shoot raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool hitSomething = Physics.Raycast(ray, out RaycastHit hit, 200f);

        FocusTarget hoveredTarget = null;

        // 2. Check if we hit ANY of the targets in our list
        if (hitSomething)
        {
            foreach (var target in targets)
            {
                if (hit.collider == target.targetCollider)
                {
                    hoveredTarget = target;
                    break; 
                }
            }
        }

        // 3. Handle Outlines
        foreach (var target in targets)
        {
            if (target.targetOutline != null)
            {
                // Only highlight if we are hovering over it AND we aren't currently zoomed into it
                target.targetOutline.enabled = (target == hoveredTarget && currentFocus != target);
            }
        }

        // 4. Handle Clicks
        if (Input.GetMouseButtonDown(0))
        {
            if (hoveredTarget != null)
            {
                if (currentFocus == hoveredTarget)
                {
                    // Clicked the object we are already zoomed into -> Zoom Out
                    StartCoroutine(MoveCamera(startPos, startRot, null));
                }
                else 
                {
                    // Zoom into the new target (works from start position OR jumping between objects)
                    StartCoroutine(MoveCamera(hoveredTarget.focusPoint.position, hoveredTarget.focusPoint.rotation, hoveredTarget));
                }
            }
            else if (currentFocus != null)
            {
                // Clicked empty space/background while zoomed in -> Zoom Out
                StartCoroutine(MoveCamera(startPos, startRot, null));
            }
        }
    }

    System.Collections.IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, FocusTarget newFocus)
    {
        moving = true;

        // Hide current UI immediately before moving
        if (currentFocus != null && currentFocus.uiScreen != null)
            currentFocus.uiScreen.SetActive(false);

        Vector3 p0 = transform.position;
        Quaternion r0 = transform.rotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(p0, targetPos, t);
            transform.rotation = Quaternion.Slerp(r0, targetRot, t);
            yield return null;
        }

        currentFocus = newFocus;
        moving = false;

        // Show new UI after arriving
        if (currentFocus != null && currentFocus.uiScreen != null)
            currentFocus.uiScreen.SetActive(true);
    }
}