using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    public string Version = "V1.0";
    public InputField userId;
    public InputField roomName;
    public GameObject roomItem;
    public GameObject scrollContents;
    readonly string roomItemTag = "ROOMITEM";

    void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = Version;
            PhotonNetwork.ConnectUsingSettings();
            roomName.text = "Room" + Random.Range(0, 999).ToString("000");
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby");
        //PhotonNetwork.JoinRandomRoom();
        userId.text = GetUserId();
    }

    string GetUserId()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if (string.IsNullOrEmpty(userId))
            userId = "USER_" + Random.Range(0, 999).ToString("000");
        return userId;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        print("No rooms");
        PhotonNetwork.CreateRoom("MyRoom", new RoomOptions { MaxPlayers = 20 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Enter Room");
        StartCoroutine(LoadTankMainScene());
    }

    IEnumerator LoadTankMainScene()
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        AsyncOperation ao = SceneManager.LoadSceneAsync("TankMainScene");
        yield return ao;

    }

    public void OnClickJoinRandomRoom()
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnClickCreateRoom()
    {
        string _roomName = roomName.text;

        if (string.IsNullOrEmpty(roomName.text))
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");

        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        //print(PhotonNetwork.CurrentRoom.PlayerCount);

        RoomOptions ro = new RoomOptions();
        ro.IsOpen = true;       //공개방
        ro.IsVisible = true;    //방 목록 리스트에 룸 나타내기
        ro.MaxPlayers = 20;

        PhotonNetwork.CreateRoom(_roomName, ro, TypedLobby.Default);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(roomItemTag))
        {
            Destroy(obj);
        }

        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList) continue;

            GameObject room = (GameObject)Instantiate(roomItem);
            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = roomInfo.Name;
            roomData.connectPlayer = roomInfo.PlayerCount;
            roomData.maxPlayer = roomInfo.MaxPlayers;
            roomData.DisplayRoomData(); //텍스트 정보를 표시하는 함수

            roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomItem(roomData.roomName); });
            if (roomData.connectPlayer == 0)
                Destroy(room);
        }
    }

    void OnClickRoomItem(string roomName)
    {
        PhotonNetwork.NickName = userId.text;
        PlayerPrefs.SetString("USER_ID", userId.text);
        PhotonNetwork.JoinRoom(roomName);
    }

    private void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.InRoom.ToString());
    }

        public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed");
    }

}
