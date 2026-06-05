using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 3.0f;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // 플레이어에게 닿았는지 체크해보는 테스트 코드
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("몬스터가 캐릭터를 공격했음");
        }
    }
}
