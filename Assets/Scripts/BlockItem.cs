using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItem : MonoBehaviour
{
    public BlockType currentType;


    void Start()
    {
    }

    void Update()
    {
    }

    //public function 
    public void ChangeType()
    {
        if (currentType == BlockType.BLACK)
        {
            currentType = BlockType.WHITE;
        }

        if (currentType == BlockType.WHITE)
        {
            currentType = BlockType.BLACK;
        }
        //update
    }
}