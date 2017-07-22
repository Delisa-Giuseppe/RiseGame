using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Personaggi : MonoBehaviour {

    public Button A;
    public Button B;
    public int party;

	// Use this for initialization
	void Start () {
        party = 0;
	}
	
	// Update is called once per frame
	public void Update () {
        if (party >= 4)
        {
            sblocco();
        }
        else {
            A.interactable = false;
            B.interactable = false;
        }
    }

    public void sblocco() {
       
            A.interactable = true;
            B.interactable = true;
            Debug.Log("Partiamo"); 
    }

    public void reset()
    {
        party = 0;
    }


    public void ImIn() {
        party++;
        Debug.Log(party);
    }
}
