using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverOutline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("The Object to Outline")]
    // Drag the 3D object that has the new "Outline" component into this slot
    public Outline targetOutline; 

    void Start()
    {
        // Make sure the outline is turned off when the game starts
        if (targetOutline != null)
        {
            targetOutline.enabled = false;
        }
        else
        {
            Debug.LogWarning("You forgot to assign the Target Outline to the button!");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Turn the outline ON when hovering
        if (targetOutline != null)
        {
            targetOutline.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Turn the outline OFF when the mouse leaves
        if (targetOutline != null)
        {
            targetOutline.enabled = false;
        }
    }
}