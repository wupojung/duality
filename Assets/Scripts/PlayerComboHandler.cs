using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        try
        {
            BlockItem block = other.GetComponent<BlockItem>();
            //Debug.Log(other.name+"-->"+block.currentType.ToString());
            
            PlayerHandler handler = transform.parent.GetComponent<PlayerHandler>();
            handler.SetPredictBlock(block);  //向上傳輸
        }
        catch (Exception exp)
        {
            Debug.LogError(exp.ToString());
            Console.WriteLine(exp);
            throw;
        }
    }
}
