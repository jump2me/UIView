using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public partial class UIView
{
    public EventTrigger EventTrigger { get; private set; }
    public void InitializeEventTrigger()
    {
        EventTrigger = GameObject.GetComponent<EventTrigger>();
        if(EventTrigger == null)
        {
            EventTrigger = GameObject.AddComponent<EventTrigger>();
        }

        EventTrigger.triggers = new List<EventTrigger.Entry>();
        var triggers = System.Enum.GetValues(typeof(EventTriggerType));
        foreach(EventTriggerType t in triggers)
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = t;
            switch(t)
            {
                case EventTriggerType.PointerClick:
                    entry.callback.AddListener(OnPointerClicked);
                    break;
                case EventTriggerType.PointerUp:
                    entry.callback.AddListener(OnPointerUp);
                    break;
                case EventTriggerType.PointerDown:
                    entry.callback.AddListener(OnPointerDown);
                    break;
                default:
                    continue;
            }

            EventTrigger.triggers.Add(entry);
        }
    }

    public virtual void OnPointerClicked(BaseEventData e)
    {
        
    }

    public virtual void OnPointerUp(BaseEventData e)
    {
        
    }

    public virtual void OnPointerDown(BaseEventData e)
    {
        
    }   
}