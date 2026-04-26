using UnityEngine;
using System;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;


public class FishingRodController : MonoBehaviour
{

    public static FishingRodController Instance { get; set; }

    public bool isWater;
    public bool isCasting; // забрасывается
    public bool isCasted; // заброшена
    public bool isPulling; // вытягивается

    Animator animator;
    private InputManager inputManager;
    public GameObject bait;
    public GameObject startOfRod;
    public GameObject ropeStart;
    public GameObject ropeEnd;
    public LineRenderer Rope;


    private RaycastHit hit;


    Transform baitPosition;
    GameObject instantiatedBait;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        animator = GetComponent<Animator>();
        inputManager = InputManager.Instance;
        FishingMiniGame.OnFishingEnd += ReturnRod;
    }

    private void OnDestroy()
    {
        FishingMiniGame.OnFishingEnd -= ReturnRod;
    }

    private void OnEnable()
    {
        if (inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.CastAFishingRod.performed += OnCastPerformed;
        }
        if (inputManager?.actions?.Fishing != null)
        {
            inputManager.actions.Fishing.Pull.performed += OnPullPerformed;
        }
    }

    private void OnDisable()
    {
        if (inputManager?.actions?.Player != null)
        {
            inputManager.actions.Player.CastAFishingRod.performed -= OnCastPerformed;
        }
        if (inputManager?.actions?.Fishing != null)
        {
            inputManager.actions.Fishing.Pull.performed -= OnPullPerformed;
        }
    }

    private void OnCastPerformed(InputAction.CallbackContext ctx)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        if (WaterCheck(ray) && !isCasted && !isPulling)
        {
            Vector3 point = new Vector3(hit.point.x, 7f, hit.point.z);
            StartCoroutine(CastRod(point));
        }
    }
    private void OnPullPerformed(InputAction.CallbackContext ctx)
    {
        if (isCasted && FishingMiniGame.Instance.isThereABite)
        {
            PullRod();
        }
    }

    private bool WaterCheck(Ray ray)
    {
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Water"))
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isCasted || isPulling)
        {
            if (ropeStart != null && startOfRod != null && ropeEnd != null)
            {
                ropeStart.transform.position = startOfRod.transform.position;
                if (baitPosition != null)
                {
                    ropeEnd.transform.position = baitPosition.position;
                }
            }
        }

    }

    IEnumerator CastRod(Vector3 targetPosition)
    {
        isCasted = true;

        animator.SetTrigger("Cast");
        yield return new WaitForSeconds(1f);

        instantiatedBait = Instantiate(bait);
        instantiatedBait.transform.position = targetPosition;
        Rope.enabled = true;

        baitPosition = instantiatedBait.transform;

        FishingMiniGame.Instance.StartFishing(); // начало мини-игры
    }

    private void PullRod()
    {

        animator.SetTrigger("Pull");

        isCasted = false;
        isPulling = true;

        FishingMiniGame.Instance.setHasPulled();
    }

    // не доделана
    public void ReturnRod()
    {

        Rope.enabled = false;
        Destroy(instantiatedBait);

        animator.SetTrigger(isCasted == true ? "Cancel" : "Cancel2");
        isCasted = false;
        isPulling = false;

    }
}
