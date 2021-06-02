using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventListener<T>
{
    void OnEventRaised(T item);
}
