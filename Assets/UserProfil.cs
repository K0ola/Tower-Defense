using UnityEngine;
using TMPro;

public class DisplayUserInfo : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text usernameText;
    public TMP_Text levelText;

    void Start()
    {
        if (SessionManager.Instance != null)
        {
            Debug.Log("SessionManager trouvé.");
            if (!string.IsNullOrEmpty(SessionManager.Instance.Username))
            {
                UpdateUserInfo();
            }
            else
            {
                Debug.LogError("Le pseudonyme est vide dans SessionManager.");
            }
        }
        else
        {
            Debug.LogError("SessionManager n'est pas initialisé.");
        }
    }

    private void UpdateUserInfo()
    {
        string username = SessionManager.Instance.Username;
        int level = SessionManager.Instance.Level;

        if (usernameText != null)
        {
            usernameText.text = $"{username}";
        }
        else
        {
            Debug.LogError("usernameText n'est pas assigné dans l'Inspector.");
        }

        if (levelText != null)
        {
            levelText.text = $"Lv {level}";
        }
        else
        {
            Debug.LogError("levelText n'est pas assigné dans l'Inspector.");
        }
    }
}
