using UnityEngine;

public class AutoCloseUI : MonoBehaviour
{
    void Update()
    {
        // 화면 어디든 마우스 왼쪽 클릭이 감지되면
        if (Input.GetMouseButtonDown(0))
        {
            // 이 UI 오브젝트를 즉시 삭제(닫기)
            Destroy(gameObject);
        }
    }
}