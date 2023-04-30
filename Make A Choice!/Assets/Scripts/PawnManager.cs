using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PawnManager : MonoBehaviour
{
    #region Public Variables

    public bool IsGameOver { get; private set; } = false;

    public TextMeshProUGUI PlayerAndPawnCountText { get; private set; }

    public List<GameObject> Pawns { get; private set; } = new();

    public bool DidThePlayerWin { get; private set; } = false;

    #endregion

    #region Private Variables

    [SerializeField]
    GameObject pawnPrefab;

    [SerializeField]
    GameObject pawnsGameObject;

    [SerializeField]
    GameManager gameManager;

    [SerializeField]
    Transform playersTransform;

    [Header("lineup")]

    float xAxis = 0;
    float zAxis = 0;

    [SerializeField]
    float distance;

    float xAxissAdditional;
    float zAxissAdditional;

    int smallVariableCounter = 2;
    int bigVariableCounter = 8;

    int smallVariableCountersAdditional = 2;

    List<Vector3> localPositions = new();

    Vector3 pawnPrefabsFirstLocalScaleValue;

    Vector3 firstPawnsLocalScale;

    #endregion

    [Obsolete]
    private void Start()
    {
        SetTheVariables();

        void SetTheVariables()
        {
            distance = (GameObject.Find("Plane").transform.localScale.x * 10 / 2 - playersTransform.localScale.x / 2) / 2;

            xAxis = 0;
            zAxis = 2 * distance;

            xAxissAdditional = distance;
            zAxissAdditional = -distance;

            pawnPrefabsFirstLocalScaleValue = pawnPrefab.transform.localScale;

            PlayerAndPawnCountText = playersTransform.FindChild("Canvas").transform.FindChild("Player + Pawn Count Text").GetComponent<TextMeshProUGUI>();
        }
    }

    public void SetPlayerAndPawnCountAndStreamlineThePawnsAccordingToSelectionGatesText(string selectionGatesText)
    {
        if (!gameManager.IsGameOver)
            CheckThatWhatSelectionGatesTextContains();

        void CheckThatWhatSelectionGatesTextContains()
        {
            if (selectionGatesText.Contains("+"))
            {
                int numberOfRows = NumberOfRows();

                CallPawnSpawningProcesssMethodAccordingToTheText();

                UpdatePlayerAndPawnCountText();

                CheckIfNumberOfRowsMethodsValueIsIncreased();

                void CallPawnSpawningProcesssMethodAccordingToTheText()
                {
                    for (int i = 1; i <= Convert.ToInt32(selectionGatesText.Split('+')[1]); i++)
                    {
                        PawnSpawningProcesss();
                    }
                }

                void CheckIfNumberOfRowsMethodsValueIsIncreased()
                {
                    if (NumberOfRows() > numberOfRows)
                    {
                        RearrangeThePawns();
                    }
                }
            }
            
            else if (selectionGatesText.Contains("-"))
            {
                int selectionGatesTextsValue = Convert.ToInt32(selectionGatesText.Split('-')[1]);

                int numberOfRows = NumberOfRows();

                CallPawnDespawningProcesssMethodAccordingToTheText();

                ResetTheDistance();

                CheckIfNumberOfPawnsThatWillDespawnIsEqualOrBiggerThanOne();

                CheckIfNumberOfRowsMethodsValueIsDecreased();

                void CallPawnDespawningProcesssMethodAccordingToTheText()
                {
                    for (int i = 0; i < selectionGatesTextsValue; i++)
                    {
                        PawnDespawningProcesss();
                    }
                }

                void CheckIfNumberOfPawnsThatWillDespawnIsEqualOrBiggerThanOne()
                {
                    if (selectionGatesTextsValue >= 1)
                    {
                        SetSmallAndBigVariableCounters();

                        SetSmallVariableCountersAdditionalValue();

                        ResetTheXAxisAndZAxisVariables();

                        UpdatePlayerAndPawnCountText();
                    }
                }

                void CheckIfNumberOfRowsMethodsValueIsDecreased()
                {
                    if (NumberOfRows() < numberOfRows)
                    {
                        RearrangeThePawns();
                    }
                }
            }

            else if (selectionGatesText.Contains("x"))
            {
                int numberOfRows = NumberOfRows();

                CallPawnSpawningProcesssMethodAccordingToTheText();

                UpdatePlayerAndPawnCountText();

                CheckIfNumberOfRowsMethodsValueIsIncreased();

                void CallPawnSpawningProcesssMethodAccordingToTheText()
                {
                    for (int i = 0; i < Convert.ToInt32(PlayerAndPawnCountText.text) * (Convert.ToInt32(selectionGatesText.Split('x')[1]) - 1); i++)
                    {
                        PawnSpawningProcesss();
                    }
                }

                void CheckIfNumberOfRowsMethodsValueIsIncreased()
                {
                    if (NumberOfRows() > numberOfRows)
                    {
                        RearrangeThePawns();
                    }
                }
            }

            else if (selectionGatesText.Contains("/"))
            {
                int numberOfPawnsThatWillDespawn = Convert.ToInt32(PlayerAndPawnCountText.text) - Convert.ToInt32(Math.Round(Convert.ToDecimal(Convert.ToDouble(PlayerAndPawnCountText.text) / Convert.ToDouble(selectionGatesText.Split('/')[1]))));

                int numberOfRows = NumberOfRows();

                CallPawnDespawningProcesssMethodAccordingToTheText();

                ResetTheDistance();

                CheckIfNumberOfPawnsThatWillDespawnIsEqualOrBiggerThanOne();

                CheckIfNumberOfRowsMethodsValueIsDecreased();

                void CallPawnDespawningProcesssMethodAccordingToTheText()
                {
                    for (int i = 0; i < numberOfPawnsThatWillDespawn; i++)
                    {
                        PawnDespawningProcesss();
                    }
                }

                void CheckIfNumberOfPawnsThatWillDespawnIsEqualOrBiggerThanOne()
                {
                    if (numberOfPawnsThatWillDespawn >= 1)
                    {
                        SetSmallAndBigVariableCounters();

                        SetSmallVariableCountersAdditionalValue();

                        ResetTheXAxisAndZAxisVariables();

                        UpdatePlayerAndPawnCountText();
                    }
                }

                void CheckIfNumberOfRowsMethodsValueIsDecreased()
                {
                    if (NumberOfRows() < numberOfRows)
                    {
                        RearrangeThePawns();
                    }
                }
            }
        }

        void UpdatePlayerAndPawnCountText()
        {
            PlayerAndPawnCountText.text = (Pawns.Count + 1).ToString();
        }

        void PawnSpawningProcesss()
        {
            GameObject pawn = Instantiate(pawnPrefab, pawnsGameObject.transform);

            CheckIfPawnsCountValueIsBiggerThanZero();

            Vector3 pawnsPosition = new(xAxis, pawn.transform.localPosition.y, zAxis);

            pawn.transform.localPosition = pawnsPosition;

            Pawns.Add(pawn);

            localPositions.Add(pawnsPosition);

            xAxis += xAxissAdditional;
            zAxis += zAxissAdditional;

            CheckIfPawnsCountIsEqualToSmallVariableCounter();

            CheckIfPawnsCountIsEqualToBigVariableCounter();

            void CheckIfPawnsCountValueIsBiggerThanZero()
            {
                if (Pawns.Count > 0)
                    pawn.transform.localScale = firstPawnsLocalScale;

                else
                    firstPawnsLocalScale = pawn.transform.localScale;
            }

            void CheckIfPawnsCountIsEqualToSmallVariableCounter()
            {
                if (Pawns.Count == smallVariableCounter)
                {
                    if (xAxissAdditional > 0 && zAxissAdditional < 0)
                        xAxissAdditional *= -1;

                    else if (xAxissAdditional < 0 && zAxissAdditional < 0)
                        zAxissAdditional *= -1;

                    else if (xAxissAdditional < 0 && zAxissAdditional > 0)
                        xAxissAdditional *= -1;

                    else if (xAxissAdditional > 0 && xAxissAdditional > 0)
                        zAxissAdditional *= -1;

                    smallVariableCounter += smallVariableCountersAdditional;
                }
            }

            void CheckIfPawnsCountIsEqualToBigVariableCounter()
            {
                if (Pawns.Count == bigVariableCounter)
                {
                    zAxis += 2 * distance;

                    smallVariableCounter += 2;
                    smallVariableCountersAdditional += 2;

                    bigVariableCounter += (NumberOfRows() + 3) * 4;
                }
            }
        }

        void PawnDespawningProcesss()
        {
            CheckIfPawnsCountIsBiggerThanZero();

            void CheckIfPawnsCountIsBiggerThanZero()
            {
                if (Pawns.Count > 0)
                {
                    Destroy(Pawns[Pawns.Count - 1]);

                    pawnsGameObject.transform.GetChild(Pawns.Count - 1).transform.parent = null;

                    Pawns.RemoveAt(Pawns.Count - 1);
                    localPositions.RemoveAt(localPositions.Count - 1);
                }

                else
                {
                    IsGameOver = true;
                    DidThePlayerWin = false;
                }
            }
        }

        void SetSmallAndBigVariableCounters()
        {
            SetTheSmallVariableCounter();
            SetTheBigVariableCounter();

            void SetTheSmallVariableCounter()
            {
                int numberOfRows = NumberOfRows();

                int maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows = MaxNumberOfPawnsOfThePreviousRowAccordingTo(numberOfRows);

                int pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow = Pawns.Count - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows;

                if (pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow >= 0 && pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow < (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4)
                {
                    smallVariableCounter = maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows + AdditionalValueAccordingTo(numberOfRows) + 1;
                }

                else if (pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow >= (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 && pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow < (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 2)
                {
                    smallVariableCounter = maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows + AdditionalValueAccordingTo(numberOfRows) + (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 + 1;
                }

                else if (pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow >= (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 2 && pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow < (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 * 3)
                {
                    smallVariableCounter = maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows + AdditionalValueAccordingTo(numberOfRows) + (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 * 2 + 1;
                }

                else if (pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow >= (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 * 3 && pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow < MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows)
                {
                    smallVariableCounter = maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows + AdditionalValueAccordingTo(numberOfRows) + (MaxNumberOfPawnsOfThisRow() - maxNumberOfPawnsOfThePreviousRowAccordingToNumberOfRows) / 4 * 3 + 1;
                }

                else if (pawnsCountMinusMaxNumberOfPawnsOfThePreviousRow == MaxNumberOfPawnsOfThisRow())
                {
                    smallVariableCounter = MaxNumberOfPawnsOfThisRow() + AdditionalValueAccordingTo(numberOfRows + 1) + 1;
                }
            }

            void SetTheBigVariableCounter()
            {
                bigVariableCounter = MaxNumberOfPawnsOfThisRow();
            }

            int MaxNumberOfPawnsOfThisRow()
            {
                int maxNumberOfPawnsOfThisRow = 0;

                int numberOfRows = NumberOfRows();

                int counter = numberOfRows == 0 ? 1 : numberOfRows;

                SetMaxNumberOfPawnsOfThisRowAccordingToCounter();

                return maxNumberOfPawnsOfThisRow;

                void SetMaxNumberOfPawnsOfThisRowAccordingToCounter()
                { 
                    for (int i = 2; i < counter + 2; i += 2)
                    {
                        maxNumberOfPawnsOfThisRow += 4 * i;

                        counter++;
                    }
                }
            }
        }

        void SetSmallVariableCountersAdditionalValue()
        {
            smallVariableCountersAdditional = AdditionalValueAccordingTo(NumberOfRows()) + 1;
        }

        void ResetTheXAxisAndZAxisVariables()
        {
            int minimumVariableCounter = 0;
            int maximumVariableCounter = 8;
            int numberOfRows = 0;

            Check:

            if (Pawns.Count >= minimumVariableCounter && Pawns.Count < maximumVariableCounter)
            {
                int pawnsCountMinusMinimumVariableCounter = Pawns.Count - minimumVariableCounter;

                int maximumVariableCounterMinusMinimumVariableCounter = maximumVariableCounter - minimumVariableCounter;

                if (pawnsCountMinusMinimumVariableCounter >= 0 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4 + 1)
                {
                    xAxissAdditional = distance;
                    zAxissAdditional = -distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 + 1 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 2 + 1)
                {
                    xAxissAdditional = -distance;
                    zAxissAdditional = -distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 2 + 1 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 3 + 1)
                {
                    xAxissAdditional = -distance;
                    zAxissAdditional = distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 3 + 1 && pawnsCountMinusMinimumVariableCounter < maximumVariableCounter - minimumVariableCounter)
                {
                    xAxissAdditional = distance;
                    zAxissAdditional = distance;
                }

                if (Pawns.Count == minimumVariableCounter)
                {
                    float thisDistance = Pawns.Count > 0 ? Pawns[0].transform.localPosition.z : distance * 2;

                    xAxis = 0;
                    zAxis = (numberOfRows == 0 ? 1 : numberOfRows) * thisDistance;
                }

                else
                {
                    RearrangeThePawns();

                    Vector3 lastPawnsLocalPosition = localPositions[localPositions.Count - 1];

                    xAxis = lastPawnsLocalPosition.x + xAxissAdditional;
                    zAxis = lastPawnsLocalPosition.z + zAxissAdditional;
                }

                if (pawnsCountMinusMinimumVariableCounter >= 0 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4)
                {
                    xAxissAdditional = distance;
                    zAxissAdditional = -distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 2)
                {
                    xAxissAdditional = -distance;
                    zAxissAdditional = -distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 2 && pawnsCountMinusMinimumVariableCounter < (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 3)
                {
                    xAxissAdditional = -distance;
                    zAxissAdditional = distance;
                }

                else if (pawnsCountMinusMinimumVariableCounter >= (maximumVariableCounterMinusMinimumVariableCounter) / 4 * 3 && pawnsCountMinusMinimumVariableCounter < maximumVariableCounter - minimumVariableCounter)
                {
                    xAxissAdditional = distance;
                    zAxissAdditional = distance;
                }
            }

            else
            {
                numberOfRows++;

                minimumVariableCounter = maximumVariableCounter;
                maximumVariableCounter += MaxNumberOfPawnsInTheNextRowAccordingToThe(NumberOfRows());

                goto Check;
            }
        }

        void RearrangeThePawns()
        {
            int numberOfRows = NumberOfRows();

            float firstPawnsFirstLocalScalesYAxis;

            ResetTheDistance();

            ReplaceThePawns();

            GetTheScale();

            RescaleThePawns();

            ResetPawnsHeight();

            void ReplaceThePawns()
            {
                for (int i = 0; i < Pawns.Count; i++)
                {
                    Vector3 positionssIthElement = localPositions[i];

                    bool isSetted = false;

                    float indexOfThisRowAccordingToIMultipliedByDistance = IndexOfThisRowAccordingTo(i) * distance;

                    float indexOfThisRowAccordingToIMultipliedByNegativeDistance = IndexOfThisRowAccordingTo(i) * -distance;

                    float previousPawnsLocalPositionsXAxisPlusDistance = i > 0 ? localPositions[i - 1].x + distance : 0;

                    float previousPawnsLocalPositionsXAxisMinusDistance = i > 0 ? localPositions[i - 1].x - distance : 0;

                    float previousPawnsLocalPositionsZAxisPlusDistance = i > 0 ? localPositions[i - 1].z + distance : 0;

                    float previousPawnsLocalPositionsZAxisMinusDistance = i > 0 ? localPositions[i - 1].z - distance : 0;

                    int additionalValueAccordingToIndexOfThisRowAccordingToI = AdditionalValueAccordingTo(IndexOfThisRowAccordingTo(i));

                    int numberOfPawnsThatReplasedInThisRow = NumberOfPawnsThatReplasedInThisRow();

                    if (!isSetted)
                        if (numberOfPawnsThatReplasedInThisRow == 0)
                        {
                            localPositions[i] = new Vector3(positionssIthElement.x, positionssIthElement.y, indexOfThisRowAccordingToIMultipliedByDistance * 2);

                            isSetted = true;
                        }

                    if (!isSetted)
                        for (int j = 1; j < additionalValueAccordingToIndexOfThisRowAccordingToI + 1; j++)
                        {
                            if (numberOfPawnsThatReplasedInThisRow == j)
                                {
                                    localPositions[i] = new Vector3(previousPawnsLocalPositionsXAxisPlusDistance, positionssIthElement.y, previousPawnsLocalPositionsZAxisMinusDistance);

                                    isSetted = true;
                                }
                        }

                    if (!isSetted)
                        if (numberOfPawnsThatReplasedInThisRow == MaximumPawnCountToReplaceInThisRow() / 4 * 1)
                        {
                            localPositions[i] = new Vector3(indexOfThisRowAccordingToIMultipliedByDistance * 2, positionssIthElement.y, positionssIthElement.z);

                            isSetted = true;
                        }

                    if (!isSetted)
                        for (int j = additionalValueAccordingToIndexOfThisRowAccordingToI + 2; j < 2 * additionalValueAccordingToIndexOfThisRowAccordingToI + 2; j++)
                        {
                            if (numberOfPawnsThatReplasedInThisRow == j)
                                {
                                    localPositions[i] = new Vector3(previousPawnsLocalPositionsXAxisMinusDistance, positionssIthElement.y, previousPawnsLocalPositionsZAxisMinusDistance);

                                    isSetted = true;
                                }
                        }

                    if (!isSetted)
                        if (numberOfPawnsThatReplasedInThisRow == MaximumPawnCountToReplaceInThisRow() / 4 * 2)
                        {
                            localPositions[i] = new Vector3(positionssIthElement.x, positionssIthElement.y, indexOfThisRowAccordingToIMultipliedByNegativeDistance * 2);

                            isSetted = true;
                        }

                    if (!isSetted)
                        for (int j = 2 * additionalValueAccordingToIndexOfThisRowAccordingToI + 3; j < 3 * additionalValueAccordingToIndexOfThisRowAccordingToI + 3; j++)
                        {
                            if (numberOfPawnsThatReplasedInThisRow == j)
                                {
                                    localPositions[i] = new Vector3(previousPawnsLocalPositionsXAxisMinusDistance, positionssIthElement.y, previousPawnsLocalPositionsZAxisPlusDistance);

                                    isSetted = true;
                                }
                        }

                    if (!isSetted)
                        if (numberOfPawnsThatReplasedInThisRow == MaximumPawnCountToReplaceInThisRow() / 4 * 3)
                        {
                            localPositions[i] = new Vector3(indexOfThisRowAccordingToIMultipliedByNegativeDistance * 2, positionssIthElement.y, positionssIthElement.z);

                            isSetted = true;
                        }

                    if (!isSetted)
                        for (int j = 3 * additionalValueAccordingToIndexOfThisRowAccordingToI + 4; j < 4 * additionalValueAccordingToIndexOfThisRowAccordingToI + 4; j++)
                        {
                            if (numberOfPawnsThatReplasedInThisRow == j)
                                {
                                    localPositions[i] = new Vector3(previousPawnsLocalPositionsXAxisPlusDistance, positionssIthElement.y, previousPawnsLocalPositionsZAxisPlusDistance);

                                    isSetted = true;
                                }
                        }

                    int NumberOfPawnsThatReplasedInThisRow()
                    {
                        int minimumVariableCounter = 0;
                        int maximumVariableCounter = 8;
                        int numberOfRowsOfPawnsThatReplased = 0;

                        Check:

                        if (i >= minimumVariableCounter && i < maximumVariableCounter)
                        {
                            return i - minimumVariableCounter;
                        }

                        else
                        {
                            numberOfRowsOfPawnsThatReplased++;

                            minimumVariableCounter = maximumVariableCounter;
                            maximumVariableCounter += MaxNumberOfPawnsInTheNextRowAccordingToThe(numberOfRowsOfPawnsThatReplased);

                            goto Check;
                        }
                    }

                    int MaximumPawnCountToReplaceInThisRow()
                    {
                        int minimumVariableCounter = 0;
                        int maximumVariableCounter = 8;
                        int currentNumberOfRowsOfReplasedPawns = 0;

                        Check:

                        if (i >= minimumVariableCounter && i < maximumVariableCounter)
                        {
                            return maximumVariableCounter - minimumVariableCounter;
                        }

                        else
                        {
                            currentNumberOfRowsOfReplasedPawns++;

                            minimumVariableCounter = maximumVariableCounter;
                            maximumVariableCounter += MaxNumberOfPawnsInTheNextRowAccordingToThe(currentNumberOfRowsOfReplasedPawns);

                            goto Check;
                        }
                    }

                    int IndexOfThisRowAccordingTo(int value)
                    {
                        int indexOfThisRow = 0;
                        int minimumVariableCounter = 0;
                        int maximumVariableCounter = 8;

                        Check:

                        if (value >= minimumVariableCounter && value < maximumVariableCounter)
                        {
                            indexOfThisRow++;

                            return indexOfThisRow;
                        }

                        else
                        {
                            indexOfThisRow++;

                            minimumVariableCounter = maximumVariableCounter;
                            maximumVariableCounter += MaxNumberOfPawnsInTheNextRowAccordingToThe(indexOfThisRow);

                            goto Check;
                        }
                    }
                }
            }

            void GetTheScale()
            {
                firstPawnsFirstLocalScalesYAxis = Pawns.Count > 0 ? Pawns[0].transform.localScale.y : 0;
            }

            void RescaleThePawns()
            {
                ResetEachPawnLocalScale();

                CheckIfPawnsCountIsBiggerThan0();

                void ResetEachPawnLocalScale()
                {
                    foreach (GameObject pawn in Pawns)
                    {
                        pawn.transform.localScale = new Vector3(pawnPrefabsFirstLocalScaleValue.x / numberOfRows, pawnPrefabsFirstLocalScaleValue.y / numberOfRows, pawnPrefabsFirstLocalScaleValue.z / numberOfRows);
                    }
                }

                void CheckIfPawnsCountIsBiggerThan0()
                {
                    if (Pawns.Count > 0)
                        firstPawnsLocalScale = Pawns[0].transform.localScale;
                }
            }

            void ResetPawnsHeight()
            {
                for (int i = 0; i < Pawns.Count; i++)
                {
                    localPositions[i] -= new Vector3(0, (firstPawnsFirstLocalScalesYAxis - Pawns[i].transform.localScale.y) / 2, 0);
                }
            }
        }

        void ResetTheDistance()
        {
            distance = (GameObject.Find("Plane").transform.localScale.x * 5 - playersTransform.localScale.x / 2) / (NumberOfRows() + 1.5f);
        }

        int AdditionalValueAccordingTo(int value)
        {
            int additionalValueAccordingToTheNumberOfRows = value == 0 ? 1 : value;

            IncreaseAdditionalValueAccordingToTheNumberOfRowsAccordingToNumberOfRows();

            return additionalValueAccordingToTheNumberOfRows;

            void IncreaseAdditionalValueAccordingToTheNumberOfRowsAccordingToNumberOfRows()
            {
                for (int i = 1; i < value; i++)
                {
                    additionalValueAccordingToTheNumberOfRows++;
                }
            }
        }

        int MaxNumberOfPawnsOfThePreviousRowAccordingTo(int value)
        {
            int maxNumberOfPawnsOfThePreviousRow = 0;

            int counter = value;

            SetMaxNumberOfPawnsOfThePreviousRowAccordingToCounter();

            return maxNumberOfPawnsOfThePreviousRow;

            void SetMaxNumberOfPawnsOfThePreviousRowAccordingToCounter()
            {
                for (int i = 2; i < counter + 1; i += 2)
                {
                    maxNumberOfPawnsOfThePreviousRow += 4 * i;

                    counter++;
                }
            }
        }
    }

    int NumberOfRows()
    {
        int minimumVariableCounter = 0;
        int maximumVariableCounter = 8;
        int numberOfRows = 0;

        if (Pawns.Count == 0)
            return 0;

        Check:

        if (Pawns.Count > minimumVariableCounter && Pawns.Count <= maximumVariableCounter)
        {
            numberOfRows++;

            return numberOfRows;
        }

        else
        {
            numberOfRows++;

            minimumVariableCounter = maximumVariableCounter;
            maximumVariableCounter += MaxNumberOfPawnsInTheNextRowAccordingToThe(numberOfRows);

            goto Check;
        }
    }

    int MaxNumberOfPawnsInTheNextRowAccordingToThe(int value)
    {
        int maxNumberOfPawnsInTheNextRow = 0;

        int counter = value;

        SetMaxNumberOfPawnsOfTheNextRowAccordingToCounter();

        return maxNumberOfPawnsInTheNextRow;

        void SetMaxNumberOfPawnsOfTheNextRowAccordingToCounter()
        {
            for (int i = 2; i < counter + 3; i += 2)
            {
                maxNumberOfPawnsInTheNextRow = 4 * i;

                counter++;
            }
        }
    }

    private void LateUpdate()
    {
        pawnsGameObject.transform.position = playersTransform.position;

        CheckIfIIsSmallerThanPawnsCount();

        void CheckIfIIsSmallerThanPawnsCount()
        {
            for (int i = 0; i < Pawns.Count; i++)
            {
                Pawns[i].transform.localPosition = localPositions[i];
            }
        }
    }
}