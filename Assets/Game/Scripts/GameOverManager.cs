using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text baseHealthText;

    private float timer;
    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
        gameOverCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!gameEnded)
        {
            timer += Time.deltaTime;
        }
    }

    public void TriggerGameOver(bool won)
    {
        if (gameEnded) return;

        gameEnded = true;

        gameOverCanvas.SetActive(true);

        if (gameOverlay != null)
            gameOverlay.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        resultText.text = won ? "Victoire !" : "Défaite...";
        timeText.text = $"Temps : {timer:F1} s";
        // moneyText.text = $"Argent : {MoneyManager.Instance.GetMoney()}";
        baseHealthText.text = $"Vie base : {BaseHealthManager.Instance.GetCurrentHealth()}";
    }
}
