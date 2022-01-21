using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class S3Mgr : MonoBehaviour
{
 
    public GameObject objP1;
    public GameObject objP2;

    public float horizontalSpeed = 40000.0f;

    //移動面板
    public float verticalSpeed = 2000.0f;
    public GameObject scrollPanel;
    private IList<GameObject> _scrollPanelList; //子物件
    private int _scrollPanelIndex = 0; // 索引

    #region Unity core event

    void Start()
    {
        ScanPanelInScrollPanel();
    }


    void Update()
    {
        ScrollPaneMovement();

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
                objP1.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed * -1, 0));
                objP1.GetComponent<PlayerHandler>().isBusy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (!objP1.GetComponent<PlayerHandler>().isBusy)
            {
                objP1.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed, 0));
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
                objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed * -1, 0));
                objP2.GetComponent<PlayerHandler>().isBusy = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(speed, 0));
            if (!objP2.GetComponent<PlayerHandler>().isBusy)
            {
                objP2.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed, 0));
                objP2.GetComponent<PlayerHandler>().isBusy = true;
            }
        }
    }
    #endregion

    #region Support function（private）

    /// <summary>
    /// 掃描物件
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    private void ScanPanelInScrollPanel()
    {
        try
        {
            if (scrollPanel == null)
            {
                throw new NullReferenceException("scrollPanel  was null ");
            }

            _scrollPanelList = new List<GameObject>();
            int childCount = scrollPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                _scrollPanelList.Add(scrollPanel.transform.GetChild(i).gameObject);
            }

            Debug.Log($"{_scrollPanelList.Count}");
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

    /// <summary>
    /// 移動面板
    /// </summary>
    private void ScrollPaneMovement()
    {
        try
        {
            //scroll panel movement 
            Vector3 pos = scrollPanel.transform.localPosition;
            pos.y += verticalSpeed * Time.deltaTime;
            scrollPanel.transform.localPosition = pos;

            //高度 255，  兩倍再移開 ，故為 255*5*2 = 2550
            if (_scrollPanelList[_scrollPanelIndex].transform.position.y > 2550)
            {
                Vector3 v3 = _scrollPanelList[_scrollPanelIndex].transform.localPosition;
                v3.y -= 3825; // 255*5*3
                _scrollPanelList[_scrollPanelIndex].transform.localPosition = v3;
        
                //TODO：reset 面板內容 （可能要人控）
                RandomScrollPanel(_scrollPanelList[_scrollPanelIndex]);
                
                //更新index,並修正
                _scrollPanelIndex++;
                if (_scrollPanelIndex == _scrollPanelList.Count)
                {
                    _scrollPanelIndex = 0;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// 亂數面板產生
    /// </summary>
    /// <param name="panelRoot"></param>
    private void RandomScrollPanel(GameObject panelRoot)
    {
        int count = panelRoot.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            panelRoot.transform.GetChild(i).GetComponent<BlockItem>()?.RandomType();
        }
    }

    #endregion
}