using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public GameObject boosterOutlineObj;
    public void PlayBoosterOutlineAni(bool isWhite)
    {
        boosterOutlineObj.transform.position = this.transform.position;
        boosterOutlineObj.SetActive(true);
        if (isWhite)
        {
            boosterOutlineObj.GetComponent<Animator>().SetBool("isWhite", true);
        }
        else
        {
            boosterOutlineObj.GetComponent<Animator>().SetBool("isBlack", true);
        }
    }



}
