using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    Vector3 position;
    int arrayX;
    int arrayY;
    GameObject tileObject;

    public bool isWalkable;
    public bool isChecked;
    public bool isSelected;
    public bool isBusy;
    public bool isEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Tile" && !collision.GetComponent<Tile>().isChecked)
        {
            TileManager.tilesSelectable.Add(collision.gameObject);
            collision.gameObject.layer = LayerMask.NameToLayer("GridBattle");
            collision.GetComponent<SpriteRenderer>().color = Color.red;
            collision.GetComponent<Tile>().isSelected = true;
        }
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
