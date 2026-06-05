using Cinemachine;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    public static MyGameManager Instance { get; private set; }

    [Header("Player Setting")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        if (playerPrefab != null && playerSpawnPoint != null)
        {
            GameObject playerObj = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            Debug.Log("플레이어 생성완료");

            // 시네머신 타겟 연결
            CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();

            if (vcam != null)
            {
                vcam.Follow = playerObj.transform;
                Debug.Log("카메라가 플레이어 추적 시작");
            }
            else
            {
                Debug.LogError("카메라 오류발생");
            }

            // 자식 찾기
            Transform container = playerObj.transform.Find("SpawnerContainer");

            if (container != null)
            {
                SpawnManager.Inst.SetSpawnContainer(container);
                Debug.Log("성공: SpawnerContainer를 스폰 매니저에 연결했습니다.");
            }
            else
            {
                Debug.LogError("오류: 플레이어 자식에서 'SpawnerContainer'를 찾지 못했습니다. 이름을 다시 확인하세요.");
            }
        }
    }
}
