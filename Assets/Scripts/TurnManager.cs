using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public GameObject[] turns;
    public GameObject currentObjectTurn;
    int currentTurn = 0;

    //Calculate turns
    public void CalculateTurns(GameObject[] players, GameObject[] enemies)
    {
        turns = new GameObject[players.Length + enemies.Length];
        ObjectController[] orderTurn = new ObjectController[players.Length + enemies.Length];
        int i = 0;


        foreach (GameObject player in players)
        {
            turns[i] = player;
            i++;
        }
        foreach (GameObject enemy in enemies)
        {
            turns[i] = enemy;
            i++;
        }

        turns = turns.OrderBy(c => c.GetComponent<ObjectController>().skill).ToArray();

        foreach (GameObject turn in turns)
        {
            Debug.Log(turn.name);
        }
    }

    public GameObject GetNextTurn()
    {
        currentObjectTurn = turns[currentTurn];
        currentTurn++;
        Debug.Log("ACTUAL TURN : " + currentObjectTurn.name);
        return currentObjectTurn;
    }

    public GameObject GetPreviousTurn()
    {
        int tmp = currentTurn - 2;
        if(tmp >=0)
        {
            return turns[tmp];
        }
        else
        {
            return null;
        }
    }

    public IEnumerator RecalculateTurn(GameObject[] players, GameObject[] enemies)
    {
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForEndOfFrame();
        currentTurn = 0;
        if(AreEnemiesAlive(enemies))
        {
            CalculateTurns(players, enemies);
            GameManager.currentState = GameManager.States.SELECT;
        }
        else
        {
            GameManager.currentState = GameManager.States.EXPLORATION;
        }
        

    }

    public bool IsAllTurnFinished()
    {
        if (currentTurn == turns.Length)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public bool AreEnemiesAlive(GameObject[] enemies)
    {
        bool found = false;
        if (enemies.Length > 0)
        {
            foreach(GameObject enemy in enemies)
            {
                if(enemy != null)
                {
                    found = true;
                }
                else
                {
                    continue;
                }
            }
            
        }

        return found;
    }
}
