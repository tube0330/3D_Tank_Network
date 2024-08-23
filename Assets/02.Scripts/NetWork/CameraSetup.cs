using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;


public class CameraSetup : MonoBehaviourPun
{
    
    void Start()
    {

        if (photonView.IsMine) //����䰡 ���� ���̸� ����
        {
            CinemachineVirtualCamera virtualCamera = FindObjectOfType<CinemachineVirtualCamera>(); 
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }
        
    }

    
}
