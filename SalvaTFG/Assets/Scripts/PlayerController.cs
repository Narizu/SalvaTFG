using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{

    private NavMeshAgent agent;
    private int attackDamage;
    private GameObject enemy;
    private EnemyHealth enemyHealth;
    private float timeBetweenAttacks;
    private float timeLastAttack;
    private float distance;
    private int maxDistance;
    private bool attacking;
    private Vector3 destination;
    private GameObject cameraHB;
    private HealthBar healthBar;

    private int experience;
    private int level;
    private int constant;

    private void Start ()
    {

        agent = GetComponent<NavMeshAgent>();
        attackDamage = 10;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyHealth = enemy.GetComponent<EnemyHealth>();
        timeBetweenAttacks = 1;
        timeLastAttack = 0;
        distance = 0;
        maxDistance = 4;
        attacking = false;
        destination = new Vector3(0, 0, 0);
        cameraHB = GameObject.FindGameObjectWithTag("MainCamera");
        healthBar = cameraHB.GetComponent<HealthBar>();

        experience = 0;
        level = 1;
        constant = 10;
        print("GAINED EXPERIENCE: 0. CURRENT EXPERIENCE: " + experience + ".");
        print("LEVEL: " + level + ".");

    }
	
	private void FixedUpdate ()
    {

        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {

                destination = hit.point;
                
                if (hit.collider.tag == "Enemy")
                {

                    attacking = true;
                    healthBar.ShowBar();

                    if (!InRange(destination))
                    {

                        PlayerMovement(destination);

                    }

                }

                else if (hit.collider.tag == "Floor")
                {

                    attacking = false;
                    healthBar.HideBar();
                    PlayerMovement(destination);

                }

            }

        }


        if (attacking && InRange(destination))
        {

            if (!enemyHealth.isDead)
            {

                agent.isStopped = true;
                PlayerAttack();

            }

            else
            {

                attacking = false;
                healthBar.HideBar();
                UpdateExperience(constant);

            }

        }

	}

    private void PlayerMovement (Vector3 point)
    {

        print("GOING TO " + point);
        agent.isStopped = false;
        agent.destination = point;

    }

    private void PlayerAttack ()
    {

        if (Time.time - timeLastAttack > timeBetweenAttacks)
        {

            enemyHealth.TakeDamage(attackDamage);
            healthBar.Damage(attackDamage);
            timeLastAttack = Time.time;

        }

    }

    private bool InRange (Vector3 point)
    {

        distance = Vector3.Distance(point, transform.position);
        
        if (distance < maxDistance)
        {

            return true;

        }

        else
        {

            return false;

        }

    }

    private void UpdateExperience (int exp)
    {

        experience += exp;
        print("GAINED EXPERIENCE: " + exp + ". CURRENT EXPERIENCE: " + experience + ".");
        level = experience / constant + 1;
        print("LEVEL: " + level + ".");

    }

}
