using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEvent<T> : ScriptableObject
{
    List<IEventListener<T>> listeners = new List<IEventListener<T>>();

    public void Raise(T item)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(item);
        }
    }

    public void RegisterListener(IEventListener<T> listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }
    }

    public void UnregisterListener(IEventListener<T> listener)
    {
        if (listeners.Contains(listener))
        {
            listeners.Remove(listener);
        }
    }
}
