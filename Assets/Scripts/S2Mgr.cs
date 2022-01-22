using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class S2Mgr : MonoBehaviour
{
    public GameObject PressKeyText;
    public GameObject w_key, s_key, d_Key, a_key;
    public GameObject up_key, down_key, right_Key, left_key;
    public Sprite w_keySprite_down, s_keySprite_down, d_KeySprite_down, a_keySprite_down;
    public Sprite up_keySprite_down, down_keySprite_down, right_KeySprite_down, left_keySprite_down;



    public Sprite w_keySprite_keyUp, s_keySprite_keyUp, d_KeySprite_keyUp, a_keySprite_keyUp;
    public Sprite up_keySprite_keyUp, down_keySprite_keyUp, right_KeySprite_keyUp, left_keySprite_keyUp;



    public SpriteRenderer spriteRenderer_w, spriteRenderer_a, spriteRenderer_s, spriteRenderer_d;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer_w = w_key.GetComponent<SpriteRenderer>();
        spriteRenderer_a = a_key.GetComponent<SpriteRenderer>();
        spriteRenderer_s = s_key.GetComponent<SpriteRenderer>();
        spriteRenderer_d = d_Key.GetComponent<SpriteRenderer>();


        //StartCoroutine(ExampleCoroutine());
        StartCoroutine(ChangeColor());
    }
    IEnumerator ExampleCoroutine()
    {
        ChgTextStatus();
        yield return new WaitForSeconds(2);

    }
    void TextControlVisible()
    {
        PressKeyText.active = !PressKeyText.active;
       

    }
    public float delaySeconds = .5f;

    IEnumerator ChangeColor()
    {
        //WaitUntil waitForFlag = new WaitUntil(() => doChangeColor);

        while (true)
        {
            //yield return waitForFlag;

            PressKeyText.active = !PressKeyText.active;

            yield return new WaitForSeconds(delaySeconds);
        }
    }

    void Chg_w()
    {
      w_key.gameObject.GetComponent<Image>().sprite = w_keySprite_keyUp;

    }
    void Chg_a()
    {
        a_key.gameObject.GetComponent<Image>().sprite = a_keySprite_keyUp;

    }
    void Chg_s()
    {
        s_key.gameObject.GetComponent<Image>().sprite = s_keySprite_keyUp;

    }
    void Chg_d()
    {
        d_Key.gameObject.GetComponent<Image>().sprite = d_KeySprite_keyUp;

    }



    void Chg_up()
    {
        up_key.gameObject.GetComponent<Image>().sprite = up_keySprite_keyUp;

    }
    void Chg_down()
    {
        down_key.gameObject.GetComponent<Image>().sprite = down_keySprite_keyUp;

    }
    void Chg_left()
    {
        left_key.gameObject.GetComponent<Image>().sprite = left_keySprite_keyUp;

    }
    void Chg_right()
    {
        right_Key.gameObject.GetComponent<Image>().sprite = right_KeySprite_keyUp;

    }

    void ChgTextStatus()
    {
        Invoke("TextControlVisible", 2F);
    }
    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.W))
        {
            w_key.gameObject.GetComponent<Image>().sprite = w_keySprite_down;
            
            Invoke("Chg_w", .5F);
       
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            a_key.gameObject.GetComponent<Image>().sprite = a_keySprite_down;
            Invoke("Chg_a", .5F);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            s_key.gameObject.GetComponent<Image>().sprite = s_keySprite_down;
            Invoke("Chg_s", .5F);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            d_Key.gameObject.GetComponent<Image>().sprite = d_KeySprite_down;
            Invoke("Chg_d", .5F);
        }



        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            up_key.gameObject.GetComponent<Image>().sprite = up_keySprite_down;
            Invoke("Chg_up", .5F);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            down_key.gameObject.GetComponent<Image>().sprite = down_keySprite_down;
            Invoke("Chg_down", .5F);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            left_key.gameObject.GetComponent<Image>().sprite = left_keySprite_down;
            Invoke("Chg_left", .5F);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            right_Key.gameObject.GetComponent<Image>().sprite = right_KeySprite_down;
            Invoke("Chg_right", .5F);
        }

        
        //TextControlVisible();

    }
}
