using UnityEngine;

public class DungeonExit : MonoBehaviour
{
    [Header("Teleport Destination")]
    // 돌아갈 마을의 목적지 좌표 (기본값은 0, 0 이지만 인스펙터에서 수정 가능)
    [SerializeField] private Vector2 villageTargetPosition = new Vector2(0f, 0f);

    [Header("Camera Control")]
    private Transform mainCameraTransform;

    private void Start()
    {
        // 메인 카메라 자동으로 찾아오기
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // 던전 출구 트리거에 플레이어가 부딪혔을 때
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 유저님 코드 방식 그대로 Player 태그 확인
        if (collision.CompareTag("Player"))
        {
            // 플레이어를 마을 좌표로 순간이동
            TeleportPlayer(collision.transform);

            // 카메라도 마을 좌표로 즉시 이동
            TeleportCamera();

            Debug.Log("플레이어가 던전 출구에 닿아 마을로 귀환함");
        }
    }

    // 플레이어를 마을 좌표로 이동
    private void TeleportPlayer(Transform playerTransform)
    {
        playerTransform.position = new Vector3(villageTargetPosition.x, villageTargetPosition.y, playerTransform.position.z);
        Debug.Log("플레이어가 마을 좌표로 이동 완료");
    }

    // 카메라를 마을 좌표로 즉시 이동
    private void TeleportCamera()
    {
        if (mainCameraTransform != null)
        {
            float originalCameraZ = mainCameraTransform.position.z;
            mainCameraTransform.position = new Vector3(villageTargetPosition.x, villageTargetPosition.y, originalCameraZ);
        }
    }
}