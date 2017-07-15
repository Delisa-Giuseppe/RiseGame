﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [SerializeField]
    Vector3 position;
    [SerializeField]
    int arrayX;
    [SerializeField]
    int arrayY;
    GameObject tileObject;

    public bool isWalkable;
    public bool isChecked = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Tile" && !collision.GetComponent<Tile>().isChecked)
        {
            TileManager.tileListCollision.Add(collision.gameObject);    
        }

        this.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Tile")
        {
            TileManager.SetTrigger(TileManager.tileListCollision);
        }
    }

    public int ArrayX
    {
        get
        {
            return ArrayX1;
        }

        set
        {
            ArrayX1 = value;
        }
    }

    public int ArrayX1
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
