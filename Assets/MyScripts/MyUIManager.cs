using UnityEngine;

public class MyUIManager : MonoBehaviour
{
    public static MyUIManager Inst { get; private set; }

    [Header("UI Reference")]
    [SerializeField] private MainUI mainUI;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 외부 스크립트에서 MainUI 컴포넌트에 접근할 수 있도록 하는 Getter 함수
    public MainUI GetMainUI()
    {
        return mainUI;
    }
}