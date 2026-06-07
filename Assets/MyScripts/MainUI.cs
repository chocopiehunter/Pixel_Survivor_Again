using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    [Header("EXP UI")]
    [SerializeField] private Slider expSlider; // Image에서 Slider로 변경
    [SerializeField] private TMP_Text levelText;

    [Header("Text UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text killCountText;

    private int currentKillCount = 0;

    // PlayerStats에서 이 함수를 호출해 경험치 데이터를 넘겨줍니다.
    public void UpdateExpBar(float currentExp, float maxExp, int currentLevel)
    {
        if (expSlider != null)
        {
            // 슬라이더의 value(0 ~ 1)에 [현재 경험치 / 최대 경험치] 비율을 대입합니다.
            expSlider.value = currentExp / maxExp;
        }

        if (levelText != null)
        {
            levelText.text = "LV. " + currentLevel;
        }
    }

    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            timerText.text = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(time / 60), Mathf.FloorToInt(time % 60));
        }
    }

    public void AddKillCount()
    {
        currentKillCount = currentKillCount + 1;

        if (killCountText != null)
        {
            killCountText.text = currentKillCount.ToString();
        }
    }

    public void ResetInGameUI()
    {
        currentKillCount = 0;

        if (timerText != null) timerText.text = "00:00";
        if (killCountText != null) killCountText.text = "0";
        if (expSlider != null) expSlider.value = 0f; // ★ 리셋 시 슬라이더 바닥으로 초기화
        if (levelText != null) levelText.text = "LV. 1";
    }
}