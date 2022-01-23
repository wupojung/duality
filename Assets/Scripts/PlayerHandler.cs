using System;
using System.Timers;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private float _horizontalSpeed = 40000.0f;
    private int _rank = 0;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    private AvatarStatus _avatarStatus;

    private AvatarType _avatarType;

    //private BlockItem _previousBlock;
    private BlockItem _blockType;
    private BlockItem _predictBlock;
    private bool _isCombe = false;
    private Timer _comboCountDownTimer;
    private int _interval = 2000; // 2 second 

    #region Unity core event

    
    void Start()
    {
        _avatarType = AvatarType.Black;
        _avatarStatus = AvatarStatus.Idle;

        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _comboCountDownTimer = new System.Timers.Timer {Interval = _interval};
        _comboCountDownTimer.Elapsed += new System.Timers.ElapsedEventHandler(_TimersTimer_Elapsed);
        _comboCountDownTimer.Start();
    }



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
        if (!GameMgr.IsDebug)
        {
            return;
        }
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;


        GUIStyle blockStyle = new GUIStyle();
        blockStyle.fontSize = 30;
        blockStyle.normal.textColor = Color.white;


        GUIStyle avatarStyle = new GUIStyle();
        avatarStyle.fontSize = 30;
        avatarStyle.normal.textColor = Color.white;

        GUIStyle comboStyle = new GUIStyle();
        comboStyle.fontSize = 30;
        comboStyle.normal.textColor = Color.white;

        int x = 50;
        if (gameObject.name == "P2")
        {
            x += 150;
        }

        if (_blockType?.currentType == BlockType.BLACK)
        {
            blockStyle.normal.textColor = Color.black;
        }

        if (_avatarType == AvatarType.Black)
        {
            avatarStyle.normal.textColor = Color.black;
        }


        GUI.Label(new Rect(x, 0, 150, 50), _blockType?.currentType.ToString(), blockStyle);
        GUI.Label(new Rect(x, 50, 150, 50), _avatarType.ToString(), avatarStyle);
        GUI.Label(new Rect(x, 100, 150, 50), _avatarStatus.ToString(), style);

        if (_isCombe)
        {
            comboStyle.normal.textColor = Color.red;
            GUI.Label(new Rect(x, 150, 150, 50), "combo！", comboStyle);
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
        BlockItem block = other.GetComponent<BlockItem>();
        if (block != null)
        {
            _blockType = block;
            _rank = GetRankByTwoBlockType();
        }

        if (other.name == "DeathLine")
        {
            GameMgr.Audio.PlayOutOfScreen();
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
                _comboCountDownTimer.Interval = _interval;
                _comboCountDownTimer.Start();
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
    public int GetRankByTwoBlockType()
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

    #region Support Function

    void _TimersTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        _isCombe = false;
    }

    #endregion
}