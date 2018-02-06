using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public GameObject healthBar;
    private Scrollbar scrollBar;
    private float health;

    private void Start()
    {

        scrollBar = healthBar.GetComponent<Scrollbar>();
        health = 100;

    }

    public void Damage (float value)
    {

        health -= value;
        scrollBar.size = health / 100f;

    }

    public void ShowBar ()
    {

        healthBar.SetActive(true);

    }

    public void HideBar()
    {

        healthBar.SetActive(false);

    }

}
