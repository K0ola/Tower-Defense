using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 2.5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private CameraController cameraController;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        Quaternion camYaw = cameraController.GetCameraRotation();
        Vector3 moveDir = camYaw * inputDir;

        // Mouvement
        if (inputDir.magnitude >= 0.1f)
        {
            controller.Move(moveDir * moveSpeed * Time.deltaTime);
        }

        // Rotation
        if (cameraController.IsFPSView())
        {
            // FPS : toujours aligné avec la caméra
            transform.rotation = camYaw;
        }
        else if (inputDir.magnitude >= 0.1f && cameraController.IsRotating())
        {
            // TPS : orienté selon le mouvement seulement quand clic droit
            Vector3 lookDir = new Vector3(moveDir.x, 0f, moveDir.z);
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        // Saut
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
