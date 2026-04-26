using UnityEngine;
using UnityEngine.InputSystem;

public class FishCursorMovement : MonoBehaviour
{

    private const float maxLeft = -353f;
    private const float maxRight = 353f;

    [SerializeField] private float moveSpeed = 500f;

    [SerializeField] private Vector2 move;
    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        if (inputManager?.actions?.Fishing != null)
        {
            inputManager.actions.Fishing.Move.performed += OnMovePerformed;
            inputManager.actions.Fishing.Move.canceled += OnMoveCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.Fishing != null)
        {
            inputManager.actions.Fishing.Move.performed -= OnMovePerformed;
            inputManager.actions.Fishing.Move.canceled -= OnMoveCanceled;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        move = Vector2.zero;
    }

    void Update()
    {
        float moveInput = move.x;

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
