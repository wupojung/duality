using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManager : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {

    // }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayPlayerBoosterAni(1);
        }
    }

    #region Singleton

    public static PlayerAnimationManager instance;

    private void Instance()
    {
        instance = this;
    }

    #endregion

    #region PlayerAnimators

    public Animator Player1;
    public Animator Player2;

    #endregion

    public PlayerAnimationEvents p1AniEvents;
    public PlayerAnimationEvents p2AniEvents;

    private void Awake()
    {
        Instance();
    }

    public void ResetAnimatorParams()
    {
        Player1.ResetTrigger("Burst");
        Player2.ResetTrigger("Burst");
    }

    /// <summary>
    /// 控制P1切換到哪個顏色的Bool
    /// </summary>
    public bool p1isWhite = true;

    /// <summary>
    /// 控制P2切換到哪個顏色的Bool
    /// </summary>
    bool p2isWhite = true;

    /// <summary>
    /// P1切換顏色
    /// </summary>
    public void PlayPlayerChangeColorAni(int playerID)
    {
        if (playerID == 1)
        {
            p1isWhite = !p1isWhite;
            Player1.SetBool("ChangeColorToBlack", p1isWhite);
        }
        else
        {
            p2isWhite = !p2isWhite;
            Player2.SetBool("ChangeColorToBlack", p2isWhite);
        }
    }

    /// <summary>
    /// 玩家加速動畫
    /// </summary>
    /// <param name="playerID">加速的玩家代號 左邊是1 右邊是2</param>
    public void PlayPlayerBoosterAni(int playerID)
    {
        if (playerID == 1)
        {
            Player1.SetTrigger("Booster");
            p1AniEvents.PlayBoosterOutlineAni(p1isWhite);
        }
        else
        {
            Player2.SetTrigger("Booster");
            p2AniEvents.PlayBoosterOutlineAni(p2isWhite);
        }
    }


}
