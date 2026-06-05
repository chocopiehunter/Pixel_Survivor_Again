using UnityEngine;

public class RobbyUI : DaniTechUIBase
{
    [SerializeField] private DaniTechUIButton Button_NewGameStart;
    [SerializeField] private DaniTechUIButton Button_GameQuit;
    [SerializeField] private DaniTechUIButton Button_Continue;

    private void OnEnable()
    {
        Button_NewGameStart.BindOnClickButtonEvent(OnClick_NewGameStart);
        Button_GameQuit.BindOnClickButtonEvent(OnClick_GameQuit);
    }

    public void OnClick_NewGameStart()
    {
        DaniTechUIManager.Instance.CloseContentUI(DaniTechUIType.RobbyUI);

        // 내 게임매니저에서 StartGame 호출
        if(MyGameManager.Instance != null)
        {
            MyGameManager.Instance.StartGame();
        }
    }

    public void OnClick_GameQuit()
    {
        DaniTechGameManager.Inst.SaveAndEndGame();
    }
}
