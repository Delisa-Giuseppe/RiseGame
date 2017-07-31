using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour {

    public string abilityName;
    public float damage;
    public float cure;
    public int tileRange;
    public int turnDuration;
    public int cooldown;

    // Use this for initialization
    void Start ()
    {

        abilityName = "";
        damage = 0;
        cure = 0;
        tileRange = 0;
        turnDuration = 0;
        cooldown = 0;

    }
	
	// Update is called once per frame
	void Update ()
    {
		if(GameManager.currentState == GameManager.States.ABILITY && Input.GetMouseButtonDown(1))
        {
            TileManager.ResetGrid();
            GameManager.currentState = GameManager.States.SELECT;
            TurnManager.currentTurnState = TurnManager.TurnStates.EXECUTE;
        }
	}

    protected Vector2[] CalcolaSelezioneQuadrata()
    {

        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneRomboidale()
    {

        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.polygonInitialPoint[0].x*tileRange,
                    TileManager.polygonInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[1].x*tileRange,
                    TileManager.polygonInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[2].x*tileRange,
                    TileManager.polygonInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.polygonInitialPoint[3].x*tileRange,
                    TileManager.polygonInitialPoint[3].y*tileRange)
        };

        return points;

    }

    protected Vector2[] CalcolaSelezioneACroce()
    {
        Vector2[] points = new Vector2[] {
            new Vector2(
                    TileManager.quadInitialPoint[0].x*tileRange,
                    TileManager.quadInitialPoint[0].y),
                new Vector2(
                    TileManager.quadInitialPoint[1].x*tileRange,
                    TileManager.quadInitialPoint[1].y),
                new Vector2(
                    TileManager.quadInitialPoint[2].x*tileRange,
                    TileManager.quadInitialPoint[2].y),
                new Vector2(
                    TileManager.quadInitialPoint[3].x*tileRange,
                    TileManager.quadInitialPoint[3].y),
                new Vector2(
                    TileManager.quadInitialPoint[0].x,
                    TileManager.quadInitialPoint[0].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[1].x,
                    TileManager.quadInitialPoint[1].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[2].x,
                    TileManager.quadInitialPoint[2].y*tileRange),
                new Vector2(
                    TileManager.quadInitialPoint[3].x,
                    TileManager.quadInitialPoint[3].y*tileRange)
        };

        return points;

    }
}
