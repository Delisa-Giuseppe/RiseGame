using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public Transform target;

	// Update is called once per frame
	void Update () {
        if(target.name == "Nemesy_Warrior")
        {
            transform.position = target.position + (Vector3.up * 3f);
        }
        else
        {
            transform.position = target.position + (Vector3.up * 2.2f);
        }
	}
}
