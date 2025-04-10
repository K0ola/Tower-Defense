using UnityEngine;

public class TurretManager : MonoBehaviour
{
    [Header("Tourelles")]
    [SerializeField] private GameObject[] turretPrefabs;
    [SerializeField] private GameObject[] ghostValidPrefabs;
    [SerializeField] private GameObject[] ghostInvalidPrefabs;

    [Header("Placement")]
    [SerializeField] private LayerMask placementMask;
    [SerializeField] private float placementYOffset = 0.5f;
    [SerializeField] private float minDistanceFromOtherTurret = 2f;
    [SerializeField] private float minDistanceFromPath = 2f;
    [SerializeField] private LayerMask turretLayer;
    [SerializeField] private LayerMask pathLayer;

    [Header("UI")]
    [SerializeField] private BounceElement[] turretUIBounce;

    private GameObject currentGhost;
    private int selectedTurretIndex = -1;
    private bool isPlacementValid = false;

    void Update()
    {
        HandleSelectionInput();

        if (selectedTurretIndex != -1)
        {
            UpdatePreview();

            if (Input.GetMouseButtonDown(0) && isPlacementValid)
            {
                PlaceTurret();
            }
        }
    }

    void HandleSelectionInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectTurret(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectTurret(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectTurret(2);
    }

    void SelectTurret(int index)
    {
        if (currentGhost != null) Destroy(currentGhost);

        // Stop bounce précédente
        if (selectedTurretIndex != -1 && selectedTurretIndex < turretUIBounce.Length)
        {
            turretUIBounce[selectedTurretIndex].StopLoopBounce();
        }

        selectedTurretIndex = index;

        // Bounce visuel de sélection
        if (index >= 0 && index < turretUIBounce.Length && turretUIBounce[index] != null)
        {
            turretUIBounce[index].Bounce();
            turretUIBounce[index].StartLoopBounce();
        }

        currentGhost = Instantiate(ghostValidPrefabs[index]);
        currentGhost.tag = "turret_preview";
    }

    void UpdatePreview()
    {
        Ray ray;

        // FPS = regard caméra | TPS = souris
        CameraController camController = FindObjectOfType<CameraController>();
        if (camController != null && camController.IsFPSView())
        {
            ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementMask))
        {
            Vector3 placePos = hit.point + Vector3.up * placementYOffset;
            isPlacementValid = ValidatePlacement(placePos);

            Destroy(currentGhost);
            currentGhost = Instantiate(
                isPlacementValid ? ghostValidPrefabs[selectedTurretIndex] : ghostInvalidPrefabs[selectedTurretIndex],
                placePos, Quaternion.identity
            );
            currentGhost.tag = "turret_preview";
        }
    }

    void PlaceTurret()
    {
        if (currentGhost == null || selectedTurretIndex == -1) return;

        GameObject turretPrefab = turretPrefabs[selectedTurretIndex];
        TurretData data = turretPrefab.GetComponent<TurretData>();

        if (data == null)
        {
            Debug.LogWarning("Pas de script TurretData sur le prefab !");
            return;
        }

        int cost = data.Cost;
        if (!MoneyManager.Instance.SpendMoney(cost))
        {
            Debug.Log("Pas assez d'argent pour cette tourelle !");
            return;
        }

        Vector3 placePosition = currentGhost.transform.position;
        Instantiate(turretPrefab, placePosition, Quaternion.identity);

        // Stop animation
        if (selectedTurretIndex >= 0 && selectedTurretIndex < turretUIBounce.Length)
        {
            turretUIBounce[selectedTurretIndex].StopLoopBounce();
        }

        Destroy(currentGhost);
        selectedTurretIndex = -1;
    }

    bool ValidatePlacement(Vector3 position)
    {
        // Trop près d’une autre tourelle ?
        if (Physics.CheckSphere(position, minDistanceFromOtherTurret, turretLayer))
            return false;

        // Trop près du chemin ?
        if (Physics.CheckSphere(position, minDistanceFromPath, pathLayer))
            return false;

        return true;
    }
}
