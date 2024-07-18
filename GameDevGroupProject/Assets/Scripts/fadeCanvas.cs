using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeCanvas : MonoBehaviour
{
    private soundManager sndManager;

    void Start()
    {
        sndManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<soundManager>();
    }

    public void destroyCanvas()
    {
        Destroy(gameObject);
        sndManager.PlayBGM();
    }
}
