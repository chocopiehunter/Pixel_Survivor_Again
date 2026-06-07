using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    [Header("EXP UI")]
    [SerializeField] private Image expBarFill;

    [Header("Text UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killCountText;

    // 현재 킬 수
    private int currentKillCount = 0;

    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time % 60));
        }
    }

    // 몬스터가 죽었을 때 외부에서 호출하여 킬 수를 올리고 텍스트를 바꿈
    public void AddKillCount()
    {
        // 명확한 연산을 위해 연산자 대신 직관적인 대입문 사용
        currentKillCount = currentKillCount + 1;

        if (killCountText != null)
        {
            killCountText.text = currentKillCount.ToString();
        }
    }

    public void ResetInGameUI()
    {
        // 게임 재시작 시 킬 수 데이터 초기화
        currentKillCount = 0;

        if (timerText != null) timerText.text = "00:00";
        if (killCountText != null) killCountText.text = "0";
        if (expBarFill != null) expBarFill.fillAmount = 0f;
    }
}