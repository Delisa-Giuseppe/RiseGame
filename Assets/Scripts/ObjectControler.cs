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
    public int combatMoves;
    public Color colorTile;
    public Sprite iconObject;

    //Stats of single enemy
    public int totalHealth;
    public int currentHealth;
    public int physicAttack;
    public int magicAttack;
    public int moves;
    protected int critic;
    protected int evasion;
    protected Animator anim;

    protected GameObject UI;

    private void Awake()
    {
        CalculateStatistics();
        UI = GameObject.Find("UI");
        anim = GetComponentInChildren<Animator>();
    }

    public void CalculateStatistics()
    {
        totalHealth = 2 * strength + 6 * constitution + 2 * mind;
        magicAttack = 5 * mind;
        physicAttack = 3 * strength + constitution;
        moves = Mathf.CeilToInt(skill / 3f);
        critic = skill;
        evasion = skill;
        currentHealth = totalHealth;
    }


    protected void OnHit(GameObject target)
    {
        UI.GetComponent<UIManager>().ShowPopupDamage(physicAttack, target.transform);
        target.GetComponent<ObjectController>().currentHealth = target.GetComponent<ObjectController>().currentHealth - physicAttack;
        if (target.tag == "Enemy")
        {
            UI.GetComponent<UIManager>().SetEnemyHealth(target.GetComponent<EnemyController>().position, target);
        }
        else if(target.tag == "Player")
        {
            UI.GetComponent<UIManager>().SetPlayerHealthBar(target);
        }
    }

    protected bool IsDead(ObjectController target)
    {
        if(target.currentHealth <= 0)
        {
            if (target.tag == "Enemy")
            {
                UI.GetComponent<UIManager>().DestroyEnemyUI(target.GetComponent<EnemyController>().position);
            }
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
