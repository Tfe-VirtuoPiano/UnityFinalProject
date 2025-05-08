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

