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
    private PlayerHandler _player1;
    private PlayerHandler _player2;

    private float _distance = 0.0f;
    public GameObject markObj;

    public float horizontalSpeed = 40000.0f;


    //移動面板(底板，block)
    public float verticalSpeed = 2000.0f;
    public GameObject scrollPanel;
    private IList<GameObject> _scrollPanelList; //子物件
    private int _scrollPanelIndex = 0; // 索引


    //碰撞牆面
    public GameObject wallPanel;

    #region Unity core event

    void Start()
    {
        ScanPanelInScrollPanel();

        _player1 = objP1.GetComponent<PlayerHandler>();
        _player2 = objP2.GetComponent<PlayerHandler>();

        //setup parms
        _player1.SetHorizontalSpeed(horizontalSpeed);
        _player2.SetHorizontalSpeed(horizontalSpeed);

        ScanPanelInWallPanel();
    }


    void Update()
    {
        ScrollPaneMovement();
        ProcessAvatarInput();
        CalculateDistance();
        CalculateCombo();
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(0, 180, 100, 50), _distance.ToString());


        if (GUI.Button(new Rect(0, 200, 100, 50), "+"))
        {
            _distance += 100;
        }

        if (GUI.Button(new Rect(0, 250, 100, 50), "-"))
        {
            _distance -= 100;
        }
    }

    #endregion

    #region Support function（private）

    /// <summary>
    /// 處理移動輸入(input)
    /// </summary>
    private void ProcessAvatarInput()
    {
        try
        {
            //P1 
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            {
                //Debug.Log("P1 Change Color");
                _player1.ChangeAvatarType();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _player1.ChangeStatus(AvatarStatus.Left);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _player1.ChangeStatus(AvatarStatus.Right);
            }

            //P2
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                //Debug.Log("P2 Change Color");
                _player2.ChangeAvatarType();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                _player2.ChangeStatus(AvatarStatus.Left);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                _player2.ChangeStatus(AvatarStatus.Right);
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

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
    /// 掃描牆壁（同時修改成透明）
    /// </summary>
    private void ScanPanelInWallPanel()
    {
        try
        {
            int childCount = wallPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Image wall = wallPanel.transform.GetChild(i).GetComponentInChildren<Image>();
                Color color = wall.color;
                color.a = 0;
                wall.color = color;
            }
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
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

    /// <summary>
    /// 亂數面板產生
    /// </summary>
    /// <param name="panelRoot"></param>
    private void RandomScrollPanel(GameObject panelRoot)
    {
        try
        {
            int count = panelRoot.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                panelRoot.transform.GetChild(i).GetComponent<BlockItem>()?.RandomType();
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

    /// <summary>
    /// 計算距離
    /// </summary>
    private void CalculateDistance()
    {
        try
        {
        
            //計算

            //for debug mode （計算理論）
            // int r1 = player1.GetRank();
            // int r2 = player2.GetRank();
            //
            // //Debug.Log($"{r1} - {r2} = {r1 - r2}   ||| {distance} - {r1 - r2} =  {distance - (r1 - r2)}");
            //
            // distance -= (r1 - r2);

            _distance -= (_player1.GetRank() - _player2.GetRank());
            // 顯示
            if (_distance == 0)
            {
                Vector3 p1v3 = new Vector3(objP1.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                objP1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(objP2.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                objP2.transform.localPosition = p2v3;
            }

            if (_distance > 0)
            {
                Vector3 p1v3 = new Vector3(objP1.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                p1v3.y += Math.Abs(_distance / 2);
                objP1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(objP2.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                p2v3.y -= Math.Abs(_distance / 2);
                objP2.transform.localPosition = p2v3;
            }

            if (_distance < 0)
            {
                Vector3 p1v3 = new Vector3(objP1.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                p1v3.y -= Math.Abs(_distance / 2);
                objP1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(objP2.transform.localPosition.x, markObj.transform.localPosition.y, 0);
                p2v3.y += Math.Abs(_distance / 2);
                objP2.transform.localPosition = p2v3;
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

    /// <summary>
    /// 計算Combo (額外加速）
    /// </summary>
    private void CalculateCombo()
    {
        try
        {
            //TODO: 計算 combo邏輯
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }

    #endregion
}