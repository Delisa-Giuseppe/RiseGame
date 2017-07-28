using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHealer : MonoBehaviour {

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

        GameObject healerUI = GetComponent<PlayerController>().playerUI;
        healerUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(CostrizioneCurativa);
        healerUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(Eucarestia);
        healerUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(CatenaDiFolgore);
        healerUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(Devozione);

    }

    public void CostrizioneCurativa()
    {

        this.abilityName = "Costrizione Curativa";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 50f;
        this.cure = GetComponent<PlayerController>().magicAttack / 100f * 50f;
        this.tileRange = 2;
        this.cooldown = 3;

    }

    public void Eucarestia()
    {

        this.abilityName = "Eucarestia";
        this.damage = 0;
        this.cure = GetComponent<PlayerController>().currentHealth / 100f * 30f;
        this.tileRange = 2;
        this.cooldown = 4;

    }

    public void CatenaDiFolgore()
    {

        this.abilityName = "Catena di folgore";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 15f;
        this.cure = 0;
        this.tileRange = 2;
        this.cooldown = 4;

    }

    public void Devozione()
    {

        this.abilityName = "Devozione";
        this.damage = GetComponent<PlayerController>().magicAttack;
        this.cure = 0;
        this.tileRange = 0;
        this.cooldown = 5;

    }
}
