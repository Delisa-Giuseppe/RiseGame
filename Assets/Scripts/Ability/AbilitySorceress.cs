using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitySorceress : MonoBehaviour {

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

        GameObject sorceressUI = GetComponent<PlayerController>().playerUI;
        sorceressUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(PallaDiFuoco);
        sorceressUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(PioggiaDiScintille);
        sorceressUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(CenereAlVento);
        sorceressUI.GetComponentsInChildren<Button>()[3].onClick.AddListener(Elementale);

    }

    public void PallaDiFuoco()
    {

        this.abilityName = "Palla Di Fuoco";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 60f;
        this.cure = 0;
        this.tileRange = 3;
        this.cooldown = 2;

    }

    public void PioggiaDiScintille()
    {

        this.abilityName = "Pioggia Di Scintille";
        this.damage = GetComponent<PlayerController>().magicAttack / 100f * 30f;
        this.cure = 0;
        this.tileRange = 2;
        this.cooldown = 5;

    }

    public void CenereAlVento()
    {

        this.abilityName = "Cenere Al Vento";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 3;
        this.cooldown = 6;

    }

    public void Elementale()
    {

        this.abilityName = "Elementale";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = GetComponent<PlayerController>().moves;
        this.cooldown = 7;

    }
}
