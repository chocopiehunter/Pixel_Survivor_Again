using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZone : MonoBehaviour
{
    // 플레이어의 태그를 "Player"로 설정해야함
    [SerializeField] private string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 태그가 Player인지 확인
        if (collision.CompareTag(playerTag))
        {
            Debug.Log("플레이어가 데드존에 닿음");
            RestartGame();
        }
    }

    private void RestartGame()
    {
        // 현재 활성화된 씬의 인덱스를 가져와서 다시 로드 (처음부터 재시작)
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}