using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public bool isBusy = false;

    private BlockItem _currentBlock;

    #region Unity core event

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Rigidbody2D>().velocity.x == 0)
        {
            isBusy = false;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        int x = 0;
        if (gameObject.name == "P2")
        {
            x = 150;
        }

        if (_currentBlock.currentType == BlockType.BLACK)
        {
            style.normal.textColor = Color.black;
        }

        GUI.Label(new Rect(x, 0, 150, 50), _currentBlock.currentType.ToString(), style);
    }

    #endregion

    #region Unity Events

    public void OnCollisionEnter2D(Collision2D other)
    {
        Wall wall = other.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            isBusy = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        BlockItem block = other.GetComponent<BlockItem>();
        _currentBlock = block;
    }

    #endregion
}