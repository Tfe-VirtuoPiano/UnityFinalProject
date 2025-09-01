using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class SimpleInputFieldManager : MonoBehaviour
{
    [Header("Input Fields")]
    public InputField[] inputFields;
    
    [Header("Virtual Keyboard")]
    public OVRVirtualKeyboard virtualKeyboard;
    
    [Header("Settings")]
    public bool autoFocusOnStart = true;
    public bool cycleThroughFields = true;
    
    private int currentFieldIndex = 0;
    private List<OVRVirtualKeyboardInputFieldTextHandler> textHandlers = new List<OVRVirtualKeyboardInputFieldTextHandler>();
    
    void Start()
    {
        // Trouver le clavier virtuel s'il n'est pas assigné
        if (virtualKeyboard == null)
        {
            virtualKeyboard = FindObjectOfType<OVRVirtualKeyboard>();
        }
        
        // Créer les gestionnaires de texte pour chaque champ
        CreateTextHandlers();
        
        // Configurer les événements de focus
        SetupFocusEvents();
        
        // Focus automatique sur le premier champ
        if (autoFocusOnStart && inputFields.Length > 0)
        {
            FocusField(0);
        }
    }
    
    void CreateTextHandlers()
    {
        // Supprimer les anciens gestionnaires
        foreach (var handler in textHandlers)
        {
            if (handler != null)
            {
                DestroyImmediate(handler.gameObject);
            }
        }
        textHandlers.Clear();
        
        // Créer un nouveau gestionnaire pour chaque champ
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i] == null) continue;
            
            // Créer un GameObject pour le gestionnaire
            GameObject handlerGO = new GameObject($"TextHandler_{i}");
            handlerGO.transform.SetParent(virtualKeyboard.transform);
            
            // Ajouter le composant de gestionnaire
            OVRVirtualKeyboardInputFieldTextHandler handler = handlerGO.AddComponent<OVRVirtualKeyboardInputFieldTextHandler>();
            handler.InputField = inputFields[i];
            
            textHandlers.Add(handler);
        }
    }
    
    void SetupFocusEvents()
    {
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i] == null) continue;
            
            int fieldIndex = i; // Capture pour la closure
            
            // Événement de début d'édition (quand on clique sur le champ)
            inputFields[i].onValueChanged.AddListener((string value) => {
                // Focus automatique quand on commence à taper
                if (currentFieldIndex != fieldIndex)
                {
                    FocusField(fieldIndex);
                }
            });
            
            // Événement de fin d'édition (pour passer au champ suivant)
            inputFields[i].onEndEdit.AddListener((string value) => {
                if (cycleThroughFields && fieldIndex < inputFields.Length - 1)
                {
                    // Passer au champ suivant après validation
                    FocusField(fieldIndex + 1);
                }
            });
            
            // Ajouter un EventTrigger pour détecter les clics
            AddClickDetection(inputFields[i], fieldIndex);
        }
    }
    
    void AddClickDetection(InputField inputField, int fieldIndex)
    {
        // Ajouter un EventTrigger si il n'existe pas déjà
        EventTrigger trigger = inputField.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = inputField.gameObject.AddComponent<EventTrigger>();
        }
        
        // Créer l'événement de clic
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => {
            FocusField(fieldIndex);
        });
        
        // Ajouter l'événement au trigger
        trigger.triggers.Add(entry);
    }
    
    public void FocusField(int index)
    {
        if (index < 0 || index >= inputFields.Length || inputFields[index] == null)
            return;
            
        currentFieldIndex = index;
        
        // Activer le focus sur le champ
        inputFields[index].Select();
        inputFields[index].ActivateInputField();
        
        // Connecter le gestionnaire de texte au clavier virtuel
        if (index < textHandlers.Count && textHandlers[index] != null)
        {
            virtualKeyboard.TextHandler = textHandlers[index];
        }
        
        Debug.Log($"Focus sur le champ {index}: {inputFields[index].name}");
    }
    
    public void FocusNextField()
    {
        int nextIndex = (currentFieldIndex + 1) % inputFields.Length;
        FocusField(nextIndex);
    }
    
    public void FocusPreviousField()
    {
        int prevIndex = (currentFieldIndex - 1 + inputFields.Length) % inputFields.Length;
        FocusField(prevIndex);
    }
    
    public void FocusEmailField()
    {
        // Chercher le champ email par nom ou tag
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i] != null && 
                (inputFields[i].name.ToLower().Contains("email") || 
                 inputFields[i].name.ToLower().Contains("mail")))
            {
                FocusField(i);
                return;
            }
        }
    }
    
    public void FocusPasswordField()
    {
        // Chercher le champ password par nom ou tag
        for (int i = 0; i < inputFields.Length; i++)
        {
            if (inputFields[i] != null && 
                (inputFields[i].name.ToLower().Contains("password") || 
                 inputFields[i].name.ToLower().Contains("pass") ||
                 inputFields[i].name.ToLower().Contains("mdp")))
            {
                FocusField(i);
                return;
            }
        }
    }
    
    // Méthodes publiques pour les boutons UI
    public void OnEmailFieldSelected()
    {
        FocusEmailField();
    }
    
    public void OnPasswordFieldSelected()
    {
        FocusPasswordField();
    }
    
    public void OnNextField()
    {
        FocusNextField();
    }
    
    public void OnPreviousField()
    {
        FocusPreviousField();
    }
}
