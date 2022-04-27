using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]
public class PlayerControlsTPS : MonoBehaviour
{
    [Header("Player Movement")]
    [Tooltip("Movement Speed of the player")]
    public float movementSpeed = 2.0f;
    [Tooltip("Sprinting Speed of the player")]
    public float runningSpeed = 6.0f;
    [Tooltip("Player turn rate")]
    [Range(0.0f, 0.4f)]
    public float rotationSmoothRate = 0.1f;
    [Tooltip("Player rate of speed change")]
    public float speedChnageRate = 10.0f;

    [Header("Player Grounded")]
    [Tooltip("check if the player is Grounded")]
    public bool isGrounded = true;
    [Tooltip("Rough ground offset, useful for complicated terrains")]
    [Range(-1, 1)]
    public float groundOffset = -0.15f;
    [Tooltip("Radius of the above check")]
    public float groundRadius = 0.3f;
    [Tooltip("Layers approved to be the ground")]
    public LayerMask groundLayers;

    [Space(10)]
    [Header("Player Cinemachine Camera Controls")]
    [Tooltip("Follow target set for virtual camera that is active")]
    public GameObject CinemachineVirtualCamera;
    [Tooltip("In degrees, how high up it moves")]
    public float topClamp = 70.0f;
    [Tooltip("In degrees, how far down it moves")]
    public float bottomClamp = -30.0f;
    [Tooltip("Testing/Overloading the currently accepted degrees")]
    public float cameraClampOverride;
    [Tooltip("Locking the camera on each axis")]
    public bool lockCameraPosition;
    [Tooltip("Camera Sensitivity Speed adjustment")]
    public float cameraSensitivity;

    // Cinemachine Camera
    private float m_cinemachineTargetYaw;
    private float m_cinemachineTargetPitch;

    // Player Ground Settings
    private float m_speed;
    private float m_animationBlend;
    private float m_rotation = 0.0f;
    private float m_rotationVelocity;
    private float m_forwardVelocity;
    private float m_maxForwardVelocity = 55.0f;
    private bool m_rotateOnMove = true;

    private Animator m_animator;
    private CharacterController m_characterController;
    private InputManagerTPS m_input;
    private GameObject m_mainCamera;

    private bool m_hasAnimator;
    private const float m_threshold = 0.01f;

    private void Awake()
    {
        if (m_mainCamera == null)
            m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hasAnimator = TryGetComponent(out m_animator);
        m_characterController = GetComponent<CharacterController>();
        m_input = GetComponent<InputManagerTPS>();
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        Moving();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void GroundCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, groundRadius, groundLayers, QueryTriggerInteraction.Ignore);
        if (m_hasAnimator)
            m_animator.SetBool("IsGrounded", isGrounded);
    }

    private void CameraRotation()
    {
        if (m_input.look.sqrMagnitude >= m_threshold && !lockCameraPosition)
        {
            m_cinemachineTargetYaw += m_input.look.x * Time.deltaTime * cameraSensitivity;
            m_cinemachineTargetPitch += m_input.look.y * Time.deltaTime * cameraSensitivity;
        }

        m_cinemachineTargetYaw = ClampAngle(m_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        m_cinemachineTargetPitch = ClampAngle(m_cinemachineTargetPitch, bottomClamp, topClamp);

        CinemachineVirtualCamera.transform.rotation = Quaternion.Euler(m_cinemachineTargetPitch + cameraClampOverride, m_cinemachineTargetYaw, 0.0f);
    }

    private void Moving()
    {
        // We need our speed to change depending on if the sprint key is pressed or not
        float targetSpeed = m_input.run ? runningSpeed : movementSpeed;

        if (m_input.move == Vector2.zero)
            targetSpeed = 0.0f;

        //  Next we need to grab the players current speeds
        float currentHorizontalSpeed = new Vector3(m_characterController.velocity.x, 0.0f, m_characterController.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMag = m_input.movement ? m_input.move.magnitude : 1.0f;

        // Now we adjust to the target speed
        if(currentHorizontalSpeed < targetSpeed - speedOffset ||
           currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            m_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMag, Time.deltaTime * speedChnageRate);
            m_speed = Mathf.Round(m_speed * 1000.0f) / 1000.0f; // This will keep it at 3 decimal places
        }
        else
        {
            m_speed = targetSpeed;
        }

        m_animationBlend = Mathf.Lerp(m_animationBlend, targetSpeed, Time.deltaTime * speedChnageRate);

        // We then need to normalise the input direction
        Vector3 inputDir = new Vector3(m_input.move.x, 0.0f, m_input.move.y).normalized;

        if(m_input.move != Vector2.zero)
        {
            m_rotation = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + m_mainCamera.transform.eulerAngles.y;
            float rotate = Mathf.SmoothDampAngle(transform.eulerAngles.y, m_rotation, ref m_rotationVelocity, rotationSmoothRate);
            if (m_rotateOnMove)
                transform.rotation = Quaternion.Euler(0.0f, rotate, 0.0f);
        }
        Vector3 targetDirection = Quaternion.Euler(0.0f, m_rotation, 0.0f) * Vector3.forward;

        m_characterController.Move(targetDirection.normalized * (m_speed * Time.deltaTime) + new Vector3(0.0f, m_forwardVelocity, 0.0f) * Time.deltaTime);

        if(m_hasAnimator)
        {
            //Vector2 move = new Vector2(m_forwardVelocity, currentHorizontalSpeed).normalized;
            m_animator.SetFloat("Speed", m_animationBlend);
            m_animator.SetFloat("SpeedMultiplier", inputMag);
        }
    }

    private static float ClampAngle(float a_angle, float a_min, float a_max)
    {
        if (a_angle < -360f)
            a_angle += 360;
        if (a_angle > 360f)
            a_angle -= 360;

        return Mathf.Clamp(a_angle,a_min,a_max);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentPurple = new Color(0.5f, 0.0f, 0.5f, 0.4f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.5f);

        if (isGrounded)
            Gizmos.color = transparentPurple;
        else
            Gizmos.color = transparentRed;

        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - groundOffset, transform.position.z), groundRadius);
    }

    public void SetCamSensitivity(float a_camSensitivity)
    {
        cameraSensitivity = a_camSensitivity;
    }

    public void SetRotateOnMove(bool a_rotate)
    {
        m_rotateOnMove = a_rotate;
    }
}
