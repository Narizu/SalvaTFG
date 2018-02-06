using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public bool isDead;
    private int currentHealth;

    private void Start ()
    {

        currentHealth = 100;

	}

    public void TakeDamage (int attackDamage)
    {

        if (!isDead)
        {

            currentHealth -= attackDamage;
            print("ENEMY HEALTH: " + currentHealth);

            if (currentHealth <= 0)
            {

                Death();

            }

        }

    }

    private void Death()
    {

        if (!isDead)
        {

            print("ENEMY DIES");
            Destroy(gameObject);
            isDead = true;

        }

    }

}
