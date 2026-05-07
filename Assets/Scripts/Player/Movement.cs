using Assets.Scripts.Player;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private PlayerInputHandler input;
    private Rigidbody rb;
    private Transform cameraTransform;

    private Vector3 moveDirection;

    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInputHandler>();

        if (!cameraTransform)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Vector2 moveInput = input.MoveInput;

        // Camera-relatvice directions
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten to ground plane
        camForward.y = 0f;
        camRight.y = 0f;

        camForward.Normalize();
        camRight.Normalize();

        moveDirection =
            camForward * moveInput.y +
            camRight * moveInput.x;

        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);
        //moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveDirection.x * moveSpeed;
        velocity.z = moveDirection.z * moveSpeed;
        rb.linearVelocity = velocity;

        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            Quaternion newRotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);

            rb.MoveRotation(newRotation);
        }
    }

}
