using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    //Audio Sources
    [Header("Audio Sources")]
    [SerializeField] AudioSource BGMSource;
    [SerializeField] AudioSource SFXSource;

    //BGM samples
    [Header("BGM samples")]
    public AudioClip BGM;

    //SFX samples
    [Header("SFX samples")]
    public AudioClip characterJump;

    public AudioClip playerDetected;
    public AudioClip enemyDefeated;

    public AudioClip powerUpPickUpRed;
    public AudioClip powerUpPickUpGreen;
    public AudioClip powerUpPickUpPulse;

    public AudioClip powerUpFireRed;
    public AudioClip powerUpFirePulse;
    
    public AudioClip levelComplete;


    // Start is called before the first frame update
    void Start()
    {
        //Setting up and playing bacground music on game start
        PlayBGM(BGM);
    }

    public void PlayBGM(AudioClip sample)
    {
        BGMSource.clip = sample;
        BGMSource.Play();
    }

    public void PlaySFX(AudioClip sample)
    {
        SFXSource.clip = sample;
        //Should not cut previously playing audio clip
        SFXSource.PlayOneShot(sample);
    }   

}
