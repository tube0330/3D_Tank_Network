using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;

public class LookAtVituralCam : MonoBehaviourPun
{
    CinemachineVirtualCamera virtualCamera;
    void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

    }
    private void Update()
    {

        if (photonView.IsMine) //포톤뷰가 나의 것이면 로컬
        {
           
            virtualCamera.LookAt = transform;
        }
    }

}
