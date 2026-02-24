using UnityEngine;

public class FishCursorMovement : MonoBehaviour
{

    private const float maxLeft = -353f;
    private const float maxRight = 353f;

    private float moveSpeed = 500f;
    void Update()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if(moveInput != 0)
        {
            MoveCursor(moveInput);
        }
    }

    private void MoveCursor(float moveInput)
    {
        Vector3 movement = Vector3.right * moveInput * moveSpeed * Time.deltaTime;

        Vector3 newPosition = transform.localPosition + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, maxLeft, maxRight);

        transform.localPosition = newPosition;

    }
}
