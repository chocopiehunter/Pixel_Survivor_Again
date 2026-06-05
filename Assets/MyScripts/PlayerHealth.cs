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

    private Transform _canvasTransform;
    private Vector3 _initCanvasScale;

    private void Awake()
    {
        currentHP = maxHP;

        // 최상위 캔버스를 찾아 기본 크기를 기억해둠
        if(hpBarFill != null && hpBarFill.canvas != null)
        {
            _canvasTransform = hpBarFill.canvas.transform;
            _initCanvasScale = _canvasTransform.localScale;
        }

        UpdateHP();
    }

    // 좌우 이동에 따른 HUD 뒤집힘 해결 로직
    private void LateUpdate()
    {
        if(_canvasTransform != null)
        {
            Vector3 currentScale = _canvasTransform.localScale;

            // 플레이어의 방향 체크
            // 플레이어가 왼쪽을 볼때 -1이 되면 UI도 원래 크기에 -1을 곱해서 방향전환되는걸 막음
            currentScale.x = Mathf.Sign(transform.localScale.x) * Mathf.Abs(_initCanvasScale.x);

            _canvasTransform.localScale = currentScale;
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
