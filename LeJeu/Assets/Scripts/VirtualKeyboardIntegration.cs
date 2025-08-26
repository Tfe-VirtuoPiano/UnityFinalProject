using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VirtualKeyboardIntegration : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    
    [Header("Virtual Keyboard")]
    public GameObject virtualKeyboardPrefab;
    public Transform keyboardSpawnPoint;
    
    [Header("Settings")]
    public float keyboardDistance = 0.5f;
    public bool autoShowOnInputSelect = true;
    
    private GameObject virtualKeyboardInstance;
    private TMP_InputField currentActiveInput;
    private bool isKeyboardVisible = false;
    
    void Start()
    {
        // Créer l'instance du clavier virtuel
        CreateVirtualKeyboard();
        
        // Ajouter les listeners aux champs d'input
        SetupInputFieldListeners();
    }
    
    void CreateVirtualKeyboard()
    {
        if (virtualKeyboardPrefab != null)
        {
            // Position de spawn du clavier
            Vector3 spawnPosition;
            if (keyboardSpawnPoint != null)
            {
                spawnPosition = keyboardSpawnPoint.position;
            }
            else
            {
                // Position par défaut devant l'utilisateur
                Vector3 headPosition = Camera.main.transform.position;
                Vector3 headForward = Camera.main.transform.forward;
                spawnPosition = headPosition + headForward * keyboardDistance;
                spawnPosition.y = headPosition.y - 0.3f; // Légèrement plus bas
            }
            
            // Instancier le clavier
            virtualKeyboardInstance = Instantiate(virtualKeyboardPrefab, spawnPosition, Quaternion.identity);
            
            // Cacher le clavier au démarrage
            SetKeyboardVisibility(false);
            
            Debug.Log("Clavier virtuel créé avec succès");
        }
        else
        {
            Debug.LogError("Prefab du clavier virtuel non assigné !");
        }
    }
    
    void SetupInputFieldListeners()
    {
        if (emailInput != null)
        {
            emailInput.onSelect.AddListener((text) => OnInputFieldSelected(emailInput));
            emailInput.onEndEdit.AddListener((text) => OnInputFieldEndEdit(emailInput));
        }
        
        if (passwordInput != null)
        {
            passwordInput.onSelect.AddListener((text) => OnInputFieldSelected(passwordInput));
            passwordInput.onEndEdit.AddListener((text) => OnInputFieldEndEdit(passwordInput));
        }
    }
    
    void OnInputFieldSelected(TMP_InputField inputField)
    {
        currentActiveInput = inputField;
        
        if (autoShowOnInputSelect && virtualKeyboardInstance != null)
        {
            // Afficher le clavier
            SetKeyboardVisibility(true);
            
            // Positionner le clavier près du champ sélectionné
            PositionKeyboardNearInput(inputField);
            
            Debug.Log($"Clavier affiché pour: {inputField.name}");
        }
    }
    
    void OnInputFieldEndEdit(TMP_InputField inputField)
    {
        if (virtualKeyboardInstance != null)
        {
            // Cacher le clavier
            SetKeyboardVisibility(false);
            
            Debug.Log($"Clavier caché pour: {inputField.name}");
        }
        
        currentActiveInput = null;
    }
    
    void PositionKeyboardNearInput(TMP_InputField inputField)
    {
        if (inputField != null && virtualKeyboardInstance != null)
        {
            // Convertir la position du champ d'input en position monde
            Vector3 inputWorldPos = inputField.transform.position;
            
            // Positionner le clavier en dessous du champ
            Vector3 keyboardPos = inputWorldPos;
            keyboardPos.y -= 0.2f; // 20cm en dessous
            keyboardPos.z += 0.1f; // Légèrement en avant
            
            virtualKeyboardInstance.transform.position = keyboardPos;
        }
    }
    
    void SetKeyboardVisibility(bool visible)
    {
        if (virtualKeyboardInstance != null)
        {
            virtualKeyboardInstance.SetActive(visible);
            isKeyboardVisible = visible;
        }
    }
    
    // Méthodes publiques pour contrôler le clavier manuellement
    public void ShowKeyboard()
    {
        SetKeyboardVisibility(true);
        Debug.Log("Clavier affiché manuellement");
    }
    
    public void HideKeyboard()
    {
        SetKeyboardVisibility(false);
        Debug.Log("Clavier caché manuellement");
    }
    
    public void ToggleKeyboard()
    {
        SetKeyboardVisibility(!isKeyboardVisible);
    }
    
    // Méthode pour repositionner le clavier
    public void RepositionKeyboard(Vector3 newPosition)
    {
        if (virtualKeyboardInstance != null)
        {
            virtualKeyboardInstance.transform.position = newPosition;
        }
    }
    
    // Méthode pour repositionner le clavier devant l'utilisateur
    public void RepositionKeyboardInFront()
    {
        if (virtualKeyboardInstance != null && Camera.main != null)
        {
            Vector3 headPosition = Camera.main.transform.position;
            Vector3 headForward = Camera.main.transform.forward;
            Vector3 keyboardPos = headPosition + headForward * keyboardDistance;
            keyboardPos.y = headPosition.y - 0.3f;
            
            virtualKeyboardInstance.transform.position = keyboardPos;
        }
    }
    
    void OnDestroy()
    {
        // Nettoyer les listeners
        if (emailInput != null)
        {
            emailInput.onSelect.RemoveAllListeners();
            emailInput.onEndEdit.RemoveAllListeners();
        }
        
        if (passwordInput != null)
        {
            passwordInput.onSelect.RemoveAllListeners();
            passwordInput.onEndEdit.RemoveAllListeners();
        }
    }
}
