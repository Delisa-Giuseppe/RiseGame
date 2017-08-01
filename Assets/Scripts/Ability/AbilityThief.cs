using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityThief : Ability {

//    private void Start()
//    {
//
//        GameObject thiefUI = GetComponent<PlayerController>().playerUI;
//        thiefUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaPassoDiOmbra);
//        thiefUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaSabotaggio);
//        thiefUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(AttivaRitornoPlanare);
//
//    }
//
//    public void AttivaPassoDiOmbra()
//    {
//        this.abilityName = "Passo d'ombra";
//        this.damage = 0;
//        this.cure = 0;
//        this.tileRange = 3;
//        this.cooldown = 4;
//        Vector2[] newPoints = CalcolaSelezioneRomboidale();
//        TileManager.ResetGrid();
//        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
//        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
//    }
//
//    public void AttivaRitornoPlanare()
//    {
//        GameManager.currentState = GameManager.States.ABILITY;
//        this.abilityName = "Ritorno Planare";
//        this.damage = 0;
//        this.cure = 0;
//        this.tileRange = 2;
//        this.cooldown = 6;
//
//    }
//
//    public void AttivaSabotaggio()
//    {
//        this.abilityName = "Furto d'identità";
//        this.damage = 0;
//        this.cure = 0;
//        this.tileRange = 4;
//        this.cooldown = 5;
//        Vector2[] newPoints = CalcolaSelezioneRomboidale();
//        TileManager.ResetGrid();
//        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
//        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
//
//    }
}
