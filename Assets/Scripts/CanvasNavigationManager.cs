using UnityEngine;

public class CanvasNavigationManager : MonoBehaviour
{
    public GameObject homeCanvas; // R�f�rence au Canvas HOME
    public GameObject inventoryCanvas; // R�f�rence au Canvas INVENTORY
    public GameObject classmentCanvas; // R�f�rence au Canvas CLASSMENT

    public void ShowHomeCanvas()
    {
        Debug.Log("ShowHomeCanvas() appel�");
        ActivateCanvas(homeCanvas);
    }

    public void ShowInventoryCanvas()
    {
        Debug.Log("ShowInventoryCanvas() appel�");
        ActivateCanvas(inventoryCanvas);
    }

    public void ShowClassmentCanvas()
    {
        Debug.Log("ShowClassmentCanvas() appel�");
        ActivateCanvas(classmentCanvas);
    }

    private void ActivateCanvas(GameObject canvasToActivate)
    {
        homeCanvas.SetActive(canvasToActivate == homeCanvas);
        inventoryCanvas.SetActive(canvasToActivate == inventoryCanvas);
        classmentCanvas.SetActive(canvasToActivate == classmentCanvas);
    }
}
