using UnityEngine;

public class cam : MonoBehaviour
{
    public GameObject VCam1, VCam2;


    void OnPrevious()
    {
        VCam2.SetActive(true);
        VCam1.SetActive(false);
    }

    void OnNext()
    {
        VCam2.SetActive(false);
        VCam1.SetActive(true);
    }
}
