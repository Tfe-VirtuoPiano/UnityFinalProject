using UnityEngine;

public class VirtualKeyboardPrefabLoader : MonoBehaviour
{
    [Header("Auto Load Settings")]
    public bool autoLoadOnStart = true;
    public string prefabPath = "Assets/Samples/Meta XR Core SDK/74.0.2/Sample Scenes/VirtualKeyboard.unity";
    
    [Header("Loaded Prefab")]
    public GameObject loadedVirtualKeyboardPrefab;
    
    void Start()
    {
        if (autoLoadOnStart)
        {
            LoadVirtualKeyboardPrefab();
        }
    }
    
    public void LoadVirtualKeyboardPrefab()
    {
        // Essayer de charger le prefab depuis les resources
        GameObject prefab = Resources.Load<GameObject>("OVRVirtualKeyboard");
        
        if (prefab == null)
        {
            // Essayer de trouver le prefab dans les samples
            prefab = FindVirtualKeyboardPrefabInSamples();
        }
        
        if (prefab != null)
        {
            loadedVirtualKeyboardPrefab = prefab;
            Debug.Log("Prefab OVRVirtualKeyboard chargé avec succès");
        }
        else
        {
            Debug.LogError("Impossible de trouver le prefab OVRVirtualKeyboard. Vérifiez que les samples Meta XR sont installés.");
        }
    }
    
    GameObject FindVirtualKeyboardPrefabInSamples()
    {
        // Chercher dans les dossiers de samples
        string[] searchPaths = {
            "Assets/Samples/Meta XR Core SDK",
            "Assets/Samples/Oculus",
            "Assets/Oculus/Samples"
        };
        
        foreach (string path in searchPaths)
        {
            GameObject prefab = FindPrefabInDirectory(path, "OVRVirtualKeyboard");
            if (prefab != null)
            {
                return prefab;
            }
        }
        
        return null;
    }
    
    GameObject FindPrefabInDirectory(string directoryPath, string prefabName)
    {
        // Cette méthode utilise les ressources Unity pour chercher le prefab
        // En pratique, vous devrez peut-être assigner manuellement le prefab
        return null;
    }
    
    // Méthode pour assigner manuellement le prefab
    public void SetVirtualKeyboardPrefab(GameObject prefab)
    {
        loadedVirtualKeyboardPrefab = prefab;
        Debug.Log("Prefab OVRVirtualKeyboard assigné manuellement");
    }
    
    // Méthode pour obtenir le prefab chargé
    public GameObject GetVirtualKeyboardPrefab()
    {
        return loadedVirtualKeyboardPrefab;
    }
}
