using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityHealer : Ability {

//    private void Start()
//    {
//
//        GameObject healerUI = GetComponent<PlayerController>().playerUI;
//        healerUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(AttivaCostrizioneCurativa);
//        healerUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaEucarestia);
//        healerUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaCatenaDiFolgore);
//        healerUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(AttivaDevozione);
//
//    }
//
//    public void AttivaCostrizioneCurativa()
//    {
//        this.abilityName = "Costrizione Curativa";
//        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 50f;
//        this.cure = GetComponent<PlayerController>().magicAttack / 100f * 50f;
//        this.tileRange = 2;
//        this.cooldown = 3;
//        Vector2[] newPoints = CalcolaSelezioneRomboidale();
//        TileManager.ResetGrid();
//        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
//        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
//
//    }
//
//    public void AttivaEucarestia()
//    {
//        GameManager.currentState = GameManager.States.ABILITY;
//        this.abilityName = "Eucarestia";
//        this.damage = 0;
//        this.cure = GetComponent<PlayerController>().currentHealth / 100f * 30f;
//        this.tileRange = 2;
//        this.cooldown = 4;
//
//    }
//
//    public void AttivaCatenaDiFolgore()
//    {
//        this.abilityName = "Catena di folgore";
//        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 15f;
//        this.cure = 0;
//        this.tileRange = 2;
//        this.cooldown = 4;
//        Vector2[] newPoints = CalcolaSelezioneRomboidale();
//        TileManager.ResetGrid();
//        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
//        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
//
//    }
//
//    public void AttivaDevozione()
//    {
//        GameManager.currentState = GameManager.States.ABILITY;
//        this.abilityName = "Devozione";
//        this.damage = GetComponent<PlayerController>().magicAttack;
//        this.cure = 0;
//        this.tileRange = 0;
//        this.cooldown = 5;
//
//    }
}
