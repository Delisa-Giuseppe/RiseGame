using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public void OnRestart()
    {
        foreach (GameObject player in TileManager.playerInstanceClone)
        {
            DontDestroyOnLoad(player);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnMenu()
    {
        SceneManager.LoadScene("Menu Gioco");
    }
}
