using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlockItem : MonoBehaviour
{
    public BlockType currentType = BlockType.BLACK;

  //  public 
   // private Image _image;
    public Sprite day;
    public Sprite night;
    void Start()
    {
        //Refresh();
       // _image = GetComponent<Image>();
    }

    private void Update()
    {
        // Refresh();
    }

    //public function 

    public void Refresh()
    {
        Image _image = this.gameObject.GetComponent<Image>();
        //TODO:後續配合美術動態抓圖
        switch (currentType)
        {
            case BlockType.BLACK:
                _image.sprite = day;
                _image.color = Color.white;
                break;
            case BlockType.WHITE:
                // image.material.color = Color.white;
                _image.sprite = night;
                _image.color = Color.white;
                break;
        }
    }

    public void RandomType()
    {
        if (Random.Range(0, 2) == 1)   //隨機配色  
        {
            ChangeType();
        }
        Refresh();
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