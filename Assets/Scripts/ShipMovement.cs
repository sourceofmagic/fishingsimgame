using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))] //т. е. выполняем только если есть риджитбади

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 550;
    [SerializeField] private float rotationSpeed = 400;
    public ForceMode forcemode;

    private Rigidbody rb;
    private Vector2 move;
    private InputManager inputManager;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = InputManager.Instance;
    }

    private void OnEnable()
    {
        if(inputManager?.actions?.Ship != null)
        {
            inputManager.actions.Ship.Move.performed += OnMovePerformed;
            inputManager.actions.Ship.Move.canceled += OnMoveCanceled;
        }
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.Ship != null)
        {
            inputManager.actions.Ship.Move.performed -= OnMovePerformed;
            inputManager.actions.Ship.Move.canceled -= OnMoveCanceled;
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

    private void FixedUpdate()
    {

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        float movement = move.y * movementSpeed;
        float rot = move.x * rotationSpeed;

        rb.AddRelativeTorque(Vector3.up * rot, forcemode);

        rb.AddRelativeForce(Vector3.left * movement, forcemode);

    }
}
