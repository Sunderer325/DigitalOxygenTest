using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image[] healthIcons = default;
    [SerializeField] Color enableColor = default;
    [SerializeField] Color disableColor = default;
    int actualHP;

    private void Start()
    {
        actualHP = 0;
        foreach (Image hp in healthIcons)
            hp.color = disableColor;
    }

    public void ShowHP(int hp)
    {
        if(hp > actualHP)
        {
            for(int i = actualHP; i < hp; i++)
            {
                healthIcons[i].color = enableColor;
            }
        }
        else if(hp < actualHP)
        {
            for(int i = actualHP - 1; i + 1 > hp; i--)
            {
                healthIcons[i].color = disableColor;
            }
        }
        actualHP = hp;
    }
}
