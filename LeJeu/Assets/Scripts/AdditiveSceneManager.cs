using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AdditiveSceneManager : MonoBehaviour
{
    [Header("Scene Management")]
    public string[] availableScenes = { "SampleScene", "FourreTout", "VirtualKeyboard" };
    
    [Header("Settings")]
    public bool loadScenesAdditively = true;
    public bool unloadOtherScenes = false;
    
    private List<string> loadedScenes = new List<string>();
    
    void Start()
    {
        // Ajouter la scène actuelle à la liste
        string currentScene = SceneManager.GetActiveScene().name;
        if (!loadedScenes.Contains(currentScene))
        {
            loadedScenes.Add(currentScene);
        }
    }
    
    // Charger une scène en mode additif
    public void LoadSceneAdditively(string sceneName)
    {
        if (!loadedScenes.Contains(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            loadedScenes.Add(sceneName);
            Debug.Log($"Scène '{sceneName}' chargée en mode additif");
        }
        else
        {
            Debug.Log($"Scène '{sceneName}' déjà chargée");
        }
    }
    
    // Décharger une scène spécifique
    public void UnloadScene(string sceneName)
    {
        if (loadedScenes.Contains(sceneName))
        {
            SceneManager.UnloadSceneAsync(sceneName);
            loadedScenes.Remove(sceneName);
            Debug.Log($"Scène '{sceneName}' déchargée");
        }
        else
        {
            Debug.Log($"Scène '{sceneName}' n'est pas chargée");
        }
    }
    
    // Basculer entre deux scènes (décharger l'une, charger l'autre)
    public void SwitchBetweenScenes(string sceneToUnload, string sceneToLoad)
    {
        StartCoroutine(SwitchScenesCoroutine(sceneToUnload, sceneToLoad));
    }
    
    System.Collections.IEnumerator SwitchScenesCoroutine(string sceneToUnload, string sceneToLoad)
    {
        // Décharger la première scène
        if (loadedScenes.Contains(sceneToUnload))
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToUnload);
            while (!unloadOperation.isDone)
            {
                yield return null;
            }
            loadedScenes.Remove(sceneToUnload);
        }
        
        // Charger la nouvelle scène
        if (!loadedScenes.Contains(sceneToLoad))
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
            while (!loadOperation.isDone)
            {
                yield return null;
            }
            loadedScenes.Add(sceneToLoad);
        }
        
        Debug.Log($"Basculé de '{sceneToUnload}' vers '{sceneToLoad}'");
    }
    
    // Charger plusieurs scènes en même temps
    public void LoadMultipleScenes(string[] sceneNames)
    {
        StartCoroutine(LoadMultipleScenesCoroutine(sceneNames));
    }
    
    System.Collections.IEnumerator LoadMultipleScenesCoroutine(string[] sceneNames)
    {
        foreach (string sceneName in sceneNames)
        {
            if (!loadedScenes.Contains(sceneName))
            {
                AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!loadOperation.isDone)
                {
                    yield return null;
                }
                loadedScenes.Add(sceneName);
                Debug.Log($"Scène '{sceneName}' chargée");
            }
        }
    }
    
    // Décharger toutes les scènes sauf la principale
    public void UnloadAllScenesExcept(string mainSceneName)
    {
        StartCoroutine(UnloadAllScenesExceptCoroutine(mainSceneName));
    }
    
    System.Collections.IEnumerator UnloadAllScenesExceptCoroutine(string mainSceneName)
    {
        List<string> scenesToUnload = new List<string>(loadedScenes);
        scenesToUnload.Remove(mainSceneName);
        
        foreach (string sceneName in scenesToUnload)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);
            while (!unloadOperation.isDone)
            {
                yield return null;
            }
            loadedScenes.Remove(sceneName);
            Debug.Log($"Scène '{sceneName}' déchargée");
        }
    }
    
    // Obtenir la liste des scènes chargées
    public List<string> GetLoadedScenes()
    {
        return new List<string>(loadedScenes);
    }
    
    // Vérifier si une scène est chargée
    public bool IsSceneLoaded(string sceneName)
    {
        return loadedScenes.Contains(sceneName);
    }
    
    // Méthodes publiques pour les boutons UI
    public void LoadSampleSceneAdditively() => LoadSceneAdditively("SampleScene");
    public void LoadFourreToutAdditively() => LoadSceneAdditively("FourreTout");
    public void LoadVirtualKeyboardAdditively() => LoadSceneAdditively("VirtualKeyboard");
    
    public void UnloadSampleScene() => UnloadScene("SampleScene");
    public void UnloadFourreTout() => UnloadScene("FourreTout");
    public void UnloadVirtualKeyboard() => UnloadScene("VirtualKeyboard");
}
