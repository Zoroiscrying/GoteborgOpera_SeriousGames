using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This class's interface functions need the 'PhysicsRaycaster2D' Component.
/// </summary>
public class BaseTouchable2DObject : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private float activateWaitTime = 1.0f;
    
    protected bool PointerDown = false;
    protected float PointerDownTime = 0.0f;

    /// <summary>
    /// Only True when pointer hold for 'activateWaitTime'.
    /// "ActivateObject()" can only be activated once for certain 'waitTime'.
    /// Once the pointer lifted, 'pointerDownTime' is refreshed.
    /// </summary>
    protected bool Activated = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (PointerDown && !Activated)
        {
            PointerDownTime += Time.deltaTime;
            if (PointerDownTime > activateWaitTime)
            {
                ActivateObject();
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // Placeholder
    }
    
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // Placeholder
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (!PointerDown)
        {
            Debug.Log(name + "Game Object Click in Progress");
            PointerDown = true;
            PointerDownTime = 0.0f;
            StartActivatingObject();
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (PointerDown)
        {
            Debug.Log(name + "No longer being clicked");
            Activated = false;
            PointerDown = false;
            PointerDownTime = 0.0f;
            EndActivatingObject();
        }
    }

    protected virtual void StartActivatingObject()
    {
        // maybe some scaling up hint here.
    }

    protected virtual void EndActivatingObject()
    {
        Activated = false;
    }

    protected virtual void ActivateObject()
    {
        Activated = true;
    }
}
