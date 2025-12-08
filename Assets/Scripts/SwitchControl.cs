using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchControl : MonoBehaviour
{
    //  Œ–¿¡À‹
    public CinemachineCamera shipCamera;
    public ShipMovement ship;
    
    // »√–Œ 
    public PlayerMovement player;
    public CinemachineCamera playerCamera;
    public PlayerInput playerInput;
    public CharacterController characterController;

    public enum ControlMode { Player, Ship }
    public ControlMode currentMode;

    private void Awake()
    {
        currentMode = ControlMode.Player;
        toPlayer();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Tab) && currentMode == ControlMode.Player)
        {
            toShip();
        }
        else if(Input.GetKey(KeyCode.Tab))
        {
            toPlayer();
        }
    }

    void SetMode(ControlMode mode)
    {
        currentMode = mode;

        if (player != null)
            player.enabled = (mode == ControlMode.Player);
        if(ship != null) 
            ship.enabled = (mode == ControlMode.Ship);
    }

    void toPlayer()
    {
        SetMode(ControlMode.Player);

        playerCamera.Priority = 2;
        shipCamera.Priority = 0;

        player.GetComponent<Rigidbody>().isKinematic = false;
        playerInput.enabled = true;
        characterController.enabled = true;
    }

    void toShip()
    {
        SetMode(ControlMode.Ship);
        
        shipCamera.Priority = 2;
        playerCamera.Priority = 0;
        

        player.GetComponent<Rigidbody>().isKinematic = true;
        playerInput.enabled = false;
        characterController.enabled = false;
    }
}
