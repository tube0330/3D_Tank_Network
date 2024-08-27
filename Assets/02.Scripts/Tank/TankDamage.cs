using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class TankDamage : MonoBehaviourPunCallbacks
{
    [SerializeField] private MeshRenderer[] m_Renderer;
    [SerializeField] private GameObject expEffect;

    private int initHp = 100;
    [SerializeField] private int curHp = 0;

    string playerTag = "Player";
    string apacheTag = "APACHE";

    public Canvas hudCanvas;
    public Image HpBar;
    public int killScore = 0;
    public Text killTxt;

    void Start()
    {
        m_Renderer = GetComponentsInChildren<MeshRenderer>();
        expEffect = Resources.Load<GameObject>("Explosion");

        curHp = initHp;
        HpBar.color = Color.green;
    }

    [PunRPC]
    void OnDamageRPC(string tag)
    {
        if (curHp > 0 && tag == playerTag)
        {
            //Debug.Log(playerTag);
            curHp -= 25;
            HpBar.fillAmount = (float)curHp / (float)initHp;
            if (HpBar.fillAmount <= 0.4f)
                HpBar.color = Color.red;
            else if (HpBar.fillAmount <= 0.6f)
                HpBar.color = Color.yellow;

            if (curHp <= 0)
            {
                StartCoroutine(ExplosionTank());
            }
        }

        else if (curHp > 0 && tag == apacheTag)
        {
            Debug.Log(curHp);
            curHp -= 1;
            HpBar.fillAmount = (float)curHp / (float)initHp;
            if (HpBar.fillAmount <= 0.4f)
                HpBar.color = Color.red;
            else if (HpBar.fillAmount <= 0.6f)
                HpBar.color = Color.yellow;

            if (curHp <= 0)
            {
                StartCoroutine(ExplosionTank());
                Die(PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }

    public void OnDamage(string Tag)
    {
        if (photonView.IsMine)
            photonView.RPC("OnDamageRPC", RpcTarget.All, Tag);
    }

    IEnumerator ExplosionTank()
    {
        var effect = Instantiate(expEffect, transform.position, Quaternion.identity);
        Destroy(effect, 2.0f);
        SetTankvisible(false);
        hudCanvas.enabled = false;

        yield return new WaitForSeconds(5.0f);

        curHp = initHp;
        SetTankvisible(true);
        HpBar.fillAmount = 1.0f;
        HpBar.color = Color.green;
        hudCanvas.enabled = true;
    }

    void SetTankvisible(bool isvisible)
    {
        foreach (var tank in m_Renderer)
            tank.enabled = isvisible;
    }

    public void OnKilled(int killActorNumber)
    {
        //if (photonView.IsMine) PhotonNetwork.Destroy(gameObject);   //자신이 죽었을 경우

        if (killActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)   //자신이 죽인 경우 killscore++
        {
            killScore++;
            killTxt.text = "Tank kill" + killScore.ToString();
            Debug.Log(killScore);
        }
    }

    public void Die(int killActorNumber) => photonView.RPC(nameof(OnKilled), RpcTarget.All, killActorNumber);
}