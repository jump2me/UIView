using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Property<T>
{
    private T m_value;
    public T Value 
    { 
        get
        {
            return m_value;
        }
        set
        {
            m_value = value;
            OnPropertyChanged();
        }
    }

    public delegate void PropertyChangeDelegate(object _sender, T _value);
    public event PropertyChangeDelegate PropertyChangedEvent;
    public void OnPropertyChanged() { if (PropertyChangedEvent != null) PropertyChangedEvent(this, Value); }
}
