using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField]
    private float accelerationFactor = 1;
    [SerializeField]
    private float slowingFactor = 2;
    [SerializeField]
    private float maxWalkingSpeed = 5;
    [SerializeField]
    private float runningMultiplier = 2;
    [SerializeField]
    private float jumpForce = 10;

    [Header("Ground Checking")]
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private float groundCheckSize = 1.2f;
    [SerializeField]
    private float groundCheckCenter = 1.2f;

    private float currentSpeed = 0;
    private Vector2 moveInput = new Vector2(0, 0);
    private Rigidbody rb;
    private Transform playerCamera;

    private Vector2 mouseMovement;

    private float mouseSensitivity = 1f;
    private float rotationX;
    private float rotationY;
    private float lookXLimit = 90;
    private bool horizontalInput = false;

    private bool onGround = false;
    private bool running = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ResetCurMoveSpeed();

        playerCamera = GetComponentInChildren<Camera>().transform;
    }

    private void Update()
    {
        CheckSurrounding();
        UpdateMovementSpeed();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetHorizontalVelocity();
    }

    private void CheckSurrounding()
    {
        onGround = Physics.BoxCast(transform.position + Vector3.down * groundCheckCenter, new Vector3(transform.localScale.x, 0, transform.localScale.z) * 0.5f, Vector3.down, transform.rotation, groundCheckSize * transform.localScale.y, whatIsGround);
    }

    private void UpdateMovementSpeed ()
    {
        float speedMult = running ? runningMultiplier : 1;
        if (horizontalInput)
        {
            currentSpeed += (accelerationFactor * maxWalkingSpeed) * speedMult * Time.deltaTime;

            if(currentSpeed > maxWalkingSpeed * speedMult)
            {
                currentSpeed = maxWalkingSpeed * speedMult;
            }
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
        Vector3 moveDir = currentSpeed * (transform.forward * moveInput.y + transform.right * moveInput.x) + new Vector3(0,rb.linearVelocity.y,0);

        rb.linearVelocity = moveDir;
    }

    private void ResetCurMoveSpeed()
    {
        currentSpeed = maxWalkingSpeed * 0.4f;
    }




    //INPUT Handling -------------------------------------------------------------------------------------------------------------------------------

    public void MouseMoved(InputAction.CallbackContext context)
    {
        mouseMovement = context.ReadValue<Vector2>();
        UpdateRotation();
    }

    public void MovePressed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>().normalized;
        horizontalInput = moveInput.x != 0 || moveInput.y != 0;
        if (!horizontalInput)
        {
            ResetCurMoveSpeed();
        } 
    }

    public void RunningPressed(InputAction.CallbackContext context)
    {
        if(context.canceled)
        {
            running = false;
        } else if (context.performed)
        {
            running = true;
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if(context.performed && onGround)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }





    //Debug Methods -------------------------------------------------------------------------------------------------------------------------------

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + Vector3.down * (groundCheckCenter + 0.5f * groundCheckSize), new Vector3(transform.localScale.x, transform.localScale.y * groundCheckSize, transform.localScale.z));
    }
}

