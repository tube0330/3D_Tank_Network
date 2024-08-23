using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public bool isMute = false;
    public static SoundManager s_instance;
    private void Awake()
    {
        if(s_instance == null)
            s_instance = this;
            
        else if(s_instance !=this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void BackGroundSound(Vector3 pos ,AudioClip bgm , bool isLoop)
    {
        if (isMute) return;

        GameObject soundObj = new GameObject("backGroundSound");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = bgm;
        audioSource.loop = isLoop;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 30f;
        audioSource.volume = 1.0f;
        audioSource.Play();


    }
    public void OtherPlaySound(Vector3 pos,AudioClip sfx)
    {
        if (isMute) return;

        GameObject soundObj = new GameObject("SFX");
        soundObj.transform.position = pos;
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        audioSource.clip = sfx;
      
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 30f;
        audioSource.volume = 1.0f;
        audioSource.Play();


    }

}
