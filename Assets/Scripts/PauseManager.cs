using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour {

    private GameManager.States prevState;

    // Update is called once per frame
    public void OnPause () {

        if (gameObject.activeSelf)
        {
            GameManager.currentState = prevState;
            gameObject.SetActive(false);
        }
        else
        {
            prevState = GameManager.currentState;
            GameManager.currentState = GameManager.States.PAUSED;
            gameObject.SetActive(true);
        }

	}

    public void OnExit()
    {
        TileManager.playerInstance.Clear();
        SceneManager.LoadScene("Menu Gioco");
    }

}
