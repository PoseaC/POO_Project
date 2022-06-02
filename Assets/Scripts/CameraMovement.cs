using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement")]
    public Vector2 verticalMovementLimit;
    public float movementSpeed;

    Vector3 movementDirection;
    float horizontalInput;
    float verticalInput;
    void Update()
    {
        horizontalInput += -Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;

        verticalInput += Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        verticalInput = Mathf.Clamp(verticalInput, verticalMovementLimit.x, verticalMovementLimit.y);

        movementDirection = Vector3.up * horizontalInput + Vector3.right * verticalInput;

        transform.rotation = Quaternion.Euler(movementDirection);
    }
}
