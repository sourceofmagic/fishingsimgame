using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class SwitchControl : MonoBehaviour
{

    public static SwitchControl Instance { get; private set; }


    // КОРАБЛЬ
    public CinemachineCamera shipCamera;
    public ShipMovement ship;
    
    // ИГРОК
    public PlayerMovement player;
    public CinemachineCamera playerCamera;
    public CharacterController characterController;

    // УДОЧКА
    public GameObject fishingRod;

    // UI
    public GameObject textBox;
    

    private InputManager inputManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        inputManager = InputManager.Instance;
        setFishCountText();
        inputManager.ChangeInputMap(InputManager.InputType.Player);
    }

    private void OnEnable()
    {
        if(inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.SwitchToShip.performed += OnSwitchToShip;
            inputManager.actions.Player.CastAFishingRod.performed += OnSwitchToFishing;
        }
        if (inputManager?.actions?.Ship != null)
        {
            inputManager.actions.Ship.SwitchToPlayer.performed += OnSwitchToPlayer;
        }
        FishingMiniGame.OnFishingEnd += SwitchToPlayer;
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.SwitchToShip.performed -= OnSwitchToShip;
            inputManager.actions.Player.CastAFishingRod.performed -= OnSwitchToFishing;
        }
        if (inputManager?.actions?.Ship != null)
        {
            inputManager.actions.Ship.SwitchToPlayer.performed -= OnSwitchToPlayer;
        }
        FishingMiniGame.OnFishingEnd -= SwitchToPlayer;
    }

    public void OnSwitchToPlayer(InputAction.CallbackContext ctx)
    {
        SwitchToPlayer();
    }

    public void SwitchToPlayer()
    {
        Debug.Log("Контроль игрока");
        inputManager.ChangeInputMap(InputManager.InputType.Player);

        playerCamera.Priority = 2;
        shipCamera.Priority = 0;


        player.enabled = true;
        ship.enabled = false;

        characterController.enabled = true;

        fishingRod.SetActive(true);
        fishingRod.GetComponent<FishingRodController>().enabled = true;
        setFishCountText();
    }

    public void OnSwitchToShip(InputAction.CallbackContext ctx)
    {
        SwitchToShip();
    }
    public void SwitchToShip()
    {
        Debug.Log("Контроль корабля");
        inputManager.ChangeInputMap(InputManager.InputType.Ship);

        shipCamera.Priority = 2;
        playerCamera.Priority = 0;

        player.enabled = false;
        ship.enabled = true;

        characterController.enabled = false;

        fishingRod.GetComponent<FishingRodController>().enabled = false;
        fishingRod.SetActive(false);
    }

    public void OnSwitchToFishing(InputAction.CallbackContext ctx)
    {
        if(FishingRodController.Instance.isCasted) SwitchToFishing();
    }
    public void SwitchToFishing()
    {
        inputManager.ChangeInputMap(InputManager.InputType.Fishing);
        player.enabled = false;
    }

    public void setFishCountText()
    {
        textBox.GetComponent<TextMeshProUGUI>().text = "Рыб поймано: " + GameManager.Instance.getFishAmount(GameManager.Instance.currentFishingResult);
    }
}
