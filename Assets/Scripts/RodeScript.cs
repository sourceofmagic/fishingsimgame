using UnityEngine;

public class RodeScript : MonoBehaviour
{
    public GameObject startRope;
    public GameObject endRope;
    LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        
    }
    void LateUpdate()
    {
        lr.SetPosition(0, startRope.transform.position);
        lr.SetPosition(1, endRope.transform.position);
    }
}
