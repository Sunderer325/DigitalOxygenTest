using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    [HideInInspector] public CharacterDescription charDescription;
    public Image icon;
    [SerializeField] Text description;
    [HideInInspector] public int id;

    bool init;

    public void Init(int id)
    {
        if (init) return;

        this.id = id;
        description.text = charDescription.description.Replace("NL", "\n");
        icon.sprite = charDescription.icon;

        init = true;
    }
}

