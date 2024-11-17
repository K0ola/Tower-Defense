using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class LoginManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField usernameField;
    public TMP_InputField passwordField;
    public Button loginButton;

    [Header("Settings")]
    public string loginUrl = "http://195.200.14.238:3001/auth/login";
    public string nextScene = "MenuScene";

    [System.Serializable]
    public class LoginData
    {
        public string username;
        public string password;

        public LoginData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }

    void Start()
    {
        loginButton.onClick.AddListener(() => StartCoroutine(HandleLogin()));
    }

    private IEnumerator HandleLogin()
    {
        string username = usernameField.text.Trim();
        string password = passwordField.text.Trim();

        Debug.Log($"Username : {username}, Password : {password}");

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Le pseudonyme ou le mot de passe est vide.");
            yield break;
        }

        LoginData loginData = new LoginData(username, password);
        string jsonData = JsonUtility.ToJson(loginData);
        Debug.Log($"Données envoyées : {jsonData}");

        using (UnityWebRequest request = new UnityWebRequest(loginUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            Debug.Log($"Code de réponse : {request.responseCode}");

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur de connexion : {request.error}");
            }
            else if (request.responseCode == 400)
            {
                Debug.LogError("Requête invalide : Nom d'utilisateur ou mot de passe incorrect.");
            }
            else if (request.responseCode == 403)
            {
                Debug.LogError("Connexion refusée : Veuillez vérifier votre email.");
            }
            else if (request.responseCode == 200)
            {
                Debug.Log($"Connexion réussie : {request.downloadHandler.text}");

                UserResponse userResponse = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text);

                SessionManager.Instance.SetUserSession(userResponse.user.username, userResponse.user.email);
                //Debug.Log($"Pseudonyme enregistré dans SessionManager : {SessionManager.Instance.Username}");

                // Charger la scène suivante
                SceneManager.LoadScene(nextScene);
            }
            else
            {
                Debug.LogError($"Réponse inattendue : {request.downloadHandler.text}");
            }
        }
    }
}

[System.Serializable]
public class AuthUser
{
    public string username;
    public string email;
}

[System.Serializable]
public class UserResponse
{
    public AuthUser user;
}

