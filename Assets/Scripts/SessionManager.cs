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
    public string ApiUrl = "http://195.200.14.238:3001/user/getuser"; // Assurez-vous que l'URL correspond � l'API

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SessionManager initialis� et persistant.");
        }
        else
        {
            Debug.Log("SessionManager d�j� existant, destruction de ce duplicata.");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// D�finit manuellement la session utilisateur et synchronise les informations.
    /// </summary>
    public void SetUserSession(string username, string email)
    {
        Username = username;
        Email = email;

        // Synchroniser les informations utilisateur depuis l'API
        StartCoroutine(FetchUserData(username));
    }

    /// <summary>
    /// R�cup�re les informations utilisateur depuis l'API.
    /// </summary>
    private IEnumerator FetchUserData(string username)
    {
        string url = $"{ApiUrl}/{username}";
        Debug.Log($"URL utilis�e : {url}");

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Content-Type", "application/json"); // Optionnel, mais utile pour certaines APIs
            yield return request.SendWebRequest();

            Debug.Log($"Code de r�ponse : {request.responseCode}");

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Erreur r�seau : {request.error}");
            }
            else if (request.responseCode == 200)
            {
                Debug.Log($"Donn�es utilisateur r�cup�r�es : {request.downloadHandler.text}");

                // Parsez les donn�es utilisateur et mettez � jour la session
                try
                {
                    UserData userData = JsonUtility.FromJson<UserData>(request.downloadHandler.text);
                    Username = userData.user_username;
                    Email = userData.user_email;
                    Level = userData.user_lv;
                    Experience = userData.user_xp;

                    Debug.Log($"Session mise � jour : Username = {Username}, Email = {Email}, Level = {Level}, XP = {Experience}");
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Erreur lors du parsing des donn�es utilisateur : {ex.Message}");
                }
            }
            else if (request.responseCode == 404)
            {
                Debug.LogError("Utilisateur non trouv�.");
            }
            else
            {
                Debug.LogError($"R�ponse inattendue : {request.responseCode}, {request.downloadHandler.text}");
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
