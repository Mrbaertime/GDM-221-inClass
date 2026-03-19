using UnityEngine;

public class SwapCamera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject VCam1, VCam2;


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            VCam2.SetActive(true);
            VCam1.SetActive(false);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            VCam2.SetActive(false);
            VCam1.SetActive(true);
        }
    }
}
