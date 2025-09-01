using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoginUI : MonoBehaviour
{

    [Header("Champs de saisie")]
    public InputField emailInput;
    public InputField passwordInput;

    [Header("UI")]
    public Button loginButton;
    public TextMeshProUGUI errorText;
    
    [Header("Scene Management")]
    public SceneTransitionManager sceneTransitionManager;
    
    [Header("Auto Login")]
    public bool enableAutoLogin = false;
    public string autoLoginEmail = "ldevroye1@gmail.com";
    public string autoLoginPassword = "Lilian123";
    public float autoLoginDelay = 2f;

    private AuthManager authManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        authManager = FindFirstObjectByType<AuthManager>();
        loginButton.onClick.AddListener(OnLoginClicked);
        
        // Auto login si activé
        if (enableAutoLogin)
        {
            StartCoroutine(AutoLogin());
        }
    }

    private void OnLoginClicked()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ShowError("Veuillez remplir tous les champs.");
            return;
        }

        authManager.LoginFromUI(email, password, OnLoginSuccess, ShowError);
    }

        private void OnLoginSuccess()
    {
        Debug.Log("Connexion réussie !");
        errorText.text = "";
        
        // Utiliser la transition avec fade si disponible
        if (sceneTransitionManager != null)
        {
            sceneTransitionManager.LoadSceneWithFade("SampleScene");
        }
        else
        {
            // Fallback vers le changement direct
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void ShowError(string message)
    {
        errorText.text = message;
    }
    
    private IEnumerator AutoLogin()
    {
        Debug.Log("Auto login activé - Attente de " + autoLoginDelay + " secondes...");
        
        // Attendre le délai configuré
        yield return new WaitForSeconds(autoLoginDelay);
        
        // Remplir automatiquement les champs
        if (emailInput != null)
        {
            emailInput.text = autoLoginEmail;
        }
        
        if (passwordInput != null)
        {
            passwordInput.text = autoLoginPassword;
        }
        
        Debug.Log("Tentative de connexion automatique avec : " + autoLoginEmail);
        
        // Effectuer la connexion automatique
        authManager.LoginFromUI(autoLoginEmail, autoLoginPassword, OnLoginSuccess, ShowError);
    }
    
    // Méthode publique pour déclencher l'auto login manuellement
    public void TriggerAutoLogin()
    {
        if (enableAutoLogin)
        {
            StartCoroutine(AutoLogin());
        }
    }
    
    // Méthode pour activer/désactiver l'auto login
    public void SetAutoLogin(bool enabled)
    {
        enableAutoLogin = enabled;
    }
    
    // Méthode pour configurer les identifiants d'auto login
    public void SetAutoLoginCredentials(string email, string password)
    {
        autoLoginEmail = email;
        autoLoginPassword = password;
    }
}

