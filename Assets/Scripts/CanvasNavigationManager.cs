using UnityEngine;

public class CanvasNavigationManager : MonoBehaviour
{
    public GameObject homeCanvas; // Référence au Canvas HOME
    public GameObject inventoryCanvas; // Référence au Canvas INVENTORY
    public GameObject classmentCanvas; // Référence au Canvas CLASSMENT

    public void ShowHomeCanvas()
    {
        Debug.Log("ShowHomeCanvas() appelé");
        ActivateCanvas(homeCanvas);
    }

    public void ShowInventoryCanvas()
    {
        Debug.Log("ShowInventoryCanvas() appelé");
        ActivateCanvas(inventoryCanvas);
    }

    public void ShowClassmentCanvas()
    {
        Debug.Log("ShowClassmentCanvas() appelé");
        ActivateCanvas(classmentCanvas);
    }

    private void ActivateCanvas(GameObject canvasToActivate)
    {
        homeCanvas.SetActive(canvasToActivate == homeCanvas);
        inventoryCanvas.SetActive(canvasToActivate == inventoryCanvas);
        classmentCanvas.SetActive(canvasToActivate == classmentCanvas);
    }
}
