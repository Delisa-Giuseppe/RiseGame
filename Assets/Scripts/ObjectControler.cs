using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    public int strength; // Attributo forza
    public int constitution; // Attributo costituzione
    public int skill; // Attributo destrezza
    public int mind; // Attributo intelligenza

    //Stats of single enemy
    public int health;
    public int physicAttack;
    public int magicAttack;
    public int moves;
    public int critic;
    public int evasion;


    private void Awake()
    {
        CalculateStatistics();
    }

    private void CalculateStatistics()
    {
        health = 2 * strength + 6 * constitution + 2 * mind;
        magicAttack = 5 * mind;
        physicAttack = 3 * strength + constitution;
        moves = skill;
        critic = skill;
        evasion = skill;
    }


    protected void OnHit(ObjectController target)
    {
        target.health = target.health - physicAttack;
    }

    protected bool IsDead(int targetHealth)
    {
        if(targetHealth <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
