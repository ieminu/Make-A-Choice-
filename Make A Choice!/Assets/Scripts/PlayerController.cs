using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Private Variables

    Rigidbody thisRigidbody;

    [SerializeField]
    float verticalSpeed;

    Vector3 mousePositionToWorldPoint;

    #endregion

    private void Start()
    {
        SetTheVariables();

        void SetTheVariables()
        {
            thisRigidbody = GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        SetTheVelocity();
        SetThePosition();

        void SetTheVelocity()
        {
            thisRigidbody.velocity = new Vector3(thisRigidbody.velocity.x, thisRigidbody.velocity.y, verticalSpeed * Time.deltaTime);
        }

        void SetThePosition()
        {
            thisRigidbody.position = new Vector3(mousePositionToWorldPoint.x, thisRigidbody.position.y, thisRigidbody.position.z);
        }
    }

    private void Update()
    {
        CheckIfTheMouseClicked();

        void CheckIfTheMouseClicked()
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Input.mousePosition;

                mousePositionToWorldPoint = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, transform.position.z - Camera.main.transform.position.z));
            }
        }
    }
}