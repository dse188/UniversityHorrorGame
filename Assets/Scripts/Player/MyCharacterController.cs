using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

/// <summary>
/// CharacterController-based first-person controller
/// No jump — exploration only. Sprint with Left Shift.
/// Look and head bob run in Update; movement uses CharacterController.Move.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class MyCharacterController : MonoBehaviour
{
    [Header("Movement")]
    [Range(1f, 6f)]  public float walkSpeed    = 3f;
    [Range(1f, 8f)]  public float runSpeed     = 5f;
    [Range(5f, 25f)] public float acceleration = 12f;
    [Range(5f, 25f)] public float deceleration = 15f;

    [Header("Look")]
    [Range(0.05f, 5f)] public float mouseSensitivity = 0.3f;
    [Range(30f, 89f)]  public float verticalClamp    = 80f;

    [Header("Head Bob")]
    [Range(0f, 0.2f)] public float headBobAmount          = 0.06f;
    [Range(0.5f, 3f)] public float headBobFrequency        = 1.4f;
    [Range(1f, 3f)]   public float headBobSprintMultiplier = 1.6f;

    [Header("Gravity")]
    [Range(-30f, -1f)] public float gravity = -9.81f;

    [Header("References")]
    [Tooltip("Child eye-height pivot — bobs during walk, used for vertical look fallback.")]
    public Transform cameraPivot;
    [Tooltip("Cinemachine virtual camera. Auto-found if left empty.")]
    public CinemachineCamera virtualCamera;

    // Visible in Inspector for debugging
    public bool isGrounded;

    // ── private ──────────────────────────────────────────────────────
    private CharacterController _cc;
    private CinemachinePanTilt  _panTilt;

    private float _panValue;
    private float _tiltValue;

    private float   _headBobTimer;
    private Vector3 _camPivotRestPos;
    private Vector3 _camPivotVel;

    private Vector2 _moveInput;
    private Vector2 _lookDelta;
    private bool    _sprinting;

    private float   _verticalVelocity;
    private Vector3 _horizontalVelocity;

    // ─────────────────────────────────────────────────────────────────

    void Start()
    {
        _cc = GetComponent<CharacterController>();

        if (virtualCamera == null)
            virtualCamera = FindFirstObjectByType<CinemachineCamera>();

        if (virtualCamera != null)
        {
            _panTilt = virtualCamera.GetComponent<CinemachinePanTilt>();
            if (_panTilt != null)
            {
                var iac = virtualCamera.GetComponent<CinemachineInputAxisController>();
                if (iac != null) iac.enabled = false;

                _panValue  = transform.eulerAngles.y;
                _tiltValue = 0f;
                _panTilt.PanAxis.Value  = _panValue;
                _panTilt.TiltAxis.Value = _tiltValue;
            }
        }

        if (cameraPivot != null)
            _camPivotRestPos = cameraPivot.localPosition;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    void Update()
    {
        _sprinting = Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;
        isGrounded = _cc.isGrounded;

        HandleLook();
        HandleMovement();
        HandleHeadBob();
    }

    // ── Input System callbacks ────────────────────────────────────────

    void OnMove(InputValue v) => _moveInput = v.Get<Vector2>();
    void OnLook(InputValue v) => _lookDelta = v.Get<Vector2>();

    // ── Look ──────────────────────────────────────────────────────────

    void HandleLook()
    {
        _panValue  += _lookDelta.x * mouseSensitivity;
        _tiltValue -= _lookDelta.y * mouseSensitivity;
        _tiltValue  = Mathf.Clamp(_tiltValue, -verticalClamp, verticalClamp);

        transform.rotation = Quaternion.Euler(0f, _panValue, 0f);

        if (_panTilt != null)
        {
            _panTilt.PanAxis.Value  = _panValue;
            _panTilt.TiltAxis.Value = _tiltValue;
        }
        else if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(_tiltValue, 0f, 0f);
        }
    }

    // ── Movement ──────────────────────────────────────────────────────

    void HandleMovement()
    {
        // Keep grounded — small negative prevents floating when walking down slopes
        if (isGrounded && _verticalVelocity < 0f)
            _verticalVelocity = -2f;

        _verticalVelocity += gravity * Time.deltaTime;

        float   speed    = _sprinting ? runSpeed : walkSpeed;
        Vector3 inputDir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 worldDir = transform.TransformDirection(inputDir);

        float blend = (inputDir.magnitude > 0.1f ? acceleration : deceleration) * Time.deltaTime;
        _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, worldDir * speed, blend);

        Vector3 motion = (_horizontalVelocity + Vector3.up * _verticalVelocity) * Time.deltaTime;
        _cc.Move(motion);
    }

    // ── Head Bob ──────────────────────────────────────────────────────

    void HandleHeadBob()
    {
        if (cameraPivot == null) return;

        float hSpeed = _horizontalVelocity.magnitude;
        bool  moving = hSpeed > 0.3f && isGrounded;

        if (moving)
        {
            float   mult   = _sprinting ? headBobSprintMultiplier : 1f;
            _headBobTimer += Time.deltaTime * headBobFrequency * mult;
            float   bob    = Mathf.Sin(_headBobTimer) * headBobAmount;
            Vector3 target = _camPivotRestPos + Vector3.up * bob;

            cameraPivot.localPosition = Vector3.SmoothDamp(
                cameraPivot.localPosition, target, ref _camPivotVel, 0.05f);
        }
        else
        {
            _headBobTimer = 0f;
            cameraPivot.localPosition = Vector3.SmoothDamp(
                cameraPivot.localPosition, _camPivotRestPos, ref _camPivotVel, 0.1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_cc == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        float radius = _cc.radius;
        Vector3 bottom = transform.position + Vector3.up * (radius - _cc.skinWidth);
        Gizmos.DrawWireSphere(bottom, radius);
    }
}