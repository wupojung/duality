using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public AvatarStatus currentStatus;

    private AvatarType _currentType;
    private BlockItem _blockType;

    public float horizontalSpeed = 40000.0f;

    #region Unity core event

    // Start is called before the first frame update
    void Start()
    {
        _currentType = AvatarType.Black;
        currentStatus = AvatarStatus.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStatus)
        {
            case AvatarStatus.Left:
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed * -1, 0));
                break;
            case AvatarStatus.Right:
                this.GetComponent<Rigidbody2D>().AddForce(new Vector2(horizontalSpeed * 1, 0));
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

        if (_currentType == AvatarType.Black)
        {
            _avatarStyle.normal.textColor = Color.black;
        }


        GUI.Label(new Rect(x, 0, 150, 50), _blockType.currentType.ToString(), _blockStyle);
        GUI.Label(new Rect(x, 50, 150, 50), _currentType.ToString(), _avatarStyle);
        GUI.Label(new Rect(x, 100, 150, 50), currentStatus.ToString(), style);
    }

    #endregion

    #region Unity Events

    public void OnCollisionEnter2D(Collision2D other)
    {
        Wall wall = other.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            this.currentStatus = AvatarStatus.Idle;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        BlockItem block = other.GetComponent<BlockItem>();
        _blockType = block;
    }

    #endregion


    #region Public Function

    public void ChangeAvatarType()
    {
        switch (_currentType)
        {
            case AvatarType.Black:
                _currentType = AvatarType.White;
                break;
            case AvatarType.White:
                _currentType = AvatarType.Black;
                break;
        }
    }

    public int GetRank()
    {
        int result = 0;
        //加速
        if ((_blockType.currentType == BlockType.BLACK && _currentType == AvatarType.Black) ||
            (_blockType.currentType == BlockType.WHITE && _currentType == AvatarType.White))
        {
            result = 1;
        }

        //減速
        if ((_blockType.currentType == BlockType.BLACK && _currentType == AvatarType.White) ||
            (_blockType.currentType == BlockType.WHITE && _currentType == AvatarType.Black))
        {
            result = -1;
        }

        return result;
    }

    #endregion
}