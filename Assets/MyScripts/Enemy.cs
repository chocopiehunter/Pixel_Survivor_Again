using System.Collections;
using MoreMountains.Feedbacks; // FEEL 에셋
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private string enemyName;
    private float maxHP;
    private float currentHP;
    private float moveSpeed;
    private float knockbackPower;

    [Header("Attack Setting")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private float attackDelay = 1f; // 틱뎀 주기 (1초마다)
    private float lastAttackTime = 0f;

    [Header("FEEL")]
    [SerializeField] private MMF_Player hitFeedback;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // 플레이어가 시체를 통과할수 있게함
    private Collider2D enemyCollider;
    // 죽음 상태 설정으로 다시 죽는것 방지
    private bool isDead = false;

    // 넉백 상태 체크 및 중복 방지용 변수
    private bool isKnockback = false;
    private Coroutine knockbackRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void InitEnemy(EnemyData data)
    {
        if (data == null) return;

        // 다시 태어날때 죽음 상태 풀고 물리/충돌체 부활
        isDead = false;
        isKnockback = false; // 넉백 상태도 초기화
        if (enemyCollider != null) enemyCollider.enabled = true;
        if (rb != null) rb.simulated = true;

        // 데이터 주입
        enemyName = data.enemyName;
        maxHP = data.maxHP;
        currentHP = maxHP; // 체력 리셋
        moveSpeed = data.moveSpeed;
        knockbackPower = data.knockbackPower;

        if (spriteRenderer != null) spriteRenderer.sprite = data.enemySprite;
        if (animator != null) animator.runtimeAnimatorController = data.animatorController;
    }

    private void FixedUpdate()
    {
        // 죽었거나 넉백 중일 때는 플레이어 추적 중지
        if (isDead || isKnockback) return;

        if (playerTransform != null)
        {
            // 플레이어의 방향 계산
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;

            // rigidbody2d 를 사용해 플레이어 방향으로 이동
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            if (direction.x != 0)
            {
                spriteRenderer.flipX = (direction.x < 0);
            }
        }
    }

    // PlayerAttack.cs가 호출할 실제 피격함수
    public void TakeDamage(float damage)
    {
        if (isDead) return; // 이미 죽은 상태일 때 공격받기 방지

        currentHP -= damage;
        Debug.Log($"{enemyName}이 {damage}의 데미지 입음 (남은 체력: {currentHP}/{maxHP})");

        // 살아있을 때 FEEL 효과 적용
        if (hitFeedback != null) hitFeedback.PlayFeedbacks();

        // 현재 체력이 0이하면 사망
        if (currentHP <= 0)
        {
            if (knockbackRoutine != null) StopCoroutine(knockbackRoutine);
            Die();
        }
        else
        {
            // 살아있다면 넉백 실행
            if (knockbackRoutine != null) StopCoroutine(knockbackRoutine);
            knockbackRoutine = StartCoroutine(KnockbackRoutine());
        }
    }

    // 넉백 물리 제어 코루틴
    private IEnumerator KnockbackRoutine()
    {
        if (playerTransform == null) yield break;

        isKnockback = true;

        // 플레이어 반대 방향 계산
        Vector2 knockbackDirection = ((Vector2)transform.position - (Vector2)playerTransform.position).normalized;

        // 플레이어 반대 방향으로 순간 충격 가함
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackDirection * knockbackPower, ForceMode2D.Impulse);

        // 0.12초 동안 밀려남 지연
        yield return new WaitForSeconds(0.12f);

        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    // 사망 처리
    private void Die()
    {
        isDead = true;
        Debug.Log($"{enemyName} 사망");

        if (enemyCollider != null) enemyCollider.enabled = false;
        if (rb != null) rb.simulated = false;

        // Die 애니메이션 재생
        if (animator != null)
        {
            animator.Play("Die");
        }

        // 코루틴으로 Die애니메이션이 재생될 시간동안 잠시 기다렸다가 사라지게 함
        StartCoroutine(DieRoutine(0.5f));
    }

    private IEnumerator DieRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    // 플레이어에게 닿으면 데미지 주는 로직
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // 틱뎀 딜레이 체크
            if (Time.time >= lastAttackTime + attackDelay)
            {
                IDamageable playerDamageable = collision.gameObject.GetComponent<IDamageable>();

                if (playerDamageable != null)
                {
                    playerDamageable.TakeDamage(attackDamage);
                    lastAttackTime = Time.time; // 틱뎀 주기 갱신
                }
            }
        }
    }
}