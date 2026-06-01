using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    [Header("Teleport Destination")]
    // 던전의 목적지 좌표 (10000f, 10000f)
    [SerializeField] private Vector2 dungeonTargetPosition = new Vector2(10000f, 10000f);

    [Header("Camera Control")]
    // 메인 카메라의 트랜스폼
    private Transform mainCameraTransform;

    private void Start()
    {
        // 게임 시작 시 태그를 통해 메인 카메라를 자동으로 찾아옵니다.
        if (Camera.main != null)
        {
            mainCameraTransform = Camera.main.transform;
        }
    }

    // Trigger 체크인 오브젝트에 무언가 부딪혔을 때 유니티가 자동으로 실행하는 메서드
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Player인지 확인
        if (collision.CompareTag("Player"))
        {
            // 플레이어 순간이동 수행
            TeleportPlayer(collision.transform);

            // 카메라 순간이동 수행
            TeleportCamera();

            Debug.Log("플레이어가 던전 입구에 닿았다");
        }
    }

    // 플레이어를 던전 좌표로 이동
    private void TeleportPlayer(Transform playerTransform)
    {
        // 플레이어의 위치를 지정된 던전 좌표로 직접 대입
        playerTransform.position = new Vector3(dungeonTargetPosition.x, dungeonTargetPosition.y, playerTransform.position.z);

        Debug.Log("플레이어가 던전 좌표로 이동함");
    }

    // 카메라를 던전 좌표로 즉시 이동시키는 메서드
    private void TeleportCamera()
    {
        if (mainCameraTransform != null)
        {
            // 카메라는 2D 게임에서 보통 Z값이 -10 정도로 유지되어야 화면이 보입니다.
            // 따라서 X, Y는 던전 좌표로 바꾸되, 원래 카메라가 가 지고 있던 Z값은 그대로 유지해 줍니다.
            float originalCameraZ = mainCameraTransform.position.z;

            mainCameraTransform.position = new Vector3(dungeonTargetPosition.x, dungeonTargetPosition.y, originalCameraZ);

            // 만약 Cinemachine(시네머신) 카메라 패키지를 사용 중이시라면, 
            // 순간이동 직후 카메라가 부드럽게 쫓아오느라 딜레이가 생길 수 있습니다.
            // 그럴 때는 시네머신 가상 카메라 컴포넌트를 가져와서 .OnTargetObjectWarped()를 호출해야 하지만,
            // 직접 만든 카메라 추적 스크립트라면 위의 포지션 대입만으로도 깔끔하게 순간이동합니다.
        }
    }
}