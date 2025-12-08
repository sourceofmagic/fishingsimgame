using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cam;
    [SerializeField] private CharacterController CharacterController;

    public Vector2 move;
    public float charSpeed;


    void Start()
    {
        Cursor.visible = false;
    }

    private void OnMove(InputValue val)
    {
        move = val.Get<Vector2>();
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
