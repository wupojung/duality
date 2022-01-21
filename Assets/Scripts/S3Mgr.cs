using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S3Mgr : MonoBehaviour
{

    public  GameObject objP1;
    public GameObject objP2;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //P1
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("P1 Change Color");
        }
        
        //P2
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("P2 Change Color");
        }
    }
    
    
    
}
