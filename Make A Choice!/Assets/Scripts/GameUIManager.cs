using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameUIManager : MonoBehaviour
{
    #region Public Variables

    public int Level { get; private set; } = 1;

    #endregion

    #region Priavte Variables

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    PawnManager pawnManager;

    [SerializeField]
    TextMeshProUGUI levelTMPUGUI;

    [Header("Winning Variables")]

    [SerializeField]
    GameObject winningImage;

    [SerializeField]
    Button winningImagesMenuButton;

    [SerializeField]
    Button nextLevelButton;

    [SerializeField]
    TextMeshProUGUI scoreTMPUGUI;
    [SerializeField]
    TextMeshProUGUI highScoreTMPUGUI;

    [Header("Losing Variables")]

    [SerializeField]
    GameObject losingImage;

    [SerializeField]
    Button losingImagesMenuButton;

    [SerializeField]
    Button restartLevelButton;

    #endregion

    private void Start()
    {
        AddListeners();
        SetVariables();

        void AddListeners()
        {
            winningImagesMenuButton.onClick.AddListener(LoadMenuScene);

            nextLevelButton.onClick.AddListener(ReloadThisSceneAsANewLevel);

            losingImagesMenuButton.onClick.AddListener(LoadMenuScene);

            restartLevelButton.onClick.AddListener(RestartThisLevel);

            void LoadMenuScene()
            {
                SceneManager.LoadScene("MenuScene");
            }

            void ReloadThisSceneAsANewLevel()
            {
                PlayerPrefs.SetInt("Level", PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") + 1 : 2);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            void RestartThisLevel()
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        void SetVariables()
        {
            Time.timeScale = 1;

            scoreTMPUGUI.text = "0";
            highScoreTMPUGUI.text = PlayerPrefs.HasKey("High Score") ? PlayerPrefs.GetInt("High Score").ToString() : "0";

            levelTMPUGUI.text = (PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 1).ToString();
        }
    }

    void Update()
    {
        CheckIfGameIsOver();

        void CheckIfGameIsOver()
        {
            if (gameManager.IsGameOver)
            {
                CheckIfThePlayerHasWon();

                void CheckIfThePlayerHasWon()
                {
                    if (gameManager.HasThePlayerWon)
                    {
                        winningImage.SetActive(true);

                        scoreTMPUGUI.text = pawnManager.PlayerAndPawnCountText.text;

                        string winningImagesScoreTMPUGUIsText = scoreTMPUGUI.text;

                        CheckIfHighScoreShouldBeUpdated();

                        void CheckIfHighScoreShouldBeUpdated()
                        {
                            if (Convert.ToInt32(winningImagesScoreTMPUGUIsText) > (PlayerPrefs.HasKey("High Score") ? PlayerPrefs.GetInt("High Score") : 0))
                            {
                                highScoreTMPUGUI.text = winningImagesScoreTMPUGUIsText;
                                PlayerPrefs.SetInt("High Score", Convert.ToInt32(highScoreTMPUGUI.text));
                            }
                        }
                    }

                    else
                        losingImage.SetActive(true);
                }
            }
        }
    }
}