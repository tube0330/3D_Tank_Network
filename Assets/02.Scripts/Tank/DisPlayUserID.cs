using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DisplayUserID : MonoBehaviourPun
{
    public Text UserID;
    void Start()
    {
        UserID.text = photonView.Owner.NickName;
         // ���̵� ��ũ �� ǥ�� 
    }
    
}
