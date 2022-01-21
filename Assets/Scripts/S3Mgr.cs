using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S3Mgr : MonoBehaviour
{
    public GameObject objP1;
    public GameObject objP2;

    public float speed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //P1 切換顏色

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("P1 Change Color");
        }

        
        //TODO:這裡可能要修改成狀態機加速
        //P1  左右移動
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!objP1.GetComponent<PlayerHandler>().isBusy)
            {
                objP1.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * -1, 0));
                objP1.GetComponent<PlayerHandler>().isBusy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!objP1.GetComponent<PlayerHandler>().isBusy)
            {
                objP1.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
                objP1.GetComponent<PlayerHandler>().isBusy = true;
            }
        }

        //P2
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            Debug.Log("P2 Change Color");
        }

        //P2  左右移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
           // objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * -1, 0));
            if (!objP2.GetComponent<PlayerHandler>().isBusy)
            {
                objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * -1, 0));
                objP2.GetComponent<PlayerHandler>().isBusy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
           // objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
            if (!objP2.GetComponent<PlayerHandler>().isBusy)
            {
                objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
                objP2.GetComponent<PlayerHandler>().isBusy = true;
            }
        }
    }
}