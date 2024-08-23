using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviourPun
{
    [HideInInspector]
    public string roomName = string.Empty;
    [HideInInspector]
    public int connectPlayer = 0;
    [HideInInspector]
    public int maxPlayer = 0;
    public Text RName;
    public Text ConnectInfo;

    public void DisplayRoomData()
    {
        RName.text = roomName;
        ConnectInfo.text = "(" + connectPlayer.ToString() + "/" + maxPlayer.ToString() + ")";
    }
}
