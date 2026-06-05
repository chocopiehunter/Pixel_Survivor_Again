using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player HP")]
    [SerializeField] private float maxHP = 100f;
    private float currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"플레이어 공격받음 {damage}의 데미지 입음 (현재 HP: {currentHP} / {maxHP})");

        // 현재 체력 0이하 되면 사망
        if ( currentHP <= 0)
        {
            PlayerDie();
        }
    }
    
    // 사망
    private void PlayerDie()
    {
        Debug.Log("플레이어 사망");

        Time.timeScale = 0f;
    }
}
