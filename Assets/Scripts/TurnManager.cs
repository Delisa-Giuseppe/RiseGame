using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public GameObject currentObjectTurn;
    public List<GameObject> turns;

    private GameObject UI;
    private int currentTurn = 0;

    public enum TurnStates
    {
        INIT,
        EXECUTE,
        EXECUTED,
        WAIT,
        FINISH
    }

    public static TurnStates currentTurnState;

    private void Awake()
    {
        UI = GameObject.Find("UI");
    }

    //Calculate turns
    public void CalculateTurns(List<GameObject> players, List<GameObject> enemies)
    {
        currentTurnState = TurnStates.INIT;
        turns = new List<GameObject>();

        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                turns.Add(enemy);
            }
        }
        foreach (GameObject player in players)
        {
            if(player != null)
            {
                turns.Add(player);
            }
            
        }

        turns.Sort(delegate (GameObject a, GameObject b) {

            return (a.GetComponent<ObjectController>().skill).CompareTo(b.GetComponent<ObjectController>().skill);

        });
        //turns = turns.Sort(c => c.GetComponent<ObjectController>().skill).ToArray();

        foreach (GameObject turn in turns)
        {
            Debug.Log(turn.name);
        }
    }

    public GameObject GetNextTurn()
    {
        currentTurnState = TurnStates.EXECUTE;
        currentObjectTurn = turns[currentTurn];
        currentTurn++;
        if(currentObjectTurn == null)
        {
            currentObjectTurn = turns[currentTurn];
            currentTurn++;
        }
        Debug.Log("ACTUAL TURN : " + currentObjectTurn.name);
        UI.GetComponent<UIManager>().SetChangeTurnText(currentObjectTurn.GetComponent<ObjectController>().name + " Turn");
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

    public IEnumerator RecalculateTurn(List<GameObject> players, List<GameObject> enemies)
    {
        GameManager.currentState = GameManager.States.WAIT;
        yield return new WaitForEndOfFrame();
        if(IsAllTurnFinished())
        {
            currentTurn = 0;
        }
        
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
        currentTurnState = TurnStates.FINISH;
        if (currentTurn == turns.Count)
        {
            return true;
        }
        else
        {
            return false;
        }
            
    }

    public bool AreEnemiesAlive(List<GameObject> enemies)
    {
        bool found = false;
        if (enemies.Count > 0)
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
