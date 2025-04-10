using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }

    [Header("Réglages")]
    [SerializeField] private int startMoney = 500;

    [Header("UI")]
    [SerializeField] private TMP_Text moneyText;

    private int currentMoney;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentMoney = startMoney;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (amount > currentMoney)
        {
            Debug.Log("Pas assez d'argent !");
            return false;
        }

        currentMoney -= amount;
        UpdateMoneyUI();
        return true;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    private void UpdateMoneyUI()
    {
        if (moneyText != null)
        {
            moneyText.text = currentMoney.ToString();
        }
    }

    public int GetMoney()
    {
        return currentMoney;
    }

}
