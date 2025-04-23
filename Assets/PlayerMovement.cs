using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float accelerationFactor = 6;
    [SerializeField]
    private float slowingFactor = 2;
    [SerializeField]
    private float maxSpeed = 10;



    private float currentSpeed = 0;
    private Vector2 moveInput = new Vector2(0, 0);
    private Rigidbody rb;
    private Transform playerCamera;

    private Vector2 mouseMovement;

    private float mouseSensitivity = 1f;
    private float rotationX;
    private float rotationY;
    private float lookXLimit = 90;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateMovementSpeed();
        SetHorizontalVelocity();
    }

    private void UpdateMovementSpeed ()
    {
        if (currentSpeed < maxSpeed)
        {
            currentSpeed += accelerationFactor * 0.2f;
        }
    }

    private void UpdateRotation()
    {
        rotationX += -mouseMovement.y * 0.05f * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        rotationY += mouseMovement.x * 0.1f * mouseSensitivity;

        transform.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    private void SetHorizontalVelocity ()
    {
        Vector3 moveDir = currentSpeed * (transform.forward * moveInput.y + transform.right * moveInput.x) + new Vector3(0, rb.linearVelocity.y,0);

        rb.linearVelocity = moveDir;
    }

    public void MouseMoved(InputAction.CallbackContext context)
    {
        mouseMovement = context.ReadValue<Vector2>();
        UpdateRotation();
    }

    public void Moving(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        if(moveInput.x == 0 && moveInput.y == 0)
        {
            currentSpeed = maxSpeed/10f;
        }
    }

}

