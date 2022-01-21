using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public bool isBusy = false; 
    
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

    public void OnCollisionEnter2D(Collision2D other)
    {
      Wall wall=  other.gameObject.GetComponent<Wall>();
      if (wall != null)
      {
          isBusy = false;
      }
      
    }
}
