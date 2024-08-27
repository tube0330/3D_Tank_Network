using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ApacheDamage : MonoBehaviourPun
{
    [SerializeField] MeshRenderer[] meshren;
    [SerializeField] GameObject explotionEff;
    readonly string tankTag = "Player";

    void Start()
    {
        meshren = GetComponentsInChildren<MeshRenderer>();
        explotionEff = Resources.Load<GameObject>("Explosion");
    }

    [PunRPC]
    void OnDamageRPC(string tag)
    {
        if (tag == tankTag)
        {
            Debug.Log(tankTag);
            var eff = Instantiate(explotionEff, transform.position, Quaternion.identity);
            Destroy(eff, 1.0f);
            
            SetApacheVisible(false);
            GameManager.Instance.apacheKillCnt++;
        }
    }

    public void OnDamage(string tag)
    {
        if (photonView.IsMine)
            photonView.RPC("OnDamageRPC", RpcTarget.All, tag);
    }

    void SetApacheVisible(bool isvisible)
    {
        foreach(var mren in meshren)
        mren.enabled = isvisible;
    }
}
