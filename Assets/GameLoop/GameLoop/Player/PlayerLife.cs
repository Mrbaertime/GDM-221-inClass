using UnityEngine;
using System.Collections;

public class PlayerLife : MonoBehaviour, IDamageable
{
    [Header("Combat Config")]
    [SerializeField] private CombatConfig config;

    [Header("HP")]
    [SerializeField] private int currentHP;

    [SerializeField] private MonoBehaviour playerController;
    [SerializeField] private Rigidbody2D rb;

    [Header("Invincibility")]
    [SerializeField] private float invincibleTime = 0f;
    public bool IsInvincible => _invincible;

    bool _invincible;
    Coroutine _coInv;
    [SerializeField] private SpriteRenderer sr;

    public void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        ResetHP();
    }

    public void ResetHP()
    {
        int max = (config != null) ? config.playerMaxHP : 5;
        currentHP = max;
    }


    void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Kill()
    {
        if (_invincible) return;
        GameManager.I?.PlayerDied();
    }

    public void SetControlEnabled(bool enabled)
    {
        if (playerController != null)
            playerController.enabled = enabled;
    }

    public void StopMotion()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }

    public void TeleportTo(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetInvincible(float seconds)
    {
        if (_coInv != null) StopCoroutine(_coInv);
        if (seconds <= 0f) { _invincible = false; return; }
        _coInv = StartCoroutine(CoInvincible(seconds));
    }

    IEnumerator CoInvincible(float seconds)
    {
        _invincible = true;
        yield return new WaitForSeconds(seconds);
        _invincible = false;
        _coInv = null;
    }

    public void TakeDamage(int amount, Vector2 Pos)
    {
        if (_invincible) return;

        currentHP -= Mathf.Max(0, amount);
        StartCoroutine(Flash());
        Debug.Log($"Player Hp: {currentHP}");
        if (currentHP <= 0) { Kill(); return; }
        if (invincibleTime > 0f) SetInvincible(invincibleTime);

    }
    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(1);
        sr.color = Color.white;
    }


}
