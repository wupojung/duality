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
    public float comboSpeed = 20.0f;
    public GameObject markObj;

    public float horizontalSpeed = 40000.0f;


    //移動面板(底板，block)
    public float verticalSpeed = 2000.0f;
    public GameObject scrollPanel;
    private IList<GameObject> _scrollPanelList; //子物件
    private int _scrollPanelIndex = 0; // 索引


    //碰撞牆面
    public GameObject wallPanel;

    // game logic
    public GameObject GameOverPanel;
    public Sprite win1;
    public Sprite win2;
    private GameObject _winPanel;
    public Animator anim;

    //粒子特效用
    public GameObject ParticleSystemCameraPanel;
    private ParticleSystem _particleForPlayer1;
    private ParticleSystem _particleForPlayer2;


    #region Unity core event

    void Start()
    {
        ScanPanelInScrollPanel();
        ScanParticleSystemInParticleSystemCameraPanel();

        _player1 = objP1.GetComponent<PlayerHandler>();
        _player2 = objP2.GetComponent<PlayerHandler>();

        //setup parms
        _player1.SetHorizontalSpeed(horizontalSpeed);
        _player2.SetHorizontalSpeed(horizontalSpeed);

        ScanPanelInWallPanel();

        GameOverPanel.SetActive(false);
        _winPanel = GameOverPanel.transform.GetChild(0).gameObject;
    }


    void Update()
    {
        //確認動畫播放完畢
        if (!GameMgr.IsGameStart && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            anim.gameObject.SetActive(false);
            GameMgr.IsGameStart = true;
        }

        if (GameMgr.IsGameStart)
        {
            ScrollPaneMovement();
            ProcessAvatarInput();
            CalculateDistance();
            CalculateCombo();
        }

        if (GameMgr.IsGameOver)
        {
            _winPanel.GetComponent<Image>().sprite = win1;
            _winPanel.SetActive(true);
            GameOverPanel.SetActive(true);
        }
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
                _player1.ChangeAvatarType();
                _particleForPlayer1.Play();
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
                _player2.ChangeAvatarType();
                _particleForPlayer2.Play();
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
                GameObject temp = scrollPanel.transform.GetChild(i).gameObject;
                _scrollPanelList.Add(temp);

                RandomScrollPanel(temp);
                // foreach (var blockItem in temp.GetComponentsInChildren<BlockItem>())
                // {
                //     blockItem.Refresh();
                // }
            }

            // Debug.Log($"{_scrollPanelList.Count}");
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }
    }


    private void ScanParticleSystemInParticleSystemCameraPanel()
    {
        try
        {
            if (ParticleSystemCameraPanel == null)
            {
                throw new ArgumentNullException("ParticleSystemCameraPanel", "請重新設定ParticleSystemCameraPanel");
            }

            int childCount = ParticleSystemCameraPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (ParticleSystemCameraPanel.transform.GetChild(i).name.Contains("Player1"))
                {
                    _particleForPlayer1 = ParticleSystemCameraPanel.transform.GetChild(i).GetChild(0)
                        .GetComponent<ParticleSystem>();
                }

                if (ParticleSystemCameraPanel.transform.GetChild(i).name.Contains("Player2"))
                {
                    _particleForPlayer2 = ParticleSystemCameraPanel.transform.GetChild(i).GetChild(0)
                        .GetComponent<ParticleSystem>();
                }
            }

            //check again 
            if (_particleForPlayer1 == null || _particleForPlayer2 == null)
            {
                throw new ArgumentNullException("_particleForPlayer1 or _particleForPlayer2",
                    "_particleForPlayer1 or 2 was empty");
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            Console.WriteLine(exp);
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

            GameObject panel = _scrollPanelList[_scrollPanelIndex]; //cache

            //動態計算便宜的
            float offsetY = panel.GetComponent<GridLayoutGroup>().cellSize.y
                            * (float) panel.transform.childCount
                            / 2.0f;


            //高度 250，  兩倍再移開 ，故為 250*5*2 = 2500
            if (panel.transform.position.y > offsetY * 2)
            {
                Vector3 v3 = panel.transform.localPosition;
                v3.y -= offsetY * _scrollPanelList.Count; // 255*5*3
                panel.transform.localPosition = v3;

                //TODO：reset 面板內容 （可能要人控）
                RandomScrollPanel(panel);

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
            for (int i = 0; i < count; i += 2)
            {
                BlockItem left = panelRoot.transform.GetChild(i).GetComponent<BlockItem>();
                BlockItem right = panelRoot.transform.GetChild(i + 1).GetComponent<BlockItem>();

                // Debug.Log($"setup {left.name}  +  {right.name} ");

                int randomIndex = Random.Range(0, MapResourceHelper.Data.Count);
                //左邊
                if (Random.Range(0, 2) == 1) //隨機配色  
                {
                    left.currentType = BlockType.WHITE;
                    left.GetImage().sprite = MapResourceHelper.Data[randomIndex].Day.Left;
                    //  Debug.Log("L->W  name=" + left.GetImage().sprite.name);
                }
                else
                {
                    left.currentType = BlockType.BLACK;
                    left.GetImage().sprite = MapResourceHelper.Data[randomIndex].Night.Left;
                    // Debug.Log("L->B  name=" + left.GetImage().sprite.name);
                }

                //右邊
                if (Random.Range(0, 2) == 1) //隨機配色  
                {
                    right.currentType = BlockType.WHITE;
                    right.GetImage().sprite = MapResourceHelper.Data[randomIndex].Day.Right;
                    // Debug.Log("R->W  name=" + right.GetImage().sprite.name);
                }
                else
                {
                    right.currentType = BlockType.BLACK;
                    right.GetImage().sprite = MapResourceHelper.Data[randomIndex].Night.Right;
                    //Debug.Log("R->B  name=" + right.GetImage().sprite.name);
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
            if (_player1.IsCombe)
            {
                _player1.PlayBooster();
                _distance -= comboSpeed;
            }

            if (_player2.IsCombe)
            {
                _player2.PlayBooster();
                _distance += comboSpeed;
            }
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