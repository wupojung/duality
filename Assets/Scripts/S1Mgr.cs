using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S1Mgr : MonoBehaviour
{
    public float delaySeconds = .5f;

    public GameObject PressKeyText, PressKeyText2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ChangeColor());
    }

    IEnumerator ChangeColor()
    {
        //WaitUntil waitForFlag = new WaitUntil(() => doChangeColor);

        while (true)
        {
            //yield return waitForFlag;

            PressKeyText.active = !PressKeyText.active;
            PressKeyText2.active = !PressKeyText2.active;
            yield return new WaitForSeconds(delaySeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            GameMgr.Audio.PlayClick();
            Debug.Log("press enter !! ");
            SceneManager.LoadScene("S3");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameMgr.Audio.PlayClick();
            Debug.Log("press enter !! ");
            SceneManager.LoadScene("S2");
        }
    }
}