using UnityEngine;

[CreateAssetMenu(fileName = "Description", menuName = "UI/Character Description")]
public class CharacterDescription : ScriptableObject
{
    public Sprite icon;
    public Sprite deadIcon;
    public string description;
}
