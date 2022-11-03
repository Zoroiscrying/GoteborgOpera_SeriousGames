using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// This class's interface functions need the 'PhysicsRaycaster2D' Component.
/// </summary>
public class BaseTouchable2DObject : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    [SerializeField] private float activateWaitTime = 1.0f;
    [SerializeField] private UnityEvent onObjectButtonDownEvent;
    [SerializeField] private UnityEvent onObjectButtonUpEvent;
    protected event Action OnObjectButtonDown;
    protected event Action OnObjectButtonUp;

    protected event Action OnTouchableActivated;
    
    protected bool PointerDown = false;
    protected float PointerDownTime = 0.0f;

    [SerializeField] protected bool canBeActivated = true;
    public bool CanBeActivated => canBeActivated;

    /// <summary>
    /// Only True when pointer hold for 'activateWaitTime'.
    /// "ActivateObject()" can only be activated once for certain 'waitTime'.
    /// Once the pointer lifted, 'pointerDownTime' is refreshed.
    /// </summary>
    protected bool Activated = false;

    // Update is called once per frame
    protected virtual void Update()
    {
        if (canBeActivated && PointerDown && !Activated)
        {
            PointerDownTime += Time.deltaTime;
            if (PointerDownTime > activateWaitTime)
            {
                ActivateObject();
            }
        }
    }

    public void ClearButtonDownEvent()
    {
        PointerDown = false;
        onObjectButtonDownEvent = new UnityEvent();
        OnObjectButtonDown = null;
    }
    
    public void ClearActivatedEvent()
    {
        PointerDown = false;
        OnTouchableActivated = null;
    }
    
    public void ClearButtonUpEvent()
    {
        PointerDown = true;
        onObjectButtonUpEvent = new UnityEvent();
        OnObjectButtonUp = null;
    }
    
    public void SubscribeOnObjectActivated(Action onActivated)
    {
        OnTouchableActivated += onActivated;
    }
        
    public void UnsubscribeOnObjectActivated(Action onActivated)
    {
        OnTouchableActivated -= onActivated;
    }

    public void SubscribeOnObjectButtonDown(Action onButtonDown)
    {
        OnObjectButtonDown += onButtonDown;
    }
        
    public void UnsubscribeOnObjectButtonDown(Action onButtonDown)
    {
        OnObjectButtonDown -= onButtonDown;
    }
    
    public void SubscribeOnObjectButtonUp(Action onButtonUp)
    {
        OnObjectButtonUp += onButtonUp;
    }
        
    public void UnsubscribeOnObjectButtonUp(Action onButtonUp)
    {
        OnObjectButtonUp -= onButtonUp;
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
        if (canBeActivated && !PointerDown)
        {
            Debug.Log(name + "Game Object Click in Progress");
            PointerDown = true;
            PointerDownTime = 0.0f;
            StartActivatingObject();
            
            // invoke OnObjectButtonDown().
            OnObjectButtonDown?.Invoke();
            onObjectButtonDownEvent?.Invoke();
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (canBeActivated && PointerDown)
        {
            // Debug.Log(name + "No longer being clicked");
            Activated = false;
            PointerDown = false;
            PointerDownTime = 0.0f;
            EndActivatingObject();
            
            OnObjectButtonUp?.Invoke();
            onObjectButtonUpEvent?.Invoke();
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
        if (canBeActivated)
        {
            OnTouchableActivated?.Invoke();
            Activated = true;   
        }
    }
}
