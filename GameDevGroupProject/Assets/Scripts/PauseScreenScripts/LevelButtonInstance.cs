using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButtonInstance : MonoBehaviour
{
    private void Start()
    {
        _image = this.GetComponent<Image>();
    }

    // set text of button!!!
    public void SetText(string text)
    {
        var buttonTextComponent = gameObject.GetComponentInChildren<Text>();

        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = text;
        }
    }

    private void Update()
    {
        if (!isEnabled)
        {
            _image.color = new Color(255, 255, 255, 0.25f);
        }
    }

    void GoToScene()
    {
        if (isEnabled)
            SceneManager.LoadScene(targetScene);
    }

    private string targetScene;

    // the scene this button goes when clicked
    public void GoesToScene(string sceneName)
    {
        targetScene = "Scenes/" + sceneName;
        this.GetComponent<Button>().onClick.AddListener(GoToScene);
    }

    public bool isEnabled = true;
    private Image _image;
}