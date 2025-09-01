using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [Header("Transition Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;
    public Color fadeColor = Color.black;
    
    [Header("Auto Setup")]
    public bool createFadeImage = true;
    
    void Start()
    {
        if (createFadeImage && fadeImage == null)
        {
            CreateFadeImage();
        }
        
        // Fade in au démarrage
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }
    
    void CreateFadeImage()
    {
        // Créer un canvas pour le fade
        GameObject canvasGO = new GameObject("TransitionCanvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // Au-dessus de tout
        
        // Ajouter un CanvasScaler
        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        
        // Créer l'image de fade
        GameObject imageGO = new GameObject("FadeImage");
        imageGO.transform.SetParent(canvasGO.transform, false);
        
        fadeImage = imageGO.AddComponent<Image>();
        fadeImage.color = fadeColor;
        
        // Configurer l'image pour couvrir tout l'écran
        RectTransform rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
    }
    
    public void LoadSceneWithFade(string sceneName)
    {
        StartCoroutine(LoadSceneWithFadeCoroutine(sceneName));
    }
    
    IEnumerator LoadSceneWithFadeCoroutine(string sceneName)
    {
        // Fade out
        yield return StartCoroutine(FadeOut());
        
        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneName);
    }
    
    IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;
        
        float elapsedTime = 0f;
        Color startColor = fadeColor;
        startColor.a = 1f;
        Color endColor = fadeColor;
        endColor.a = 0f;
        
        fadeImage.color = startColor;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
    
    IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;
        
        float elapsedTime = 0f;
        Color startColor = fadeColor;
        startColor.a = 0f;
        Color endColor = fadeColor;
        endColor.a = 1f;
        
        fadeImage.color = startColor;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }
        
        fadeImage.color = endColor;
    }
}
