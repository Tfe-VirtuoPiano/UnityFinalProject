using UnityEngine;
using Oculus.Interaction;
using Oculus.Interaction.Input;

public class AnchorPlacement : MonoBehaviour
{
    public GameObject anchorPrefab;
    public GameObject canvaCalibrationPiano;
    public GameObject canvaAncreGauche;
    public GameObject canvaAncreDroite;
    public GameObject menuDuJeu;
    public GameObject pianoPrefab;


    private GameObject leftAnchor;
    private GameObject rightAnchor;

    private IHand _leftHand;
    private IHand _rightHand;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Trouver les références des mains
        var hands = FindObjectsOfType<Hand>();
        foreach (var hand in hands)
        {
            if (hand.Handedness == Handedness.Left)
            {
                _leftHand = hand;
            }
            else if (hand.Handedness == Handedness.Right)
            {
                _rightHand = hand;
            }
        }
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
        if (_leftHand != null && _leftHand.GetJointPose(HandJointId.HandIndexTip, out Pose pose))
        {
            leftAnchor = Instantiate(anchorPrefab, pose.position, pose.rotation);
        }

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
        if (_rightHand != null && _rightHand.GetJointPose(HandJointId.HandIndexTip, out Pose pose))
        {
            rightAnchor = Instantiate(anchorPrefab, pose.position, pose.rotation);
        }

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
        // Récupérer les positions et rotations des ancres
        Vector3 leftAnchorPosition = leftAnchor.transform.position;
        Vector3 rightAnchorPosition = rightAnchor.transform.position;
        Quaternion leftAnchorRotation = leftAnchor.transform.rotation;
        Quaternion rightAnchorRotation = rightAnchor.transform.rotation;

        // Calculer le point central
        Vector3 centerPosition = (leftAnchorPosition + rightAnchorPosition) / 2f;

        // Calculer la direction entre les ancres
        Vector3 direction = (rightAnchorPosition - leftAnchorPosition).normalized;
        
        // Calculer la rotation pour aligner le piano
        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        
        // Ajouter une rotation de 90 degrés sur l'axe Y
        targetRotation *= Quaternion.Euler(0, 90, 0);
        
        // Calculer la distance entre les ancres (largeur)
        float width = Vector3.Distance(leftAnchorPosition, rightAnchorPosition);

        // Instancier le piano avec la position et rotation calculées
        GameObject thirdObject = Instantiate(pianoPrefab, centerPosition, targetRotation);
        
        // Adapter la largeur du piano
        RectTransform rectTransform = thirdObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }
        else
        {
            thirdObject.transform.localScale = new Vector3(width, thirdObject.transform.localScale.y, thirdObject.transform.localScale.z);
        }
    }
}

