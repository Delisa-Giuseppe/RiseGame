using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWarrior : Ability {

    private void Start()
    {

        GameObject warriorUI = GetComponent<PlayerController>().playerUI;
        warriorUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(AttivaCarica);
        warriorUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaAttiraNemici);
        warriorUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaAttaccoPotenziato);
        warriorUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(AttivaAttaccoRotante);

    }

    public void AttivaCarica()
    {
        this.abilityName = "Carica";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 70f;
        this.cure = 0;
        this.tileRange = GetComponent<PlayerController>().moves * 2;
        this.cooldown = 4;
        Vector2[] newPoints = CalcolaSelezioneACroce();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaAttiraNemici()
    {
        this.abilityName = "AttiraNemici";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 2;
        this.cooldown = 4;
        Vector2[] newPoints = CalcolaSelezioneRomboidale();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaAttaccoPotenziato()
    {
        this.abilityName = "Attacco Potenziato";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 125f;
        this.cure = 0;
        this.tileRange = 1;
        this.cooldown = 4;
        Vector2[] newPoints = CalcolaSelezioneRomboidale();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaAttaccoRotante()
    {
        this.abilityName = "Attacco Rotante";
        this.damage = GetComponent<PlayerController>().physicAttack / 100f * 50f;
        this.cure = (5f + GetComponent<PlayerController>().magicAttack / 100f * 20f) * 4;
        this.tileRange = 2;
        this.cooldown = 5;
        Vector2[] newPoints = CalcolaSelezioneQuadrata();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));
    }
    
}
