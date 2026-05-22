using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DaniTech_SkillProjectile : DaniTech_SkillBase
{
    [SerializeField] private SpriteRenderer SpriteRenderer_Effect;
    [SerializeField] private float ProjectileSpeed = 5.0f;

    private int _damage;
    private int _ownerInstanceId; // 나를 소환한 주인의 Id

    // 탑뷰, 아이소메트릭이면 y연산도 추가 될 예정
    private Vector3 _moveDirection = new Vector3(1, 0, 0); // 사이드뷰 기준으로는 x가 -1,1 좌우 구분

    public void InitSkillObject(int ownerInstanceId, bool isDirRight, Vector3 playerPos, int damage, string parentTag)
    {
        this.transform.position = playerPos;

        // 사이드뷰 기준 x값만 좌 우 1 또는 -1로 지정됨
        _moveDirection = isDirRight ? new Vector3(1, 0, 0) : new Vector3(-1,0,0);
        SpriteRenderer_Effect.flipX = !isDirRight;
        SpriteRenderer_Effect.flipY = !isDirRight;

        _damage = damage;
        _ownerInstanceId = ownerInstanceId;

        // 소환자의 Tag정보를 기입한다
        this.gameObject.tag = parentTag;
    }

    private void Update()
    {
        transform.position += _moveDirection * ProjectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckCollision(collision.collider);
    }

    private void CheckCollision(Collider2D collision)
    {
        // Owner가 0번이면 무조건 플레이어다
        bool isOwnerPlayer = (_ownerInstanceId == 0);

        // 투사체가 충돌한 오브젝트의 Tag가 플레이어라면?
        if (collision.CompareTag("Player") == (isOwnerPlayer == false)) 
        {
            // 플레이어라면 직접 플레이어에게 투사체가 데미지를 부여해봅시다
             var player = DaniTechGameObjectManager.Inst.GetLocalPlayer();
            player.TakeDamage(_damage);

            // 스킬은 오브젝트 매니저를 통해서 만들어지지는 않았으므로 직접 스스로 제거해봅시다
            // 몬스터 -> 오브젝트 매니저를 통해서 제거 (UI매니저와 동일한 프로세스)
            // 스킬은 직접 스스로 제거
            Destroy(this.gameObject);
        }
    }
}
