using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Canvas à gérer")]
    [SerializeField] private GameObject homeCanvas;
    [SerializeField] private GameObject chooseLevelCanvas;

    public void OnChapterClicked()
    {
        if (homeCanvas != null)
            homeCanvas.SetActive(false);


        if (chooseLevelCanvas != null)
            chooseLevelCanvas.SetActive(true);
    }
}
