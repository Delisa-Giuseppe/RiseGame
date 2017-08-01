using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour {

    public Transform target;


	// Update is called once per frame
	void Update () {
        transform.position = target.position + (Vector3.up * 2.4f);
	}
}
