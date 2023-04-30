using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    #region Private Variables

    Transform playersTransform;

    [SerializeField]
    float camerasOffset;

    #endregion

    private void Start()
    {
        SetTheVariables();

        void SetTheVariables()
        {
            playersTransform = GameObject.Find("Player").transform;
        }
    }

    private void LateUpdate()
    {
        CheckThatWhatThisTransformsNameContains();

        void CheckThatWhatThisTransformsNameContains()
        {
            if (gameObject.transform.name.Contains("Camera"))
                transform.position = new Vector3(transform.position.x, transform.position.y, playersTransform.position.z + camerasOffset);
        }
    }
}