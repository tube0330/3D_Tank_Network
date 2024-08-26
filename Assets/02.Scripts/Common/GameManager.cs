using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
        set
        {
            if (instance == null)
                instance = value;
            else if (instance != value)
                Destroy(value.gameObject);
        }
    }
    [SerializeField] List<Transform> spawnList;
    [SerializeField] GameObject apachePrefab;
    public bool isGameOver = false;
    public Text connectText;
    public Text logMsgText;

    void Awake()
    {
        Instance = this;
        //DontDestroyOnLoad(gameObject);
        apachePrefab = Resources.Load<GameObject>("Apache");
        CreateTank();
        PhotonNetwork.IsMessageQueueRunning = true;
    }

    void Update()
    {
        GetConnectPlayerCount();
    }

    void Start()
    {
        var spawnPoint = GameObject.Find("SpawnPoints").gameObject;
        if (spawnPoint != null)
            spawnPoint.GetComponentsInChildren<Transform>(spawnList);

        spawnList.RemoveAt(0);

        string msg = "\n<color=#00ff00>[" + PhotonNetwork.NickName + "]Connected</color>";
        photonView.RPC(nameof(LogMessage), RpcTarget.AllBuffered, msg);

        if(spawnList.Count > 0 && PhotonNetwork.IsMasterClient) 
        InvokeRepeating("CreateApache", 0.01f, 3.0f);
    }

    #region Start Coroutine
   /*  IEnumerator CreateApache()
    {
       while (isGameOver == false)
       {


           int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;
           if (count < 10)
           {
               yield return new WaitForSeconds(3.0f);
               int idx = Random.Range(0, spawnList.Count);
               Instantiate(apachePrefab, spawnList[idx].position, spawnList[idx].rotation);
           }
           else
               yield return null;
       }

    } */
    #endregion

    void CreateTank()
    {
        float pos = Random.Range(-50f, 50f);
        PhotonNetwork.Instantiate("Tank", new Vector3(pos, 5f, pos), Quaternion.identity, 0);
    }

    void CreateApache()
    {
        if (isGameOver) return;

        int count = (int)GameObject.FindGameObjectsWithTag("APACHE").Length;

        if (count < 10)
        {
            int idx = Random.Range(0, spawnList.Count);
            PhotonNetwork.InstantiateRoomObject("Apache", spawnList[idx].position, spawnList[idx].rotation, 0, null);
        }
    }

    [PunRPC]
    public void ApplyPlayerCountUpdate()
    {
        Room curRoom = PhotonNetwork.CurrentRoom;
        connectText.text = curRoom.PlayerCount.ToString() + " / " + curRoom.MaxPlayers.ToString();
    }

    [PunRPC]
    void GetConnectPlayerCount()
    {
        if (PhotonNetwork.IsMasterClient)
            photonView.RPC(nameof(ApplyPlayerCountUpdate), RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GetConnectPlayerCount();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        GetConnectPlayerCount();
    }

    public void OnClickExitRoom()   //룸 나가기 버튼 클릭 이벤트에 연결될 함수
    {
        string msg = "\n<color=#ff0000>[" + PhotonNetwork.NickName + "]DisConnected</color>";
        photonView.RPC(nameof(LogMessage), RpcTarget.AllBuffered, msg);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()   //룸에서 접속이 종료되었을 때 호출되는 콜백 함수
    {
        SceneManager.LoadSceneAsync("LobbyScene");
    }

    [PunRPC]
    void LogMessage(string message)
    {
        logMsgText.text += message;
    }
}
