using UnityEngine;

[CreateAssetMenu(fileName = "New Void Event", menuName ="Events/Void Event")]
public class VoidEvent : BaseEvent<Void>
{
    public void Raise() => Raise(new Void());
}
