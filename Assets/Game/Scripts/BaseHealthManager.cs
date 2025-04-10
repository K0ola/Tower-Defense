using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BaseHealthManager : MonoBehaviour
{
    [Header("Santé de la base")]
    [SerializeField] private int maxHealth = 500;
    private int currentHealth;

    [Header("UI")]
    [SerializeField] private Image healthFillImage;
    [SerializeField] private TMP_Text healthText;

    public static BaseHealthManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0, currentHealth);
        UpdateUI();

        if (currentHealth <= 0)
        {
            Debug.Log("💥 La base est détruite !");
            if (currentHealth <= 0)
            GameOverManager.Instance.TriggerGameOver(false);
        }
    }

    void UpdateUI()
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = (float)currentHealth / maxHealth;
        }

        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + maxHealth;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth; // pareil ici, adapte selon ta variable
    }

}
