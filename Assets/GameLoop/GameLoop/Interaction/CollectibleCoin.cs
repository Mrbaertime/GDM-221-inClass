using UnityEngine;

public class CollectibleCoin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private AudioClip collectSound;  

    void Reset()
    {
        var c = GetComponent<Collider2D>();
        c.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        GameManager.I?.AddCoin(value);

        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

        gameObject.SetActive(false);
    }

}
