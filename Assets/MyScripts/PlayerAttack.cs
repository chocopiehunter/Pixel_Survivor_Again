using UnityEngine;
using Assets.PixelFantasy.PixelHeroes.Common.Scripts.ExampleScripts;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRadius = 2.0f; // 공격 사정거리
    [SerializeField] private LayerMask enemyLayer;     // 몬스터만 골라내기 위한 레이어 필터

    [Header("Attack Cooldown")]
    [SerializeField] private float attackCooldown = 2.5f; // 공격 쿨타임
    private float lastAttackTime = 0f;

    // 공격 주기 감소 횟수
    private int attackUpgradeCount = 0;

    private CharacterAnimation _characterAnim;

    private void Awake()
    {
        _characterAnim = GetComponent<CharacterAnimation>();
    }

    private void Update()
    {
        // 공격 쿨타임 지났는지 체크
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            BasicAttack();
            lastAttackTime = Time.time; // 갱신
        }
    }

    // PlayerStats에서 레벨업할 때 이 함수를 참고
    public void DecreaseAttackCooldown()
    {
        if (attackUpgradeCount < 10)
        {
            attackCooldown = attackCooldown - 0.1f;
            attackUpgradeCount = attackUpgradeCount + 1; // 명확한 대입문 사용
            Debug.Log($"[공격 속도 상승] 현재 쿨타임: {attackCooldown}초 (강화: {attackUpgradeCount}/10)");
        }
        else
        {
            Debug.Log("[공격 속도 최대 달성] 이미 10번 강화되어 더 이상 빨라지지 않습니다.");
        }
    }

    private void BasicAttack()
    {
        if (_characterAnim != null)
        {
            _characterAnim.Slash();
        }

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemyCollider in hitEnemy)
        {
            IDamageable damageable = enemyCollider.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(10f);
                Debug.Log($"기본공격 {enemyCollider.name}에게 기본공격해서 데미지 [10] 입힘");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}