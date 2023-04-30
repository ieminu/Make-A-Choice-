using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Public Variables

    public bool IsGameOver { get; private set; } = false;

    public bool HasThePlayerWon { get; private set; } = false;

    #endregion

    #region Private Variables

    [SerializeField]
    PawnManager pawnManager;

    [SerializeField]
    Player player;

    [SerializeField]
    GameUIManager gameUIManager;

    #endregion

    private void Update()
    {
        SetIsGameOverBool();
        CheckIfIsTheGameOver();
        CheckIfThePlayerHasWon();

        void SetIsGameOverBool()
        {
            if (pawnManager.IsGameOver || player.IsGameOver)
                IsGameOver = true;
        }

        void CheckIfIsTheGameOver()
        {
            if (IsGameOver)
            {
                Time.timeScale = 0;
            }
        }

        void CheckIfThePlayerHasWon()
        {
            if (pawnManager.DidThePlayerWin || player.HasThePlayerWon)
            {
                HasThePlayerWon = true;
            }

            else
                HasThePlayerWon = false;
        }
    }
}