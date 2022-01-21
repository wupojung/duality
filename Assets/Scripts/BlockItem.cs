using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlockItem : MonoBehaviour
{
    public BlockType currentType = BlockType.BLACK;


    void Start()
    {
        //Refresh();
    }

    private void Update()
    {
        // Refresh();
    }

    //public function 

    public void Refresh()
    {
        Image image = this.gameObject.GetComponent<Image>();
        //TODO:後續配合美術動態抓圖
        switch (currentType)
        {
            case BlockType.BLACK:
                image.color = Color.black;
                break;
            case BlockType.WHITE:
                // image.material.color = Color.white;
                image.color = Color.white;
                break;
        }
    }

    public void RandomType()
    {
        if (Random.Range(0, 2) == 1)   //隨機配色  
        {
            ChangeType();
        }
    }
    public void ChangeType()
    {
        switch (currentType)
        {
            case BlockType.BLACK:
                currentType = BlockType.WHITE;
                break;
            case BlockType.WHITE:
                currentType = BlockType.BLACK;
                break;
        }
        Refresh();
    }
}