using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class antennaDish : MonoBehaviour
{
    public int antennaHP = 4;
    private SpriteRenderer sRender;
    [SerializeField] private Sprite crack1;
    [SerializeField] private Sprite crack2;
    [SerializeField] private Sprite crack3;

    private ParticleSystem blueAntennaParticles;
    private ParticleSystem yellowAntennaParticles;

    private soundManager sndManager;

    // Start is called before the first frame update
    void Start()
    {
        sRender = GetComponent<SpriteRenderer>();
        blueAntennaParticles = GameObject.Find("antenna-particles-blue").GetComponent<ParticleSystem>();
        yellowAntennaParticles = GameObject.Find("antenna-particles-yellow").GetComponent<ParticleSystem>();
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Pulse")
        {
            antennaHP--;
            if (antennaHP == 3)
            {
                sRender.sprite = crack1;
                sndManager.PlaySFX(sndManager.crack);
            } else if (antennaHP == 2)
            {
                sRender.sprite = crack2;
                sndManager.PlaySFX(sndManager.crack);
            } else if (antennaHP == 1)
            {
                sRender.sprite = crack3;
                sndManager.PlaySFX(sndManager.crack);
            } else if (antennaHP == 0)
            {
                AntennaDestroyed();
                sndManager.PlaySFX(sndManager.explosion);
            }
            Debug.Log("Antenna hp = " + antennaHP);
        }

    }

    private void AntennaDestroyed()
    {
        Destroy(gameObject);
        blueAntennaParticles.Play();
        yellowAntennaParticles.Play();
    }
}
