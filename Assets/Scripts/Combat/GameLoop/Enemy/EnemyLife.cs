using System.Collections;
using UnityEngine;

public class EnemyLife : MonoBehaviour, IDamageable
{
    [SerializeField] private CombatConfig config;
    [SerializeField] private int currentHP;
    [SerializeField] private SpriteRenderer sr ;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ResetHP();
    }

    void ResetHP()
    {
        int max = (config != null) ? config.enemyMaxHP : 3;
        currentHP = max;
    }

    public void TakeDamage(int amount, Vector2 Pos)
    {
        currentHP -= Mathf.Max(0, amount);
        StartCoroutine(Flash());
        Debug.Log($"Enemy HP: {currentHP}");

        if (currentHP <= 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(1);
        sr.color = Color.white;
    }
}
