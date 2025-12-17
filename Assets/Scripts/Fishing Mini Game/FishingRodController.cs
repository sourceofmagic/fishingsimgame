using UnityEngine;
using System;
using System.Collections;


public class FishingRodController : MonoBehaviour
{
    public bool isWater;
    public bool isCasted; // заброшена
    public bool isPulling; // вытягивается

    Animator animator;
    public GameObject bait;
    public GameObject startOfRod;
    public GameObject ropeStart;
    public GameObject ropeEnd;
    public LineRenderer Rope;


    Transform baitPosition;
    GameObject instantiatedBait;
    void Start()
    {
        animator = GetComponent<Animator>();
        FishingMiniGame.OnFishingEnd += ReturnRod;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Water"))
            {
                isWater = true;
                if (Input.GetMouseButtonDown(0) && !isCasted && !isPulling)
                {
                    Vector3 point = new Vector3(hit.point.x, 7f, hit.point.z);
                    StartCoroutine(CastRod(point));

                }
            }
            else
                isWater = false;
        }
        else
            isWater = false;

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
            if (Input.GetKeyDown(KeyCode.Space) && !isPulling)
            {
                ReturnRod();
            }
        }

        if (isCasted && Input.GetMouseButtonDown(1) && FishingMiniGame.Instance.isThereABite)
        { 
            PullRod();
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

    public void ReturnRod()
    {
        animator.SetTrigger(isCasted == true ? "Cancel" : "Cancel2");


        Rope.enabled = false;
        isCasted = false;
        isPulling = false;
        Destroy(instantiatedBait);
    }
}
