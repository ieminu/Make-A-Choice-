using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{
    #region Private Variables

    Button playButton;
    Button exitButton;

    #endregion

    [Obsolete]
    private void Start()
    {
        GameObject canvas;

        SetTheVariables();

        AddTheListeners();

        void SetTheVariables()
        {
            canvas = GameObject.Find("Canvas");

            playButton = canvas.transform.FindChild("Play Button").GetComponent<Button>();

            exitButton = canvas.transform.FindChild("Exit Button").GetComponent<Button>();
        }

        void AddTheListeners()
        {
            playButton.onClick.AddListener(LoadTheGameScene);

            exitButton.onClick.AddListener(ExitTheGame);

            void LoadTheGameScene()
            {
                SceneManager.LoadScene("GameScene");
            }

            void ExitTheGame()
            {
                Application.Quit();
            }
        }
    }
}