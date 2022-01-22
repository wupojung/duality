using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterAnimationEvents : MonoBehaviour
{
    public List<GameObject> boosterOutlineObjList;
    void ResetBoosterAniObj(int playerID)
    {
        this.GetComponent<Animator>().SetBool("isBlack", false);
        this.GetComponent<Animator>().SetBool("isWhite", false);
        boosterOutlineObjList[playerID - 1].SetActive(false);

    }

}
