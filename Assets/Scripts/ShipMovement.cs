using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //т. е. выполняем только если есть риджитбади

public class ShipMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed;
    public ForceMode forcemode;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float rot = h * rotationSpeed;
        rb.AddRelativeTorque(Vector3.up * rot, forcemode);

        rb.AddRelativeForce(Vector3.left * v * movementSpeed, forcemode);

    }
}
