using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    PlayerInput.GroundedActions grounded;
    Rigidbody rb;
    GameObject rotator;
    GameObject cam;

    bool isGrounded = false;
    public float speed;
    public float jumpForce;

    float xRot = 0;
    public float mouseSensitivity;
    public float controllerSensitivity;
    public float maxSpeed;

    public GameObject floor;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = new PlayerInput();
        grounded = playerInput.Grounded;
        rotator = transform.GetChild(0).gameObject;
        cam = GameObject.FindWithTag("MainCamera");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        grounded.Enable();
    }

    private void OnDisable()
    {
        grounded.Disable();
    }

    private void Update()
    {
        Look(grounded.Look.ReadValue<Vector2>(), grounded.Look2.ReadValue<Vector2>());
        Move(grounded.Move.ReadValue<Vector2>());
        Jump(grounded.Jump.ReadValue<float>());
        if (grounded.Grapple.ReadValue<float>() > 0)
        {
            Destroy(floor);
        }
    }

    void Jump(float input)
    {
        if (isGrounded && input > 0)
        {
            rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void Look(Vector2 input, Vector2 input2)
    {
        Vector2 gud;
        if (input.x + input.y == 0 && Mathf.Abs(input2.x) + Mathf.Abs(input2.y) > 0)
        {
            gud = input2 * controllerSensitivity;
        }
        else
        {
            gud = input * mouseSensitivity;
        }
        
        xRot -= (gud.y * Time.deltaTime);
        xRot = Mathf.Clamp(xRot, -80, 80);
        cam.transform.localRotation = Quaternion.Euler(xRot, 0, 0);
        rotator.transform.Rotate(Vector3.up * (gud.x * Time.deltaTime));
    }

    private void Move(Vector2 input)
    {
        if (isGrounded)
        {
            Vector3 moveDir = new Vector3(input.x, 0, input.y);
            moveDir = cam.transform.TransformDirection(moveDir);
            moveDir *= speed * Time.deltaTime;
            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed)
            {
                rb.AddForce(moveDir.x, 0, 0, ForceMode.Impulse);
            }
            if (Mathf.Abs(rb.linearVelocity.z) < maxSpeed)
            {
                if (Mathf.Abs(moveDir.y) > Mathf.Abs(moveDir.z))
                {
                    rb.AddForce(0, 0, moveDir.y, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(0, 0, moveDir.z, ForceMode.Impulse);
                }
            }
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
    }
}
