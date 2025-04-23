using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float accelerationFactor = 1;
    [SerializeField]
    private float slowingFactor = 2;
    [SerializeField]
    private float maxSpeed = 10;



    private float currentSpeed = 0;
    private Vector2 moveDir = new Vector2(0, 1);
    private Rigidbody rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovementSpeed();
        UpdateRotation();
        SetHorizontalVelocity();
    }

    private void UpdateMovementSpeed ()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationFactor * 0.1f;
        }
    }

    private void UpdateRotation()
    {
        
    }

    private void SetHorizontalVelocity ()
    {
        rb.linearVelocity = new Vector3(moveDir.x * currentSpeed, rb.linearVelocity.y, moveDir.y * currentSpeed);
    }
}
