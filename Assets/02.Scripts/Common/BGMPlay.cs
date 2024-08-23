using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlay : MonoBehaviour
{
    private Transform tr;
    public AudioClip bgm;
    void Start()
    {
        tr = transform;
        SoundManager.s_instance.BackGroundSound(tr.position, bgm, true);
        
    }

   
}
