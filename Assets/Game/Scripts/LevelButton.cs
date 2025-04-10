using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private int levelIndex;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

        if (!ProgressManager.IsLevelUnlocked(levelIndex))
        {
            button.interactable = false;
            if (buttonText != null)
                buttonText.color = Color.gray;
        }

        button.onClick.AddListener(() =>
        {
            if (!string.IsNullOrEmpty(sceneName))
            {
                SceneManager.LoadScene(sceneName);
            }
            else
            {
                Debug.LogError("Scene name is empty!");
            }
        });
    }
}
