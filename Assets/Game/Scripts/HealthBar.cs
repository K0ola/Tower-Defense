using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);

    public void UpdateHealthBar(float current, float max)
    {
        if (fillImage != null)
            fillImage.fillAmount = current / max;

        if (healthText != null)
            healthText.text = Mathf.CeilToInt(current).ToString(); // ou $"{current}/{max}"
    }

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.LookAt(Camera.main.transform);
        }
    }
}
