using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class TankMove : MonoBehaviourPun, IPunObservable
{
    [SerializeField] Rigidbody rb;
    Transform tr;
    Vector3 curPos = Vector3.zero;
    Quaternion curRot = Quaternion.identity;
    public float rotSpeed = 90f;
    //float h = 0f, v = 0f;
    float moveSpeed = 20f;

    [Header("PlayerInput")]
    [SerializeField] PlayerInput playerInput = null;
    [SerializeField] InputActionMap playerMap;
    [SerializeField] InputAction moveAction;
    //[SerializeField] InputAction rotateAction;
    Vector3 moveDir = Vector3.zero;

    void Awake()
    {
        photonView.Synchronization = ViewSynchronization.Unreliable;
        photonView.ObservedComponents[0] = this;
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        curPos = tr.position;
        curRot = tr.rotation;
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMap = playerInput.actions.FindActionMap("Player");
        moveAction = playerInput.actions["Move"];
        //rotateAction = playerMap.FindAction("Rotate");
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            /* h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            tr.Translate(Vector3.forward * v * Time.deltaTime * moveSpeed);
            tr.Rotate(Vector3.up * h * Time.deltaTime * rotSpeed); */

            Vector2 dir = moveAction.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0f, dir.y);

            if (moveDir != Vector3.zero)
            {
                tr.rotation = Quaternion.Slerp(tr.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime);  //진행 방향으로 회전
                tr.Translate(Vector3.forward * Time.deltaTime * moveSpeed);   //이동
            }
        }

        else
        {
            tr.position = Vector3.Lerp(tr.position, curPos, Time.deltaTime * 3f);
            tr.rotation = Quaternion.Slerp(tr.rotation, curRot, Time.deltaTime * 3f);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
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
