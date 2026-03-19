using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private CombatConfig config;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    float lastAttackTime;

    private InputTime playerControls;

    //private void OnAttack()
    //{
    //    TryAttack();
    //}

    private void Awake()
    {
        playerControls = new InputTime();
    }
    private void OnEnable()
    {
        playerControls.Enable();
        playerControls.Player.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        playerControls.Player.Attack.performed -= OnAttack;
    }

    private void OnAttack(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
    {
        TryAttack();
    }

    public void TryAttack()
    {
        float cd = (config != null) ? config.playerAttackCooldown : 0.5f;
        if (Time.time < lastAttackTime + cd) return;
        lastAttackTime = Time.time;

        float radius = (config != null) ? config.attackRadius : 0.5f;
        int damage = (config != null) ? config.playerDamage : 1;

        var hits = Physics2D.OverlapCircleAll(attackPoint.position, radius, enemyLayer);
        foreach (var h in hits)
        {
            var dmg = h.GetComponent<IDamageable>();
            if (dmg != null) dmg.TakeDamage(damage, transform.position);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        float radius = (config != null) ? config.attackRadius : 0.5f;
        Gizmos.DrawWireSphere(attackPoint.position, radius);
    }
#endif
}
