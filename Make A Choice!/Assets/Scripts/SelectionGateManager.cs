using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class SelectionGateManager : MonoBehaviour
{
    #region Private Variables

    int selectionGateCountToSpawn;

    [SerializeField]
    GameObject selectionGatePrefab;

    float planesLocalScalesZAxis;

    [SerializeField]
    float spacing;

    float planesPositionsZAxis;

    int averageOfPlusAndMinus;

    int averageOfPlusAndMinussSpacing;

    int averageOfMultiplicationAndDivide;

    int averageOfMultiplicationAndDividesSpacing;

    int averageOfPlus;
    int averageOfMinus;

    int averageOfMultiplication;
    int averageOfDivide;

    bool isTheLastSettedGatesPositive = false;

    [SerializeField]
    GameObject plane;

    [SerializeField]
    GameManager gameManager;

    #endregion

    [Obsolete]
    private void Start()
    {
        SetTheVariables();

        SpawnAllTheSelectionGates();

        void SetTheVariables()
        {
            int level = PlayerPrefs.HasKey("Level") ? PlayerPrefs.GetInt("Level") : 1;

            int increasingAmount = (level - level % 3) / 3;

            Vector3 planesTransformsLocalScale = plane.transform.localScale;

            float previousPlaneLocalScaleZAxisValue = plane.transform.localScale.z;

            selectionGateCountToSpawn = increasingAmount + 5;

            averageOfPlusAndMinus = increasingAmount + 3;

            averageOfPlusAndMinussSpacing = increasingAmount + 1;

            averageOfMultiplicationAndDivide = increasingAmount + 2;

            averageOfMultiplicationAndDividesSpacing = increasingAmount + 1;

            plane.transform.localScale = new(planesTransformsLocalScale.x, planesTransformsLocalScale.y, 2.8f * selectionGateCountToSpawn + 1);

            planesLocalScalesZAxis = plane.transform.localScale.z;

            plane.transform.position += new Vector3(0, 0, (planesLocalScalesZAxis - previousPlaneLocalScaleZAxisValue) * 5);

            planesPositionsZAxis = plane.transform.position.z;

            averageOfPlus = averageOfPlusAndMinus;

            averageOfMinus = -averageOfPlusAndMinus;

            averageOfMultiplication = averageOfMultiplicationAndDivide;

            averageOfDivide = averageOfMultiplicationAndDivide;
        }

        void SpawnAllTheSelectionGates()
        {
            for (int i = 0; i < selectionGateCountToSpawn; i++)
            {
                SpawnSelectionGate(i);
            }
        }

        void SpawnSelectionGate(int i)
        {
            GameObject selectionGate = Instantiate(selectionGatePrefab, new Vector3(0, 0, planesPositionsZAxis - (planesLocalScalesZAxis * 10 / 2 - spacing) + (i * ((planesLocalScalesZAxis * 10 - spacing) / selectionGateCountToSpawn))), Quaternion.identity);

            EditTheSelectionGate();

            void EditTheSelectionGate()
            {
                Transform selectionGatesCanvassTransform = selectionGate.transform.FindChild("Canvas").gameObject.transform;

                if (isTheLastSettedGatesPositive)
                {
                    int randomNegativeValue = Random.Range(averageOfMinus - averageOfPlusAndMinussSpacing, averageOfMinus + averageOfPlusAndMinussSpacing);

                    CheckTheRandomNegativeValue:

                    if (randomNegativeValue >= 0)
                    {
                        randomNegativeValue = Random.Range(averageOfMinus - averageOfPlusAndMinussSpacing, averageOfMinus + averageOfPlusAndMinussSpacing);
                        goto CheckTheRandomNegativeValue;
                    }

                    selectionGatesCanvassTransform.FindChild("First Gate").transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text = randomNegativeValue.ToString();
                    averageOfMinus -= randomNegativeValue - averageOfMinus;

                    int randomDividingValue = Random.Range(averageOfDivide - averageOfMultiplicationAndDividesSpacing, averageOfDivide + averageOfMultiplicationAndDividesSpacing);

                    CheckTheRandomDividingValue:

                    if (randomDividingValue <= 0)
                    {
                        randomDividingValue = Random.Range(averageOfDivide - averageOfMultiplicationAndDividesSpacing, averageOfDivide + averageOfMultiplicationAndDividesSpacing);
                        goto CheckTheRandomDividingValue;
                    }

                    selectionGatesCanvassTransform.FindChild("Second Gate").transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text = "/" + randomDividingValue.ToString();
                    averageOfDivide -= randomDividingValue - averageOfDivide;

                    isTheLastSettedGatesPositive = false;
                }

                else
                {
                    int randomPositiveValue = Random.Range(averageOfPlus - averageOfPlusAndMinussSpacing, averageOfPlus + averageOfPlusAndMinussSpacing);

                    CheckTheRandomPositiveValue:

                    if (randomPositiveValue < 0)
                    {
                        randomPositiveValue = Random.Range(averageOfPlus - averageOfPlusAndMinussSpacing, averageOfPlus + averageOfPlusAndMinussSpacing);
                        goto CheckTheRandomPositiveValue;
                    }

                    selectionGatesCanvassTransform.FindChild("First Gate").transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text = "+" + randomPositiveValue.ToString();
                    averageOfPlus -= randomPositiveValue - averageOfPlus;

                    int randomMultiplitingValue = Random.Range(averageOfMultiplication - averageOfMultiplicationAndDividesSpacing, averageOfMultiplication + averageOfMultiplicationAndDividesSpacing);

                    RecheckTheRandomMultiplatingValue:

                    if (randomMultiplitingValue <= 0)
                    {
                        randomMultiplitingValue = Random.Range(averageOfMultiplication - averageOfMultiplicationAndDividesSpacing, averageOfMultiplication + averageOfMultiplicationAndDividesSpacing);
                        goto RecheckTheRandomMultiplatingValue;
                    }

                    selectionGatesCanvassTransform.FindChild("Second Gate").transform.FindChild("Text").GetComponent<TextMeshProUGUI>().text = "x" + randomMultiplitingValue.ToString();
                    averageOfMultiplication -= randomMultiplitingValue - averageOfMultiplication;

                    isTheLastSettedGatesPositive = true;
                }
            }
        }
    }
}