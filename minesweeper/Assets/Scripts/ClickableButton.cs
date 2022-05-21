using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/**
 * This class represents a script that is attached to a button
 * which has a seperate events for the right and left click, which
 * makes it easier to use than the Unity default Button implementation 
 */

public class ClickableButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    public UnityEvent OnRightClick;
    public UnityEvent OnLeftClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Cancel if it is a middle
        // Click
        if (eventData.button == PointerEventData.InputButton.Middle)
            return;

        // If it's the right click
        // Perform the first event
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }

        // This means that it's the left click
        // So perform the event
        OnLeftClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // If it's the right click
        // Perform the first event
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick.Invoke();
            return;
        }
    }
}
