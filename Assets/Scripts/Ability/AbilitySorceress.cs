using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySorceress : Ability {

    private void Start()
    {

        GameObject sorceressUI = GetComponent<PlayerController>().playerUI;
        sorceressUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(AttivaPallaDiFuoco);
        sorceressUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaPioggiaDiScintille);
        sorceressUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaCenereAlVento);
        sorceressUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(AttivaElementale);

    }

    public void AttivaPallaDiFuoco()
    {
        this.abilityName = "Palla Di Fuoco";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
        this.cure = 0;
        this.tileRange = 7;
        this.cooldown = 2;
        Vector2[] newPoints = CalcolaSelezioneACroce();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaPioggiaDiScintille()
    {
        this.abilityName = "Pioggia Di Scintille";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 30f;
        this.cure = 0;
        this.tileRange = 2;
        this.cooldown = 5;
        Vector2[] newPoints = CalcolaSelezioneRomboidale();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaCenereAlVento()
    {
        this.abilityName = "Cenere Al Vento";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 3;
        this.cooldown = 6;
        Vector2[] newPoints = CalcolaSelezioneQuadrata();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }

    public void AttivaElementale()
    {
        this.abilityName = "Elementale";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = GetComponent<PlayerController>().moves;
        this.cooldown = 7;
        Vector2[] newPoints = CalcolaSelezioneQuadrata();
        TileManager.ResetGrid();
        TileManager.SetTrigger(this.GetComponent<PlayerController>().PlayerTile, newPoints);
        StartCoroutine(TileManager.WaitMovesAbility(this.gameObject));

    }
}
