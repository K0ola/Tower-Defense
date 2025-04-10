using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pitchMin = -45f;
    [SerializeField] private float pitchMax = 75f;
    private float yaw = 0f;
    private float pitch = 20f;

    [Header("Zoom")]
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 6f;
    [SerializeField] private float zoomSpeed = 2f;
    private float currentZoom = 5f;

    [Header("TPS Settings")]
    [SerializeField] private float thirdPersonHeight = 2f;

    [Header("FPS Settings")]
    [SerializeField] private Vector3 fpsCameraOffset = new Vector3(0f, 1.6f, 0.1f); // 👈 Position caméra FPS personnalisable

    private bool rotating = false;

    void LateUpdate()
    {
        HandleZoom();
        HandleRotation();

        Vector3 finalPos;
        Vector3 lookTarget;

        if (IsFPSView())
        {
            // 📌 FPS view : caméra directement à l'offset défini
            finalPos = target.position + Quaternion.Euler(0f, yaw, 0f) * fpsCameraOffset;
            lookTarget = finalPos + Quaternion.Euler(pitch, yaw, 0f) * Vector3.forward;

        }
        else
        {
            // 🎥 TPS view : orbit autour du joueur
            float tpsHeight = thirdPersonHeight;
            Vector3 heightOffset = new Vector3(0, tpsHeight, 0);

            Vector3 direction = Quaternion.Euler(pitch, yaw, 0f) * new Vector3(0, 0, -currentZoom);
            finalPos = target.position + heightOffset + direction;
            lookTarget = target.position + heightOffset;
        }

        transform.position = finalPos;
        transform.LookAt(lookTarget);

        HandleCursor();
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        currentZoom -= scroll * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    void HandleRotation()
    {
        bool isFPS = IsFPSView();

        if (isFPS || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;
            pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);
        }

        rotating = Input.GetMouseButton(1) || isFPS;
    }

    void HandleCursor()
    {
        if (IsFPSView())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public float GetYaw() => yaw;
    public Quaternion GetCameraRotation() => Quaternion.Euler(0, yaw, 0);
    public bool IsFPSView() => currentZoom <= minZoom + 0.1f;
    public bool IsRotating() => rotating;
}
