using UnityEngine;
using MoreMountains.Feedbacks;

public class PlayerStats : MonoBehaviour
{
    // 어디서나 플레이어 정보에 접근할 수 있도록 싱글톤 처리
    public static PlayerStats Inst { get; private set; }

    [Header("Player Level & EXP")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private float currentEXP = 0f;
    [SerializeField] private float maxEXP = 100f; // 1레벨 기본 필요 경험치량

    [Header("FEEL 레벨업")]
    [SerializeField] private MMF_Player levelUpFeedback;

    private void Awake()
    {
        if (Inst == null) Inst = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 게임 시작할 때 UI 화면 갱신
        UpdateExpUI();
    }

    // 몬스터가 죽을 때 호출해서 경험치를 넣어줄 함수
    public void AddExp(float amount)
    {
        currentEXP = currentEXP + amount;
        Debug.Log($"경험치 획득: +{amount} (현재: {currentEXP}/{maxEXP})");

        // 경험치가 가득 찼다면 레벨업 처리
        while (currentEXP >= maxEXP)
        {
            LevelUp();
        }

        // 최종적으로 바뀐 경험치를 UI에 반영
        UpdateExpUI();
    }

    private void LevelUp()
    {
        currentEXP = currentEXP - maxEXP;
        currentLevel = currentLevel + 1;
        maxEXP = maxEXP * 1.2f;

        Debug.Log($"LEVEL UP! 현재 레벨: {currentLevel}");

        // 체력 스크립트를 찾아 회복
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.RestoreFullHP();
        }

        // 공격 쿨타임 감소
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (playerAttack != null)
        {
            playerAttack.DecreaseAttackCooldown();
        }

        // FEEL 레벨 업 이펙트 재생
        if (levelUpFeedback != null)
        {
            levelUpFeedback.PlayFeedbacks();
        }
    }

    private void UpdateExpUI()
    {
        // MyUIManager를 통해 MainUI에 경험치바와 레벨 정보 전달
        if (MyUIManager.Inst != null && MyUIManager.Inst.GetMainUI() != null)
        {
            MyUIManager.Inst.GetMainUI().UpdateExpBar(currentEXP, maxEXP, currentLevel);
        }
    }
}