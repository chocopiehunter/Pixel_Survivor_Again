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

    [Header("UI Canvas (World Space)")]
    [SerializeField] private RectTransform hpCanvas;
    [SerializeField] private RectTransform levelUpCanvas;

    // 플레이어와 UI 사이의 처음 거리(오프셋)를 기억할 변수
    private Vector3 _hpCanvasOffset;
    private Vector3 _levelUpCanvasOffset;

    private void Awake()
    {
        currentHP = maxHP;

        // [핵심 해결책] FEEL 에셋과의 스케일 충돌을 방지하기 위해 부모 관계를 끊습니다.
        // 시작할 때 플레이어와의 상대적 거리를 기억한 뒤, 독립된 월드 오브젝트로 만듭니다.
        if (hpCanvas != null)
        {
            _hpCanvasOffset = hpCanvas.position - transform.position;
            hpCanvas.SetParent(null);
        }

        if (levelUpCanvas != null)
        {
            _levelUpCanvasOffset = levelUpCanvas.position - transform.position;
            levelUpCanvas.SetParent(null);
        }

        UpdateHP();
    }

    // 플레이어가 이동한 후, 독립시킨 UI들이 머리 위 위치를 정확히 쫓아가도록 합니다.
    private void LateUpdate()
    {
        if (hpCanvas != null)
        {
            hpCanvas.position = transform.position + _hpCanvasOffset;
        }

        if (levelUpCanvas != null)
        {
            levelUpCanvas.position = transform.position + _levelUpCanvasOffset;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log($"플레이어 공격받음 {damage}의 데미지 입음 (현재 HP: {currentHP} / {maxHP})");

        // 데미지를 받을때마다 체력바 갱신
        UpdateHP();

        // 현재 체력 0이하 되면 사망
        if (currentHP <= 0)
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