using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CharacterController CharacterController;

    public Vector2 move;
    public float charSpeed;

    private InputManager inputManager;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        if (CharacterController == null)
            CharacterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        if (inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.Move.performed += OnMovePerformed;
            inputManager.actions.Player.Move.canceled += OnMoveCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.Move.performed -= OnMovePerformed;
            inputManager.actions.Player.Move.canceled -= OnMoveCanceled;
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

    void FixedUpdate()
    {
        CharacterController.Move((GetForward() * move.y + GetRight() * move.x) * Time.deltaTime * charSpeed);
    }
  

    private Vector3 GetForward()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0f;

        return forward.normalized;
    }

    private Vector3 GetRight()
    {
        Vector3 right = cam.transform.right;
        right.y = 0f;

        return right.normalized;
    }

}
