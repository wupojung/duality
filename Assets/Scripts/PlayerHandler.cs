using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEditor.Animations;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHandler : MonoBehaviour
{
    public AnimatorController dayController;

    public AnimatorController nightController;
    private Animator _animator;


    private AvatarStatus _avatarStatus;

    private AvatarType _avatarType;
    private BlockItem _previousBlock;
    private BlockItem _blockType;
    private BlockItem _predictBlock;

    private float _horizontalSpeed = 40000.0f;


    private Rigidbody2D _rigidbody2D;

    private int _rank = 0;

    private bool _isCombe = false;

    private Timer combeCountDownTimer;

    #region Unity core event

    // Start is called before the first frame update
    void Start()
    {
        _avatarType = AvatarType.Black;
        _avatarStatus = AvatarStatus.Idle;

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        combeCountDownTimer = new System.Timers.Timer();
        combeCountDownTimer.Interval = 2000;
        combeCountDownTimer.Elapsed += new System.Timers.ElapsedEventHandler(_TimersTimer_Elapsed);
        combeCountDownTimer.Start();
    }

    void _TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        //在這裡做想做的事    
        _isCombe = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_avatarStatus)
        {
            case AvatarStatus.Left:
                _rigidbody2D.AddForce(new Vector2(_horizontalSpeed * -1 * Time.deltaTime, 0));
                break;
            case AvatarStatus.Right:
                _rigidbody2D.AddForce(new Vector2(_horizontalSpeed * 1 * Time.deltaTime, 0));
                break;
            case AvatarStatus.Idle:
                break;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;


        GUIStyle _blockStyle = new GUIStyle();
        _blockStyle.fontSize = 30;
        _blockStyle.normal.textColor = Color.white;


        GUIStyle _avatarStyle = new GUIStyle();
        _avatarStyle.fontSize = 30;
        _avatarStyle.normal.textColor = Color.white;

        GUIStyle _comboStyle = new GUIStyle();
        _comboStyle.fontSize = 30;
        _comboStyle.normal.textColor = Color.white;

        int x = 50;
        if (gameObject.name == "P2")
        {
            x += 150;
        }

        if (_blockType.currentType == BlockType.BLACK)
        {
            _blockStyle.normal.textColor = Color.black;
        }

        if (_avatarType == AvatarType.Black)
        {
            _avatarStyle.normal.textColor = Color.black;
        }


        GUI.Label(new Rect(x, 0, 150, 50), _blockType.currentType.ToString(), _blockStyle);
        GUI.Label(new Rect(x, 50, 150, 50), _avatarType.ToString(), _avatarStyle);
        GUI.Label(new Rect(x, 100, 150, 50), _avatarStatus.ToString(), style);

        if (_isCombe)
        {
            _comboStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(x, 150, 150, 50), "combo！", _comboStyle);
        }
    }

    #endregion

    #region Unity Events

    public void OnCollisionEnter2D(Collision2D other)
    {
        Wall wall = other.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            this._avatarStatus = AvatarStatus.Idle;
        }
    }

    public void OnCollisionExit2D(Collision2D other)
    {
        this._avatarStatus = AvatarStatus.Idle;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        _previousBlock = _blockType; //備份 前一個狀態
        BlockItem block = other.GetComponent<BlockItem>();
        if (block != null)
        {
            _blockType = block;
            _rank = GetRank111();
        }

        if (other.name=="DeathLine")
        {
            
            GameMgr.IsGameOver = true;

        }
    }

    #endregion


    #region Public Function

    public void SetPredictBlock(BlockItem item)
    {
        _predictBlock = item;
    }

    public void SetHorizontalSpeed(float speed)
    {
        _horizontalSpeed = speed;
    }

    public void ChangeStatus(AvatarStatus status)
    {
        _avatarStatus = status;
    }

    public void ChangeAvatarType()
    {
        switch (_avatarType)
        {
            case AvatarType.Black:
                _avatarType = AvatarType.White;
                _animator.SetBool("ChangeColorToBlack", false);
                // animator.runtimeAnimatorController = dayController;
                break;
            case AvatarType.White:
                _avatarType = AvatarType.Black;
                _animator.SetBool("ChangeColorToBlack", true);
                // animator.runtimeAnimatorController = nightController;
                break;
        }

        //切換顏色時進行預判 （combo)
        //_predictBlock
        //目前跟 預知一樣 表示 combo
        if (_predictBlock != null)
        {
            if ((_avatarType == AvatarType.Black && _predictBlock.currentType == BlockType.BLACK)
                || (_avatarType == AvatarType.White && _predictBlock.currentType == BlockType.WHITE))
            {
                Debug.Log("Combo!!");
                _isCombe = true;
                //啟動計時器 關閉
                combeCountDownTimer.Interval = 2000;
                combeCountDownTimer.Start();
            }
        }

        // _predictBlock.currentType == BlockType.BLACK
    }


    public void PlayBooster()
    {
        if (_isCombe)
        {
            _animator.SetTrigger("Booster");
            _isCombe = false;
        }    
    }
    
    
    public bool IsCombe
    {
        get => _isCombe;
        set => _isCombe = value;
    }

    public int GetRank()
    {
        return _rank;
    }

    /// <summary>
    /// 取得權重 (同色+1 ，異色-1）
    /// </summary>
    /// <returns></returns>
    public int GetRank111()
    {
        int result = 0;
        try
        {
            //加速
            if ((_blockType.currentType == BlockType.BLACK && _avatarType == AvatarType.Black) ||
                (_blockType.currentType == BlockType.WHITE && _avatarType == AvatarType.White))
            {
                result = 1;
            }

            //減速
            if ((_blockType.currentType == BlockType.BLACK && _avatarType == AvatarType.White) ||
                (_blockType.currentType == BlockType.WHITE && _avatarType == AvatarType.Black))
            {
                result = -1;
                _isCombe = false; //斷招，停combo
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

    #endregion
}