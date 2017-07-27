using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public static GameObject currentObjectTurn;
    public List<GameObject> turns;

    private GameObject UI;
    public int currentTurn = 0;

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

    public void CreateEnemyUI(List<GameObject> enemyInstance)
    {
        UI.GetComponent<UIManager>().CreateEnemyUI(enemyInstance);
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

            return (b.GetComponent<ObjectController>().skill).CompareTo(a.GetComponent<ObjectController>().skill);

        });

    }

    public GameObject GetNextTurn()
    {
        UI.GetComponent<UIManager>().ResetColor();
        currentObjectTurn = turns[currentTurn];
        //currentTurn++;
        PlayerController.canMove = true;
        UI.GetComponent<UIManager>().SetChangeTurnText(currentObjectTurn.GetComponent<ObjectController>().ObjectName + " Turn");
        if(currentObjectTurn.tag == "Player")
        {
            UI.GetComponent<UIManager>().SetPlayerTurnColor(currentObjectTurn);
        }

        currentTurnState = TurnStates.EXECUTE;
        //StartCoroutine(Wait(2f));
        return currentObjectTurn;
    }

    //IEnumerator Wait(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    currentTurnState = TurnStates.EXECUTE;
    //}


    public IEnumerator RecalculateTurn(List<GameObject> players, List<GameObject> enemies, GameManager.States nextState, TurnManager.TurnStates turnState)
    {
        GameManager.currentState = GameManager.States.WAIT;

        yield return new WaitForEndOfFrame();

        int previousTurn = currentTurn;

        if (IsAllTurnFinished())
        {
            currentTurn = 0;
        }

        if (AreEnemiesAlive(enemies))
        {
            List<GameObject> removeTurn = new List<GameObject>();
            foreach (GameObject turn in turns)
            {
                if (turn == null)
                {
                    removeTurn.Add(turn);
                    currentTurn--;
                }
            }
            foreach(GameObject remove in removeTurn)
            {
                turns.Remove(remove);
            }

            if (currentTurn < 0)
            {
                currentTurn = 0;
            }

            if (removeTurn.Count > 0 && previousTurn == currentTurn)
            {
                currentTurn++;
            }

            yield return new WaitForSeconds(1f);
            GameManager.currentState = nextState;
            currentTurnState = turnState;
        }
        else
        {
            currentTurn = 0;
            currentTurnState = TurnStates.FINISH;
            turns.Clear();
            yield return new WaitForSeconds(1.5f);
            GameManager.currentState = GameManager.States.EXPLORATION;
        }

    }

    public void ResetTurnColor()
    {
        UI.GetComponent<UIManager>().ResetColor();
    }


    public bool IsAllTurnFinished()
    {
        if (currentTurn >= turns.Count){
            
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

        if(!found)
        {
            UI.GetComponent<UIManager>().ClearEnemyList();
        }
        return found;
    }

    public void ShowBattleImage()
    {
        UI.GetComponent<UIManager>().ShowImageFight();
    }
}
