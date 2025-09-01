using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Collections;

[System.Serializable]
public class ErrorResponse
{
    public string message;
}

public class AuthManager : MonoBehaviour
{
    //[SerializeField] private string testEmail = "test@example.com";
    //[SerializeField] private string testPassword = "password123";
    //[SerializeField] private bool autoLoginOnStart = true;

    [Header("Environment Settings")]
    public bool useLocalEnvironment = true;
    public string localApiUrl = "http://localhost:3000/api/auth/unity";
    public string deployedApiUrl = "https://virtuo-piano.vercel.app/api/auth/unity";
    
    public delegate void ErrorHandler(string message);
    public event ErrorHandler OnError;

    private string authToken;
    private string currentApiUrl;

    private void Start()
    {
        // Définir l'URL de l'API selon l'environnement choisi
        SetApiUrl();
        
        //if (autoLoginOnStart)
        //{
        //    LoginFromUI(testEmail, testPassword,
        //        () => Debug.Log("Connexion auto réussie !"),
        //        (err) => Debug.LogError("Échec de connexion auto : " + err)
        //    );
        //}
    }
    
    private void SetApiUrl()
    {
        currentApiUrl = useLocalEnvironment ? localApiUrl : deployedApiUrl;
        Debug.Log($"AuthManager - Environnement : {(useLocalEnvironment ? "LOCAL" : "DEPLOYED")}");
        Debug.Log($"AuthManager - URL API : {currentApiUrl}");
    }

    public void LoginFromUI(string email, string password, System.Action onSuccess, System.Action<string> onError)
    {
        StartCoroutine(LoginRoutine(email, password, onSuccess, onError));
    }

    private IEnumerator LoginRoutine(string email, string password, System.Action onSuccess, System.Action<string> onError)
    {
        var loginData = new LoginData(email, password);
        string jsonData = JsonUtility.ToJson(loginData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(currentApiUrl, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log($"Réponse : {www.downloadHandler.text}");
                authToken = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text).token;
                PlayerPrefs.SetString("AuthToken", authToken);
                PlayerPrefs.Save();
                onSuccess?.Invoke();
            }
            else
            {
                string errorMessage = "Erreur de connexion.";
                if (!string.IsNullOrEmpty(www.downloadHandler.text))
                {
                    try
                    {
                        var errorResponse = JsonUtility.FromJson<ErrorResponse>(www.downloadHandler.text);
                        errorMessage = errorResponse.message;
                    }
                    catch { errorMessage = www.error; }
                }
                Debug.LogError("Erreur : " + errorMessage);
                onError?.Invoke(errorMessage);
            }
        }
    }
    
    // Méthodes publiques pour changer d'environnement
    public void SetLocalEnvironment()
    {
        useLocalEnvironment = true;
        SetApiUrl();
        Debug.Log("AuthManager - Passage en mode LOCAL");
    }
    
    public void SetDeployedEnvironment()
    {
        useLocalEnvironment = false;
        SetApiUrl();
        Debug.Log("AuthManager - Passage en mode DEPLOYED");
    }
    
    public void ToggleEnvironment()
    {
        useLocalEnvironment = !useLocalEnvironment;
        SetApiUrl();
        Debug.Log($"AuthManager - Changement d'environnement vers : {(useLocalEnvironment ? "LOCAL" : "DEPLOYED")}");
    }
    
    public bool IsLocalEnvironment()
    {
        return useLocalEnvironment;
    }
    
    public string GetCurrentApiUrl()
    {
        return currentApiUrl;
    }
    
    public void SetCustomApiUrl(string customUrl)
    {
        if (useLocalEnvironment)
        {
            localApiUrl = customUrl;
        }
        else
        {
            deployedApiUrl = customUrl;
        }
        SetApiUrl();
        Debug.Log($"AuthManager - URL personnalisée définie : {customUrl}");
    }
}

[System.Serializable]
public class LoginData
{
    public string email;
    public string password;

    public LoginData(string email, string password)
    {
        this.email = email;
        this.password = password;
    }
}

[System.Serializable]
public class AuthResponse
{
    public string token;
    public UserData user;
}

[System.Serializable]
public class UserData
{
    public string id;
    public string email;
    public string userName;
    public int level;
}
