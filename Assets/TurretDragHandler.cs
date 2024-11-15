using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class TurretDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject turretPrefab;
    [SerializeField] private LayerMask placementLayers;

    private GameObject turretInstance;
    private bool isValidPlacement;

    private List<Renderer> renderers = new List<Renderer>();
    private List<Color> originalColors = new List<Color>();

    public void OnBeginDrag(PointerEventData eventData)
    {
        turretInstance = Instantiate(turretPrefab);

        renderers.Clear();
        originalColors.Clear();

        Renderer[] rendererArray = turretInstance.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in rendererArray)
        {
            if (renderer != null)
            {
                renderers.Add(renderer);
                originalColors.Add(renderer.material.color);
            }
        }

        Turret turret = turretInstance.GetComponent<Turret>();
        if (turret != null)
        {
            turret.isPlaced = false;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayers))
        {
            turretInstance.transform.position = hit.point;

            isValidPlacement = CheckValidPlacement(hit.point);

            SetTurretColor(isValidPlacement);
        }
        else
        {
            isValidPlacement = false;
            SetTurretColor(isValidPlacement);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isValidPlacement)
        {

            for (int i = 0; i < renderers.Count; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer != null)
                {
                    renderer.material.color = originalColors[i];
                }
            }

            Turret turret = turretInstance.GetComponent<Turret>();
            if (turret != null)
            {
                turret.OnPlaced();
            }

        }
        else
        {
            Destroy(turretInstance);
        }

        turretInstance = null;
        renderers.Clear();
        originalColors.Clear();
    }

    private bool CheckValidPlacement(Vector3 position)
    {
        if (IsOverlappingOtherTurrets(position))
        {
            return false;
        }

        return true;
    }

    private bool IsOverlappingOtherTurrets(Vector3 position)
    {
        float radius = 0.5f;

        Collider turretCollider = turretInstance.GetComponent<Collider>();
        if (turretCollider != null)
        {
            turretCollider.enabled = false;
        }

        Collider[] colliders = Physics.OverlapSphere(position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Turret") || collider.CompareTag("Obstacle"))
            {
                if (turretCollider != null)
                {
                    turretCollider.enabled = true;
                }
                return true;
            }
        }

        if (turretCollider != null)
        {
            turretCollider.enabled = true;
        }

        return false;
    }

    private void SetTurretColor(bool isValid)
    {
        if (turretInstance == null)
        {
            return;
        }

        for (int i = 0; i < renderers.Count; i++)
        {
            Renderer renderer = renderers[i];
            if (renderer != null)
            {
                renderer.material.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
            }
        }
    }
}
