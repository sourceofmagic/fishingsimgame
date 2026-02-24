using UnityEngine;

public class FishMovement : MonoBehaviour
{
    private const float maxLeft = -447f;
    private const float maxRight = 447f;

    private float moveSpeed = 250f;
    private float changeFrequency = 0.01f;

    private float targetPosition;
    private bool movingRight = true;
    
    void Start()
    {
        targetPosition = Random.Range(maxLeft, maxRight);
        Debug.Log("Рыбка двигается на: " + targetPosition);
    }

    void Update()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, 
            new Vector3(targetPosition, transform.localPosition.y, transform.localPosition.z), 
            moveSpeed * Time.deltaTime);

        if(Mathf.Approximately(transform.position.x, targetPosition))
        {
            targetPosition = Random.Range(maxLeft, maxRight);
        }
        if(Random.value < changeFrequency)
        {
            movingRight = !movingRight;
            targetPosition = !movingRight ? maxRight : maxLeft;
        }
    }
}
