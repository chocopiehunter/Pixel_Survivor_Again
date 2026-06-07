using UnityEngine;
using UnityEngine.UI;

public class LobbyStartButton : MonoBehaviour
{
    private void Start()
    {
        // 이 스크립트가 붙은 오브젝트의 UI Button 컴포넌트를 가져옴
        Button btn = GetComponent<Button>();

        if (btn != null)
        {
            // 버튼이 클릭되었을 때 실행할 함수를 런타임에 자동으로 등록
            btn.onClick.AddListener(OnConnectStartGame);
        }
    }

    private void OnConnectStartGame()
    {
        // 게임이 시작되어 스폰 매니저가 메모리에 올라와 있다면 자물쇠 해제 함수 호출
        if (SpawnManager.Inst != null)
        {
            SpawnManager.Inst.StartGame();
        }
    }
}