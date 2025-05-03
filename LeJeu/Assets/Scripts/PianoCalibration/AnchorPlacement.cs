using UnityEngine;
using Oculus.Interaction;
using Oculus.Voice.Windows;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefab;
    public Transform leftHandTrackingTransform;
    public Transform rightHandTrackingTransform;
    public GameObject canvaCalibrationPiano;
    public GameObject canvaAncreGauche;
    public GameObject canvaAncreDroite;
    public GameObject menuDuJeu;
    public GameObject pianoPrefab;

    private GameObject leftAnchor;
    private GameObject rightAnchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Trouver la référence de la main gauche si elle n'est pas assignée
        if (leftHandTrackingTransform == null)
            leftHandTrackingTransform = GameObject.Find("LeftHandAnchor")?.transform;

        if (rightHandTrackingTransform == null)
            rightHandTrackingTransform = GameObject.Find("RightHandAnchor")?.transform;
    }

    public void CalibratePiano()
    {
        if (canvaAncreGauche != null)
        {
            canvaAncreGauche.SetActive(true);
        }
        if (canvaCalibrationPiano != null)
        {
            canvaCalibrationPiano.SetActive(false);
        }
    }
    public void CreateSpacialAnchorLeft()
    {
        Vector3 leftHandPosition = Vector3.zero;
    

        if (leftHandTrackingTransform != null)
        {
            leftHandPosition = leftHandTrackingTransform.position;
           
        }

         leftAnchor = Instantiate(anchorPrefab, leftHandPosition, Quaternion.identity);

        if (canvaAncreGauche != null)
        {
            canvaAncreGauche.SetActive(false);
        }
        if (canvaAncreDroite != null)
        {
            canvaAncreDroite.SetActive(true);
        }
    }

    public void CreateSpacialAnchorRight()
    {
        Vector3 rightHandPosition = Vector3.zero;
        

        if (leftHandTrackingTransform != null)
        {
            rightHandPosition = rightHandTrackingTransform.position;

        }

         rightAnchor = Instantiate(anchorPrefab, rightHandPosition, Quaternion.identity);

        if (canvaAncreDroite != null)
        {
            canvaAncreDroite.SetActive(false);
        }
        if (menuDuJeu != null)
        {
            menuDuJeu.SetActive(true);
        }
        PlaceThirdObject();
    }

    public void PlaceThirdObject()
    {
        // Récupérer les positions des ancres
        Vector3 leftAnchorPosition = leftAnchor.transform.position;
        Vector3 rightAnchorPosition = rightAnchor.transform.position;

        // Calculer le point central
        Vector3 centerPosition = (leftAnchorPosition + rightAnchorPosition) / 2f;

        // Calculer la distance entre les ancres (largeur)
        float width = Vector3.Distance(leftAnchorPosition, rightAnchorPosition);

        // Instancier ou mettre à jour le troisième objet
        // Par exemple, si tu as un prefab pour le piano :
        GameObject thirdObject = Instantiate(pianoPrefab, centerPosition, Quaternion.identity);
        
        // Adapter la largeur du piano (supposons que le piano a un Transform ou un RectTransform)
        // Si c'est un RectTransform (pour un UI) :
        RectTransform rectTransform = thirdObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
        // Sinon, si c'est un Transform 3D :
        else
        {
            thirdObject.transform.localScale = new Vector3(width, thirdObject.transform.localScale.y, thirdObject.transform.localScale.z);
        }
    }

}

