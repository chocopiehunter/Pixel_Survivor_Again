using UnityEngine;

public class NPCController : MonoBehaviour
{
    [Header("NPC Info")]
    [SerializeField] private int npcID;
    [SerializeField] private string npcName;

    [Header("연결할 컴포넌트")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    public void InitNPC(int id, string name, Sprite changedSprite)
    {
        npcID = id;
        npcName = name;

        // 스프라이트 교체
        spriteRenderer.sprite = changedSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{npcName}과 부딪힘. 대화창을 켭니다");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"{npcName}과 멀어져서 대화창을 닫습니다");
        }
    }
}
