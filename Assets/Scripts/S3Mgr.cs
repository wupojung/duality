using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class S3Mgr : MonoBehaviour
{
    #region Config

    [Header("遊戲時間")] public int gameTimer = 30; //30 sec

    [Header("地圖速度(垂直)")] public float verticalSpeed = 2000.0f;

    [Header("左右移動速度(水平)")] public float horizontalSpeed = 40000.0f;


    [Header("變身CD時間")] public float transformationInterval = 200.0f;

    [Header("加速速度")] public float boosterSpeed = 0.1f;

    [Header("加速持續時間")] public float boosterInterval = 2000.0f;

    #endregion

    #region Variables Area

    // -- core panel
    private GameObject _bgPanel;
    private GameObject _fgPanel;
    private GameObject _uiPanel;

    // -- Gameplay
    private GameObject _gameOverPanel;
    private GameObject _winPanel;
    private Button _btnContinue; //繼續按鈕
    private int _duration; //遊戲持續時間


    // -- 玩家相關
    private PlayerHandler _player1;
    private PlayerHandler _player2;

    // -- 模擬跑步差距
    private GameObject _centerLineObj; //中心線物件(拿來計算用）
    private float _distance = 0.0f;

    // -- 地圖模組
    private GameObject _scrollPanel;
    private IList<GameObject> _scrollPanelList; //子物件
    private int _scrollPanelIndex = 0; // 索引
    private GameObject _wallPanel; // 牆面

    // -- 動畫模組
    private Animator _openAnimator;
    private Animator _winnerAnimator;
    private Animator _hurryUpAnimator;

    private Animator _timesUpAnimator;

    // -- 粒子特效用
    private GameObject _particleSystemCameraPanel;
    private ParticleSystem _particleForPlayer1;
    private ParticleSystem _particleForPlayer2;

    // -- core timer 
    private Timer _preSecondTimer;

    #endregion

    #region Unity Core Event

    private void Awake()
    {
        //如果Audio沒有啟動，直接帶回S1 (引擎沒有啟動)
        if (GameMgr.Audio == null)
        {
            SceneManager.LoadScene("S1");
        }
    }

    private void Start()
    {
        ScanRequirementGameObject();

        ScanFG();
        ScanUI();
        ScanParticleSystemCameraPanel();

        ResetConfig(); //設定參數
        ResetForGameStart(); //重設遊戲
    }

    private void CheckIsTimesUp()
    {
        if (_duration > gameTimer - 10)
        {
            //播放警告畫面
            Debug.LogWarning("!!! hurry up !!");
            _hurryUpAnimator.gameObject.SetActive(true);
        }

        if (_duration > gameTimer)
        {
            _timesUpAnimator.gameObject.SetActive(true);
     
            GameMgr.IsTimesUp = true;
        }
    }

    private void Update()
    {
        //確認動畫播放完畢
        if (!GameMgr.IsGameStart && _openAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            _openAnimator.gameObject.SetActive(false);
            GameMgr.IsGameStart = true;
            _preSecondTimer?.Start(); //遊戲開始計時
        }
        
        if (GameMgr.IsTimesUp && _timesUpAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            GameMgr.IsGameOver = true;
        }

        if (GameMgr.IsGameStart)
        {
            CheckIsTimesUp();
            ScrollPaneMovement();
            ProcessAvatarInput();
            CalculateDistance();
            CalculateCombo();
        }

        if (GameMgr.IsGameOver)
        {
            _winnerAnimator.SetBool("p2win", CheckIsP2Win());
            _winPanel.SetActive(true);
            _gameOverPanel.SetActive(true);
        }

      
    }

    private void OnGUI()
    {
        if (!GameMgr.IsDebug)
        {
            return;
        }

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

    private void ResetConfig()
    {
        _player1.SetHorizontalSpeed(horizontalSpeed);
        _player1.SetBoosterCoolDownTime(boosterInterval);
        _player1.SetTransformationCoolDownTime(transformationInterval);
        _player1.SetDayColor(new Color32(124, 164, 81,255));
        _player1.SetNightColor(new Color32(61, 95, 135,255));

        
        _player2.SetHorizontalSpeed(horizontalSpeed);
        _player2.SetBoosterCoolDownTime(boosterInterval);
        _player2.SetTransformationCoolDownTime(transformationInterval);
        _player2.SetDayColor(new Color32(124, 164, 81,255));
        _player2.SetNightColor(new Color32(61, 95, 135,255));

    }

    private void ResetForGameStart()
    {
        GameMgr.ResetGame();
        
        //for all panel 
        _uiPanel.SetActive(true);
        _gameOverPanel.SetActive(false);
        _hurryUpAnimator.gameObject.SetActive(false);

        //啟動timer
        _preSecondTimer = new System.Timers.Timer {Interval = 1000}; //固定1秒
        _preSecondTimer.AutoReset = true;
        _preSecondTimer.Elapsed += new System.Timers.ElapsedEventHandler(GameTimer_Elapsed);
        // _gameTimer.Start();
    }

    #region Scan

    private void ScanRequirementGameObject()
    {
        GameObject mainCamera = ScanHelper.ScanGameObjectByName(this.gameObject, "Main Camera");

        GameObject canvas = ScanHelper.ScanGameObjectByName(mainCamera, "Canvas");

        _fgPanel = ScanHelper.ScanGameObjectByName(canvas, "FG");

        _uiPanel = ScanHelper.ScanGameObjectByName(canvas, "UI");

        _particleSystemCameraPanel = ScanHelper.ScanGameObjectByName(this.gameObject, "3rdCamera");
    }

    #region Scan  FG

    private void ScanFG()
    {
        //-- OperationArea
        GameObject OperationArea = ScanHelper.ScanGameObjectByName(_fgPanel, "OperationArea");

        //-- OperationArea/ScrollPanel
        _scrollPanel = ScanHelper.ScanGameObjectByName(OperationArea, "ScrollPanel");
        ScanPanelInScrollPanel();

        //-- OperationArea/Player
        GameObject playerPanel = ScanHelper.ScanGameObjectByName(OperationArea, "Player");
        GameObject p1 = ScanHelper.ScanGameObjectByName(playerPanel, "P1");
        GameObject p2 = ScanHelper.ScanGameObjectByName(playerPanel, "P2");

        _player1 = p1.GetComponent<PlayerHandler>();
        _player2 = p2.GetComponent<PlayerHandler>();

        _centerLineObj = ScanHelper.ScanGameObjectByName(playerPanel, "Mark");


        //-- OperationArea/WallPanel
        _wallPanel = ScanHelper.ScanGameObjectByName(OperationArea, "WallPanel");
        ScanPanelInWallPanel();

        GameObject alertPanel = _wallPanel = ScanHelper.ScanGameObjectByName(OperationArea, "AlertPanel");
        if (alertPanel != null)
        {
            _hurryUpAnimator = alertPanel.GetComponent<Animator>();
        }
        alertPanel.SetActive(false);
        
        GameObject TimeUpPanel = _wallPanel = ScanHelper.ScanGameObjectByName(OperationArea, "TimeUpPanel");
        if (alertPanel != null)
        {
            _timesUpAnimator = alertPanel.GetComponent<Animator>();
        }
        TimeUpPanel.SetActive(false);
        
        //
        //-- L
        //-- R
    }

    /// <summary>
    /// 掃描物件
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    private void ScanPanelInScrollPanel()
    {
        try
        {
            if (_scrollPanel == null)
            {
                throw new NullReferenceException("scrollPanel  was null ");
            }

            _scrollPanelList = new List<GameObject>();
            int childCount = _scrollPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                GameObject temp = _scrollPanel.transform.GetChild(i).gameObject;
                _scrollPanelList.Add(temp);

                RandomScrollPanel(temp);
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
    /// 掃描牆壁（同時修改成透明）
    /// </summary>
    private void ScanPanelInWallPanel()
    {
        try
        {
            int childCount = _wallPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Image wall = _wallPanel.transform.GetChild(i).GetComponentInChildren<Image>();
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

    #endregion

    #region Scan UI

    private void ScanUI()
    {
        //---
        GameObject StartCountDown = ScanHelper.ScanGameObjectByName(_uiPanel, "StartCountDown");
        _openAnimator = StartCountDown.GetComponent<Animator>();

        //---
        _gameOverPanel = ScanHelper.ScanGameObjectByName(_uiPanel, "GameOverPanel");
        ScanGamOverPanel();
    }

    private void ScanGamOverPanel()
    {
        _winPanel = ScanHelper.ScanGameObjectByName(_gameOverPanel, "WinPanel");
        _winnerAnimator = _winPanel.GetComponent<Animator>();

        GameObject continueObj = ScanHelper.ScanGameObjectByName(_gameOverPanel, "btnContinue");
        if (continueObj != null)
        {
            _btnContinue = continueObj.GetComponent<Button>();
            _btnContinue.onClick.AddListener(OnBtnContinueClick);
        }
    }

    #endregion

    /// <summary>
    /// 掃描粒子攝影機面板
    /// </summary>
    /// <exception cref="ArgumentNullException"></exception>
    private void ScanParticleSystemCameraPanel()
    {
        try
        {
            if (_particleSystemCameraPanel == null)
            {
                throw new ArgumentNullException("ParticleSystemCameraPanel", "請重新設定ParticleSystemCameraPanel");
            }

            int childCount = _particleSystemCameraPanel.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                if (_particleSystemCameraPanel.transform.GetChild(i).name.Contains("Player1"))
                {
                    _particleForPlayer1 = _particleSystemCameraPanel.transform.GetChild(i).GetChild(1)
                        .GetComponent<ParticleSystem>();
                }

                if (_particleSystemCameraPanel.transform.GetChild(i).name.Contains("Player2"))
                {
                    _particleForPlayer2 = _particleSystemCameraPanel.transform.GetChild(i).GetChild(1)
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

    #endregion

    private bool CheckIsP2Win()
    {
        bool result = false;
        try
        {
            if (_player1.gameObject.transform.localPosition.y > _player2.gameObject.transform.localPosition.y)
            {
                result = true;
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            Debug.LogError(exp.ToString());
            throw;
        }

        return result;
    }

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
                GameMgr.Audio.PlayChangeColor();
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
                GameMgr.Audio.PlayChangeColor();
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
    /// 移動面板
    /// </summary>
    private void ScrollPaneMovement()
    {
        try
        {
            //scroll panel movement 
            Vector3 pos = _scrollPanel.transform.localPosition;
            pos.y += verticalSpeed * Time.deltaTime;
            _scrollPanel.transform.localPosition = pos;

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
                Vector3 p1v3 = new Vector3(_player1.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                _player1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(_player2.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                _player2.transform.localPosition = p2v3;
            }

            if (_distance > 0)
            {
                Vector3 p1v3 = new Vector3(_player1.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                p1v3.y += Math.Abs(_distance / 2);
                _player1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(_player2.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                p2v3.y -= Math.Abs(_distance / 2);
                _player2.transform.localPosition = p2v3;
            }

            if (_distance < 0)
            {
                Vector3 p1v3 = new Vector3(_player1.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                p1v3.y -= Math.Abs(_distance / 2);
                _player1.transform.localPosition = p1v3;

                Vector3 p2v3 = new Vector3(_player2.transform.localPosition.x, _centerLineObj.transform.localPosition.y,
                    0);
                p2v3.y += Math.Abs(_distance / 2);
                _player2.transform.localPosition = p2v3;
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
                GameMgr.Audio.PlaySpeedUp();
                _player1.PlayBooster();
                _distance -= boosterSpeed;
            }

            if (_player2.IsCombe)
            {
                GameMgr.Audio.PlaySpeedUp();
                _player2.PlayBooster();
                _distance += boosterSpeed;
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

    #region Events

    /// <summary>
    /// 計算按鈕
    /// </summary>
    private void OnBtnContinueClick()
    {
        GameMgr.Audio.PlayClick();
        SceneManager.LoadScene("S2");
    }

    private void GameTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        _duration++;
    }

    #endregion
}