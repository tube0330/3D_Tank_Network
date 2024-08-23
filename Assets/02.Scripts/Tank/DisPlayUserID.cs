using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisPlayUserID : MonoBehaviourPun
{
    public Text UserID;
    void Start()
    {
        UserID.text = photonView.Owner.NickName;
         // 아이디를 탱크 에 표시 
    }
    
}
