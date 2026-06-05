using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Inst { get; private set; }
    // 연결
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if(Inst  == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TargetFollow(Transform playerTransform)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerTransform;
            Debug.Log("시네머신 카메라가 동적 생성된 플레이어를 추적함");
        }
        else
        {
            Debug.LogError("인스펙터에 카메라 등록 안됨");
        }
    }
}
