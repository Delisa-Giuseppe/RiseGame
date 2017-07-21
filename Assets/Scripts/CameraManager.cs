using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    private Vector2 velocity;

    public float smootTimeX;
    public GameObject player;
    public Vector3 initialPosition;

    private void Start()
    {
        transform.position = initialPosition;
    }

    // Update is called once per frame
    void LateUpdate () {
		if(player != null)
        {
            MoveCamera();
        }
	}

    public void MoveCamera()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, smootTimeX);
        if(posX > initialPosition.x)
        {
            transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }
}
