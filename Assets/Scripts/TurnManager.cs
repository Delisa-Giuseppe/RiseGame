﻿using System;
using System.Collections;
using UnityEngine;

public class TurnManager : MonoBehaviour {

    public GameObject[] turns;
    public GameObject currentObjectTurn;
    int currentTurn = 0;

    public enum PhaseTurnState
    {
        INIT,
        EXECUTE,
        FINISH
    }

    public PhaseTurnState actualPhaseTurn;

    public class MyMonsterSorter : IComparer
    {
        // Calls CaseInsensitiveComparer.Compare on the monster name string.
        int IComparer.Compare(System.Object x, System.Object y)
        {
            return ((new CaseInsensitiveComparer()).Compare(((PlayerController)x).skill, ((PlayerController)y).skill));
        }

    }


    //Calculate turns
    public void CalculateTurns(GameObject[] players, GameObject[] enemies)
    {
        actualPhaseTurn = PhaseTurnState.INIT;
        turns = new GameObject[players.Length + enemies.Length];
        int i = 0;

        IComparer myComparer = new MyMonsterSorter();
        Array.Sort(players, myComparer);

        
        foreach(GameObject player in players)
        {
            turns[i] = player;
            i++;
        }
        foreach (GameObject enemy in enemies)
        {
            turns[i] = enemy;
            i++;
        }
    }

    public GameObject GetNextTurn()
    {
        actualPhaseTurn = PhaseTurnState.EXECUTE;
        currentObjectTurn = turns[currentTurn];
        currentTurn++;
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

    public void RecalculateTurn()
    {

    }

    public bool IsAllTurnFinished()
    {
        if (currentTurn == turns.Length)
            return true;
        else
            return false;
    }
}
