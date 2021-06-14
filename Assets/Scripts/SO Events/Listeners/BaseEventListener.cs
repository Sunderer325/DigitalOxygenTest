using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEventListener<T, E, UER> : MonoBehaviour, IEventListener<T> 
    where E : BaseEvent<T> where UER : UnityEvent<T>
{
    [SerializeField] private E gameEvent;
    public E GameEvent { get => gameEvent; set => gameEvent = value; }

    [SerializeField] private UER unityEventResponse = default;

    private void OnEnable()
    {
        if (gameEvent == null)
            return;

        GameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
            return;

        GameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T item)
    {
        if (unityEventResponse != null)
            unityEventResponse.Invoke(item);
    }
}
