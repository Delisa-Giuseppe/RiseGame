using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityThief : MonoBehaviour {

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

        GameObject thiefUI = GetComponent<PlayerController>().playerUI;
        thiefUI.GetComponentsInChildren<Button>()[0].onClick.AddListener(PassoDiOmbra);
        thiefUI.GetComponentsInChildren<Button>()[1].onClick.AddListener(RitornoPlanare);
        thiefUI.GetComponentsInChildren<Button>()[2].onClick.AddListener(FurtoDiIdentita);

    }

    public void PassoDiOmbra()
    {

        this.abilityName = "Passo d'ombra";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 3;
        this.cooldown = 4;

    }

    public void RitornoPlanare()
    {

        this.abilityName = "Ritorno Planare";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 2;
        this.cooldown = 6;

    }

    public void FurtoDiIdentita()
    {

        this.abilityName = "Furto d'identità";
        this.damage = 0;
        this.cure = 0;
        this.tileRange = 4;
        this.cooldown = 5;

    }
}
