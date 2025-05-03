using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject greenCube;
    public GameObject blueCube;

    public void OnChangeCubeState(bool isGreen)
    {
        if (isGreen)
        {
            greenCube.SetActive(!greenCube.activeSelf);
        }
        else
        {
            greenCube.SetActive(!blueCube.activeSelf);
        }
    }
}
