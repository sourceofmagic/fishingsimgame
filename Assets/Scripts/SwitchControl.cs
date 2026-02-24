using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchControl : MonoBehaviour
{

    public static SwitchControl Instance { get; set; }


    // КОРАБЛЬ
    public CinemachineCamera shipCamera;
    public ShipMovement ship;
    
    // ИГРОК
    public PlayerMovement player;
    public CinemachineCamera playerCamera;
    public PlayerInput playerInput;
    public CharacterController characterController;

    // УДОЧКА
    public GameObject fishingRod;

    public enum ControlMode { Player, Ship, Fishing}
    public ControlMode currentMode;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        currentMode = ControlMode.Player;
        toPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            switch (currentMode)
            {
                case (ControlMode.Player):
                    toShip();
                    break;
                case (ControlMode.Ship):
                    toPlayer();
                    break;

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

    public void toPlayer()
    {
        SetMode(ControlMode.Player);
        Debug.Log("Контроль игрока");

        playerCamera.Priority = 2;
        shipCamera.Priority = 0;


        player.GetComponent<Rigidbody>().isKinematic = false;
        playerInput.enabled = true;
        characterController.enabled = true;


        fishingRod.GetComponent<FishingRodController>().enabled = true;
    }

    public void toShip()
    {
        SetMode(ControlMode.Ship);
        Debug.Log("Контроль корабля");

        shipCamera.Priority = 2;
        playerCamera.Priority = 0;


        player.GetComponent<Rigidbody>().isKinematic = true;
        playerInput.enabled = false;
        characterController.enabled = false;

        fishingRod.GetComponent<FishingRodController>().enabled = false;
    }

    public void toFishing()
    {
        SetMode(ControlMode.Fishing);
        player.GetComponent<Rigidbody>().isKinematic = true;
        playerInput.enabled = false;
        characterController.enabled = false;
    }
}
