using MoreMountains.Feedbacks;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private string enemyName = "몬스터";
    [SerializeField] private float maxHP = 20;
    private float currentHP;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // 태어날때 최대체력
        currentHP = maxHP;
    }

    private void OnEnable()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // 플레이어의 방향 계산
            Vector2 direction = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;

            // rigidbody2d 를 사용해 플레이어 방향으로 이동
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // PlayerAttack.cs가 호출할 실제 피격함수
    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"{enemyName}이 {damage}의 데미지 입음 (남은 체력: {currentHP}/{maxHP}");

        // 현재 체력이 0이하면 사망
        if (currentHP <= 0)
        {
            Die();
        }
    }

    // 사망 처리
    private void Die()
    {
        Debug.Log($"{enemyName} 사망");

        gameObject.SetActive(false);
    }

    // 플레이어에게 닿으면 데미지 주는 로직
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable playerDamageable = collision.gameObject.GetComponent<IDamageable>();

            if (playerDamageable != null)
            {
                playerDamageable.TakeDamage(5f);
            }
        }
    }
}
