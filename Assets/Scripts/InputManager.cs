using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public InputSystem_Actions actions;

    public enum InputType { Player, Ship, Fishing, UI };

    // грузим этот скрипт самым первым при инициализации
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        // если объект уже есть
        if (Instance != null) return;

        // создаем объект и не удал€ем при смене сцены
        GameObject go = new GameObject("InputManager");
        Instance = go.AddComponent<InputManager>();
        DontDestroyOnLoad(go);
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // экземпл€р ассета управлени€
        actions = new InputSystem_Actions();
        actions.Enable();

        ChangeInputMap(InputType.Player);
    }

    public void ChangeInputMap(InputType inputType)
    {
        switch(inputType)
        {
            case InputType.Player:
                actions.Player.Enable();
                actions.UI.Enable();

                actions.Ship.Disable();
                actions.Fishing.Disable();
                break;
            case InputType.Ship:
                actions.Ship.Enable();

                actions.Player.Disable();
                break;
            case InputType.Fishing:
                actions.Fishing.Enable();

                actions.Player.Disable();                
                actions.UI.Disable();
                break;
            case InputType.UI:
                actions.UI.Enable();
                
                actions.Player.Disable();
                actions.Ship.Disable();              
                break;
        }
    }

    // чистим пам€ть
    private void OnDestroy()
    {
        if (Instance == this)
        {
            actions.Disable();
            actions = null;
            Instance = null;
        }
    }
}
