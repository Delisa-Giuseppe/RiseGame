using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour {

    

    public int strength; // Attributo forza
    public int constitution; // Attributo costituzione
    public int skill; // Attributo destrezza
    public int mind; // Attributo intelligenza
    public string ObjectName;

    //Stats of single enemy
    protected int health;
    protected int physicAttack;
    protected int magicAttack;
    public int moves;
    protected int critic;
    protected int evasion;

    private GameObject UI;


    private void Awake()
    {
        CalculateStatistics();
        UI = GameObject.Find("UI");
    }

    private void CalculateStatistics()
    {
        health = 2 * strength + 6 * constitution + 2 * mind;
        magicAttack = 5 * mind;
        physicAttack = 3 * strength + constitution;
        moves = Mathf.RoundToInt(skill / 3);
        critic = skill;
        evasion = skill;
    }


    protected void OnHit(GameObject target)
    {
        UI.GetComponent<UIManager>().ShowPopupDamage(physicAttack, target.transform);
        target.GetComponent<ObjectController>().health = target.GetComponent<ObjectController>().health - physicAttack;
    }

    protected bool IsDead(ObjectController target)
    {
        if(target.health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CompareTo(ObjectController compare)
    {
        // A null value means that this object is greater.
        if (compare == null)
            return 1;

        else
            return this.skill.CompareTo(compare.skill);
    }

}
