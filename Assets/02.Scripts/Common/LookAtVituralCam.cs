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

        if (photonView.IsMine) //����䰡 ���� ���̸� ����
        {
           
            virtualCamera.LookAt = transform;
        }
    }

}
