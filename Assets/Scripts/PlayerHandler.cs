using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerHandler : MonoBehaviour
{
    private AvatarStatus _avatarStatus;

    private AvatarType _avatarType;
    private BlockItem _blockType;
    private BlockItem _predictBlock;

    private float _horizontalSpeed = 40000.0f;


    private Rigidbody2D _rigidbody2D;

    public CircleCollider2D circleCollider2D;
    private int _rank = 0;


    #region Unity core event

    // Start is called before the first frame update
    void Start()
    {
        _avatarType = AvatarType.Black;
        _avatarStatus = AvatarStatus.Idle;

        _rigidbody2D = GetComponent<Rigidbody2D>();
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
        ;
        _blockStyle.fontSize = 30;
        _blockStyle.normal.textColor = Color.white;


        GUIStyle _avatarStyle = new GUIStyle();
        ;
        _avatarStyle.fontSize = 30;
        _avatarStyle.normal.textColor = Color.white;

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
        _blockType = block;
        _rank = GetRank111();
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
                break;
            case AvatarType.White:
                _avatarType = AvatarType.Black;
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
            }
        }

        // _predictBlock.currentType == BlockType.BLACK
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