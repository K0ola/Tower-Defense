using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SessionManager : MonoBehaviour
{
    public static SessionManager Instance;

    [Header("User Data")]
    public string Username;
    public string Email;
    public int Level;
    public int Experience;

    [Header("API Settings")]
    public string ApiUrl = "http://195.200.14.238:3001/user/getuser"; // Assurez-vous que l'URL correspond à l'API

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SessionManager initialisé et persistant.");
        }
        else
        {
            Debug.Log("SessionManager déjà existant, destruction de ce duplicata.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Définit manuellement la session utilisateur et synchronise les informations.
    /// </summary>
    public void SetUserSession(string username, string email)
    {
        Username = username;
        Email = email;

        // Synchroniser les informations utilisateur depuis l'API
        StartCoroutine(FetchUserData(username));
    }

    /// <summary>
    /// Récupère les informations utilisateur depuis l'API.
    /// </summary>
    private IEnumerator FetchUserData(string username)
    {
        string url = $"{ApiUrl}/{username}";
        Debug.Log($"URL utilisée : {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json"); // Optionnel, mais utile pour certaines APIs
            yield return request.SendWebRequest();

            Debug.Log($"Code de réponse : {request.responseCode}");

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur réseau : {request.error}");
            }
            else if (request.responseCode == 200)
            {
                Debug.Log($"Données utilisateur récupérées : {request.downloadHandler.text}");

                // Parsez les données utilisateur et mettez à jour la session
                try
                {
                    UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                    Username = userData.user_username;
                    Email = userData.user_email;
                    Level = userData.user_lv;
                    Experience = userData.user_xp;

                    Debug.Log($"Session mise à jour : Username = {Username}, Email = {Email}, Level = {Level}, XP = {Experience}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Erreur lors du parsing des données utilisateur : {ex.Message}");
                }
            }
            else if (request.responseCode == 404)
            {
                Debug.LogError("Utilisateur non trouvé.");
            }
            else
            {
                Debug.LogError($"Réponse inattendue : {request.responseCode}, {request.downloadHandler.text}");
            }
        }
    }
}

[System.Serializable]
public class UserData
{
    public string user_username; // Correspond au champ "user_username" de l'API
    public string user_email;    // Correspond au champ "user_email" de l'API
    public int user_lv;          // Correspond au champ "user_lv" de l'API
    public int user_xp;          // Correspond au champ "user_xp" de l'API
}
