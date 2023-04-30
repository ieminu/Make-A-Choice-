using System;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Public Variables

    public bool IsGameOver { get; private set; } = false;

    public bool HasThePlayerWon { get; private set; } = false;

    #endregion

    #region Private Variables

    [SerializeField]
    PawnManager pawnManager;

    #endregion

    [Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        CheckOthersName();

        void CheckOthersName()
        {
            switch (other.transform.name)
            {
                case "First Gates Background":
                    pawnManager.SetPlayerAndPawnCountAndStreamlineThePawnsAccordingToSelectionGatesText(other.transform.parent.transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text);
                    break;

                case "Second Gates Background":
                    pawnManager.SetPlayerAndPawnCountAndStreamlineThePawnsAccordingToSelectionGatesText(other.transform.parent.transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text);
                    break;

                case "Finish Line":
                    IsGameOver = true;
                    HasThePlayerWon = true;
                    break;

                case "Border":
                    IsGameOver = true;
                    HasThePlayerWon = false;
                    break;

                default: break;
            }
        }
    }
}