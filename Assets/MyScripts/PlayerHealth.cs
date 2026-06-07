using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Player HP")]
    [SerializeField] private float maxHP = 100f;
    private float currentHP;

    [Header("Player HUD")]
    [SerializeField] private Image hpBarFill;

    [Header("UI Canvas")]
    [SerializeField] private RectTransform hpCanvas;
    [SerializeField] private RectTransform levelUpCanvas;

    private Vector3 _initHpCanvasScale;
    private Vector3 _initLevelUpCanvasScale;

    private void Awake()
    {
        currentHP = maxHP;

        // 최상위 캔버스를 찾아 기본 크기를 기억해둠
        if (hpCanvas != null) _initHpCanvasScale = hpCanvas.localScale;
        if (levelUpCanvas != null) _initLevelUpCanvasScale = levelUpCanvas.localScale;

        UpdateHP();
    }

    // 좌우 이동에 따른 HUD 뒤집힘 해결 로직
    private void LateUpdate()
    {
        // 플레이어의 현재 Flip 상태 1 또는 -1
        float playerScaleX = Mathf.Sign(transform.localScale.x);

        // 체력 HUD 뒤집힘 방지
        if (hpCanvas != null)
        {
            Vector3 scale = hpCanvas.localScale;
            scale.x = playerScaleX * Mathf.Abs(_initHpCanvasScale.x);
            hpCanvas.localScale = scale;
        }

        // 레벨업UI 뒤집힘 방지
        if (levelUpCanvas != null)
        {
            Vector3 scale = levelUpCanvas.localScale;
            scale.x = playerScaleX * Mathf.Abs(_initLevelUpCanvasScale.x);
            levelUpCanvas.localScale = scale;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"플레이어 공격받음 {damage}의 데미지 입음 (현재 HP: {currentHP} / {maxHP})");

        // 데미지를 받을때마다 체력바 갱신
        UpdateHP();

        // 현재 체력 0이하 되면 사망
        if ( currentHP <= 0)
        {
            PlayerDie();
        }
    }

    // 체력 회복
    public void RestoreFullHP()
    {
        currentHP = maxHP;
        UpdateHP();
    }

    private void UpdateHP()
    {
        if (hpBarFill != null)
        {
            hpBarFill.fillAmount = currentHP / maxHP;
        }
    }
    
    // 사망
    private void PlayerDie()
    {
        Debug.Log("플레이어 사망... 게임을 다시 시작합니다");

        // 현재 씬 이름을 가져옴
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 가져온 씬을 다시 시작함
        SceneManager.LoadScene(currentSceneName);

        Time.timeScale = 1.0f;
    }
}
