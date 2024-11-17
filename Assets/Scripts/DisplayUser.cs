using UnityEngine;
using TMPro;
using System;

public class DisplayUsername : MonoBehaviour
{
    public TMP_Text usernameText;

    void Start()
    {
        Console.WriteLine("Affichage du nom d'utilisateur.");
        Console.WriteLine($"SessionManager.Instance: {SessionManager.Instance}");
        if (SessionManager.Instance != null && !string.IsNullOrEmpty(SessionManager.Instance.Username))
        {
            Console.WriteLine($"Username: {SessionManager.Instance.Username}");
            usernameText.text = $"{SessionManager.Instance.Username}";
        }
        else
        {
            Console.WriteLine("Aucun utilisateur connecté.");
            return;
        }
    }
}

