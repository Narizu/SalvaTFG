﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject spellPrefab;
    public Transform spellSpawn;

    private NavMeshAgent agent;
    private Vector3 destination;
    private bool attacking;
    private float distance;
    private int maxDistance;
    private Collider selected;
    private float timeLastAttack;
    private float timeBetweenAttacks;
    private int attackDamage;
    private int experience;
    private int level;
    private int constant;

    private void Start()
    {
        destination = new Vector3(0, 0, 0);
        agent = GetComponent<NavMeshAgent>();
        attacking = false;
        distance = 0;
        maxDistance = 4;
        selected = null;
        timeLastAttack = 0;
        timeBetweenAttacks = 1;
        attackDamage = 10;
        experience = 0;
        level = 1;
        constant = 10;
        print("GAINED EXPERIENCE: 0. CURRENT EXPERIENCE: " + experience + ".");
        print("LEVEL: " + level + ".");
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (agent.isStopped)
        {
            agent.updateRotation = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                destination = hit.point;
                agent.updateRotation = true;

                if (hit.collider.tag == "Enemy")
                {
                    selected = hit.collider;
                    attacking = true;

                    if (!InRange(destination))
                    {
                        PlayerMovement(destination);
                    }
                }
                else if (hit.collider.tag == "Floor")
                {
                    attacking = false;
                    PlayerMovement(destination);
                }
            }
        }

        if (attacking && InRange(destination))
        {
            //var health = selected.GetComponent<Health>();

            if (selected != null)
            {
                agent.isStopped = true;
                PlayerAttack();
            }
            else
            {
                attacking = false;
                UpdateExperience(constant);
            }
        }

        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Spell from the Spell Prefab
        var spell = (GameObject)Instantiate(
            spellPrefab,
            spellSpawn.position,
            spellSpawn.rotation);

        // Add velocity to the spell
        spell.GetComponent<Rigidbody>().velocity = spell.transform.forward * 6;

        // Spawn the spell on the Clients
        NetworkServer.Spawn(spell);

        // Destroy the spell after 2 seconds
        Destroy(spell, 2.0f);
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    private void PlayerMovement (Vector3 point)
    {
        print("GOING TO " + point);
        agent.isStopped = false;
        agent.destination = point;
    }

    public void StopAgent ()
    {
        agent.isStopped = true;
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

    private void PlayerAttack ()
    {
        if (Time.time - timeLastAttack > timeBetweenAttacks)
        {
            var health = selected.GetComponent<Health>();
            health.TakeDamage(attackDamage);
            timeLastAttack = Time.time;
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