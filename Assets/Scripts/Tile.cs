using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

    Vector3 position;
    int arrayX;
    int arrayY;
    GameObject tileObject;

    public bool isWalkable;
    public bool isChecked;
    public bool isSelected;
    public bool isEnemy;
    public bool isPlayer;
    public bool isObstacle;
    public static Color tileColor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(TurnManager.currentObjectTurn.tag == "Enemy" && collision.tag == "Tile"
            && !collision.GetComponent<Tile>().isChecked && (!collision.GetComponent<Tile>().isEnemy || (collision.GetComponent<Tile>().isWalkable && collision.GetComponent<Tile>().isPlayer)))
        {
            TileManager.tilesSelectable.Add(collision.gameObject);
            collision.gameObject.layer = LayerMask.NameToLayer("GridBattle");
            collision.GetComponent<Tile>().isSelected = true;
            StartCoroutine(WaitColor(collision));
        }
        else if (TurnManager.currentObjectTurn.tag == "Player" && collision.tag == "Tile" 
            && !collision.GetComponent<Tile>().isChecked && (!collision.GetComponent<Tile>().isPlayer || (collision.GetComponent<Tile>().isWalkable && !collision.GetComponent<Tile>().isEnemy)))
        {
            TileManager.tilesSelectable.Add(collision.gameObject);
            collision.gameObject.layer = LayerMask.NameToLayer("GridBattle");
            collision.GetComponent<Tile>().isSelected = true;
            StartCoroutine(WaitColor(collision));
        }
    }

    IEnumerator WaitColor(Collider2D collision)
    {
        yield return new WaitForSeconds(1f);
        collision.GetComponent<SpriteRenderer>().color = new Color(tileColor.r, tileColor.g, tileColor.b, 1f);
    }

    public int ArrayX
    {
        get
        {
            return arrayX;
        }

        set
        {
            arrayX = value;
        }
    }

    public int ArrayY
    {
        get
        {
            return arrayY;
        }

        set
        {
            arrayY = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }

        set
        {
            position = value;
        }
    }

    public GameObject TileObject
    {
        get
        {
            return tileObject;
        }

        set
        {
            tileObject = value;
        }
    }
}
