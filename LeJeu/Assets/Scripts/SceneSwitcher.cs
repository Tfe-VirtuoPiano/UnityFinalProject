using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Scene Management")]
    public string[] sceneNames = { "SampleScene", "FourreTout", "VirtualKeyboard" };
    
    [Header("Settings")]
    public bool useFadeTransition = true;
    public float fadeDuration = 1f;
    
    private CanvasGroup fadeCanvasGroup;
    
    void Start()
    {
        // Créer un canvas de fade si nécessaire
        if (useFadeTransition)
        {
            CreateFadeCanvas();
        }
    }
    
    void CreateFadeCanvas()
    {
        // Créer un canvas pour le fade
        GameObject fadeCanvas = new GameObject("FadeCanvas");
        Canvas canvas = fadeCanvas.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Au-dessus de tout
        
        // Ajouter un CanvasGroup pour le fade
        fadeCanvasGroup = fadeCanvas.AddComponent<CanvasGroup>();
        
        // Créer l'image de fade
        GameObject fadeImage = new GameObject("FadeImage");
        fadeImage.transform.SetParent(fadeCanvas.transform, false);
        
        UnityEngine.UI.Image image = fadeImage.AddComponent<UnityEngine.UI.Image>();
        image.color = Color.black;
        
        // Étendre l'image sur tout l'écran
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        
        // Cacher au démarrage
        fadeCanvasGroup.alpha = 0f;
    }
    
    // Méthode pour changer de scène par nom
    public void SwitchToScene(string sceneName)
    {
        if (useFadeTransition)
        {
            StartCoroutine(SwitchSceneWithFade(sceneName));
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
    // Méthode pour changer de scène par index
    public void SwitchToScene(int sceneIndex)
    {
        if (useFadeTransition)
        {
            StartCoroutine(SwitchSceneWithFade(sceneIndex));
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
    
    // Méthode pour changer de scène par nom avec fade
    System.Collections.IEnumerator SwitchSceneWithFade(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneName);
        
        // Fade in
        yield return StartCoroutine(FadeIn());
    }
    
    // Méthode pour changer de scène par index avec fade
    System.Collections.IEnumerator SwitchSceneWithFade(int sceneIndex)
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneIndex);
        
        // Fade in
        yield return StartCoroutine(FadeIn());
    }
    
    // Fade out (écran devient noir)
    System.Collections.IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = 1f;
    }
    
    // Fade in (écran redevient normal)
    System.Collections.IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        
        fadeCanvasGroup.alpha = 0f;
    }
    
    // Méthodes publiques pour les boutons UI
    public void SwitchToSampleScene() => SwitchToScene("SampleScene");
    public void SwitchToFourreTout() => SwitchToScene("FourreTout");
    public void SwitchToVirtualKeyboard() => SwitchToScene("VirtualKeyboard");
    
    // Méthode pour retourner à la scène précédente
    public void GoBack()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int previousIndex = Mathf.Max(0, currentIndex - 1);
        SwitchToScene(previousIndex);
    }
    
    // Méthode pour aller à la scène suivante
    public void GoNext()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = Mathf.Min(SceneManager.sceneCountInBuildSettings - 1, currentIndex + 1);
        SwitchToScene(nextIndex);
    }
    
    // Méthode pour recharger la scène actuelle
    public void ReloadCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SwitchToScene(currentSceneName);
    }
    
    // Méthode pour quitter le jeu
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
