using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWarrior : MonoBehaviour {

    public string abilityName;
    public float damage;
    public float cure;
    public int tileRange;
    private int turnDuration;
    public int cooldown;

    private void Start()
    {

        abilityName = "";
        damage = 0;
        cure = 0;
        tileRange = 0;
        turnDuration = 0;
        cooldown = 0;

        GameObject warriorUI = GetComponent<PlayerController>().playerUI;
        warriorUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(AttivaCarica);
        warriorUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaAttaccoPotenziato);
        warriorUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaAttaccoRotante);

    }

    public void AttivaCarica()
    {

        this.abilityName = "Carica";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 70f;
        this.cure = 0;
        this.tileRange = GetComponent<PlayerController>().moves * 2;
        this.cooldown = 4;

    }
    
    public void AttivaAttaccoPotenziato()
    {

        this.abilityName = "Attacco Potenziato";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 125f;
        this.cure = 0;
        this.tileRange = 1;
        this.cooldown = 4;

    }

    public void AttivaAttaccoRotante()
    {

        this.abilityName = "Attacco Rotante";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 50f;
        this.cure = (5f + GetComponent<PlayerController>().magicAttack / 100f * 20f) * 4;
        this.tileRange = 1;
        this.cooldown = 5;

    }
    
}
