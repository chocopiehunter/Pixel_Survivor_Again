using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRadius = 2.0f; // 공격 사정거리
    [SerializeField] private LayerMask enemyLayer;     // 몬스터만 골라내기 위한 레이어 필터

    [Header("Attack Cooldown")]
    [SerializeField] private float attackCooldown = 2.5f; // 공격 쿨타임
    private float lastAttackTime = 0f;

    private void Update()
    {
        // 공격 쿨타임 지났는지 체크
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            BasicAttack();
            lastAttackTime = Time.time; // 갱신
        }
    }

    private void BasicAttack()
    {
        // 쿨타임이 돌때마다 원을 그려 범위 내에 Enemy들을 모음
        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach(Collider2D enemyCollider in hitEnemy)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(10f);
                Debug.Log($"기본공격 {enemyCollider.name}에게 기본공격해서 데미지 [10] 입힘");

            }
        }
    }

    // 에디터에서 공격범위를 보기위한 유니티 기능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
