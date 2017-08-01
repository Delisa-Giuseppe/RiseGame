using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWarrior : Ability {

    public enum WarriorAbilityType
    {
        CARICA,
        ATTIRA_NEMICI,
        ATTACCO_POTENZIATO,
        ATTACCO_ROTANTE
    }

    public WarriorAbilityType currentAbility;

    private void Start()
    {

        GameObject warriorUI = GetComponent<PlayerController>().playerUI;
        warriorUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(AttivaCarica);
        warriorUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(AttivaAttiraNemici);
        warriorUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(AttivaAttaccoPotenziato);
        warriorUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(AttivaAttaccoRotante);


    }

    private void Update()
    {
        if (GameManager.currentState == GameManager.States.ABILITY && Input.GetMouseButtonDown(0))
        {
            if (TurnManager.currentObjectTurn && TurnManager.currentObjectTurn.tag == "Player" && TileManager.CheckEnemy())
            {
                UseAbility(currentAbility);
            }

        }
    }

    public void UseAbility(WarriorAbilityType abilityType)
    {
        switch (abilityType)
        {
            case WarriorAbilityType.CARICA:
                Debug.Log("Carica");
                break;
            case WarriorAbilityType.ATTIRA_NEMICI:
                Debug.Log("Attira");
                break;
            case WarriorAbilityType.ATTACCO_POTENZIATO:
                UsaAttaccoPotenziato();
                break;
            case WarriorAbilityType.ATTACCO_ROTANTE:
                Debug.Log("Rotante");
                break;

        }
    }

    public void AttivaCarica()
    {
        currentAbility = WarriorAbilityType.CARICA;
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
        currentAbility = WarriorAbilityType.ATTIRA_NEMICI;
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
        currentAbility = WarriorAbilityType.ATTACCO_POTENZIATO;
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

    public void UsaAttaccoPotenziato()
    {


        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider != null && hit.collider.tag == "Enemy" ||
        (hit.collider != null && hit.collider.tag == "Tile" && hit.collider.GetComponent<Tile>().isSelected && hit.collider.GetComponent<Tile>().isEnemy))
        {
            GameObject[] enemyTarget = new GameObject[1];

            foreach (GameObject enemy in TileManager.enemyInstance)
            {
                if (enemy.GetComponent<EnemyController>().EnemyTile.transform.position == hit.collider.transform.position)
                {
                    enemyTarget[0] = enemy;
                    break;
                }
            }
            PhysicAbilityAttack(enemyTarget);
        }
    }

    public void AttivaAttaccoRotante()
    {
        currentAbility = WarriorAbilityType.ATTACCO_ROTANTE;
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
