using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
// ray�� ��Ƽ� ���콺 ������ ���� 
public class TurretCtrl : MonoBehaviourPun,IPunObservable
{
    private Transform tr;
    private float rotSpeed = 5f;
    RaycastHit hit;
    private Quaternion curRot = Quaternion.identity;
 
    void Start()
    {
        tr = transform;
        curRot = tr.localRotation;
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
            stream.SendNext(tr.localRotation);
        else
            curRot =(Quaternion)stream.ReceiveNext();   

    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);

            if (Physics.Raycast(ray, out hit, 10000f, 1 << 8))
            {
                Vector3 relative = tr.InverseTransformPoint(hit.point);
                float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
                tr.Rotate(0f, angle * Time.deltaTime * rotSpeed, 0f);
            }
        }

        else
            tr.localRotation = Quaternion.Slerp(tr.localRotation,curRot, Time.deltaTime*3f);
    }
}
