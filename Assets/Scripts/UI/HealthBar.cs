using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Image[] healthIcons;
    [SerializeField] Color disableColor;
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
                healthIcons[i].color = Color.white;
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
