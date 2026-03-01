using UnityEngine;

public class ComputerFocusClick : MonoBehaviour
{
    [Header("What you click")]
    public Collider computerCollider;

    [Header("The Outline Component")]
    // Drag the computer (which has the Outline script) into this slot
    public Outline computerOutline; 

    [Header("Where camera moves to")]
    public Transform focusPoint; 

    [Header("How fast it moves")]
    public float moveTime = 0.6f;

    [Header("Optional: UI that appears when focused")]
    public GameObject computerUIScreen;

    private Vector3 startPos;
    private Quaternion startRot;
    private bool focused;
    private bool moving;

    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;

        if (computerUIScreen != null)
            computerUIScreen.SetActive(false);

        // Ensure the outline is turned off when the game starts
        if (computerOutline != null)
            computerOutline.enabled = false;
    }

    void Update()
    {
        if (moving)
        {
            // Turn off outline while camera is flying
            if (computerOutline != null) computerOutline.enabled = false;
            return;
        }

        // 1. Shoot a raycast every frame to see what the mouse is hovering over
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHoveringComputer = false;

        if (Physics.Raycast(ray, out RaycastHit hit, 200f))
        {
            if (hit.collider == computerCollider)
            {
                isHoveringComputer = true;

                // 2. Listen for the click exactly like before
                if (Input.GetMouseButtonDown(0))
                {
                    if (focused)
                    {
                        // Move back to start
                        StartCoroutine(MoveCamera(startPos, startRot, false));
                    }
                    else
                    {
                        // Move to the focus point
                        StartCoroutine(MoveCamera(focusPoint.position, focusPoint.rotation, true));
                    }
                }
            }
        }

        // 3. Turn the outline on or off based on the hover state
        if (computerOutline != null)
        {
            // Optional: If you don't want it to highlight while you are already zoomed in, 
            // you can change this to: computerOutline.enabled = isHoveringComputer && !focused;
            computerOutline.enabled = isHoveringComputer;
        }
    }

    System.Collections.IEnumerator MoveCamera(Vector3 targetPos, Quaternion targetRot, bool toFocused)
    {
        moving = true;

        if (!toFocused && computerUIScreen != null)
            computerUIScreen.SetActive(false);

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

        focused = toFocused;
        moving = false;

        if (focused && computerUIScreen != null)
            computerUIScreen.SetActive(true);
    }
}