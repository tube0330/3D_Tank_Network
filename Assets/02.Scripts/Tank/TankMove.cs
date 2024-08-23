using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TankMove : MonoBehaviourPun,IPunObservable
{
    [SerializeField]private Rigidbody rb;
    private float h = 0f, v = 0f;
    private Transform tr;
    float moveSpeed = 20f;
    public float rotSpeed = 90f;
    private Vector3 curPos = Vector3.zero;
    private Quaternion curRot = Quaternion.identity;

    void Awake()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3 (0,-0.5f,0);
        curPos = tr.position; curRot = tr.rotation;
    }
    
    void Update()
    {
        if (photonView.IsMine)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Translate(Vector3.forward * v * Time.deltaTime * moveSpeed);
            tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed);
        }
        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(tr.position);
            stream.SendNext(tr.rotation);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
