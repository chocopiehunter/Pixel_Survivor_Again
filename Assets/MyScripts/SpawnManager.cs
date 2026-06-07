using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Inst { get; private set; }

    [Header("Spawn Setting")]
    // 스폰 주기
    [SerializeField] private float spawnDelay = 1.0f;

    [Header("Enemy Data Sheet(에셋 파일들을 넣을 곳)")]
    [SerializeField] private EnemyData[] enemyDataGroup;

    private Transform spawnContainer;
    private float timer;

    // 게임 경과 시간을 측정할 타이머
    private float gameTimer;
    // 게임 시작 버튼을 눌렀는지 판별
    private bool isGameStart = false;

    private void Awake()
    {
        Inst = this;
    }

    // 로비에서 Start 버튼을 눌렀을 때 타이머를 리셋하고 UI를 초기화하기 위해 호출할 함수
    public void StartGame()
    {
        Time.timeScale = 1f;
        Debug.Log("[★ 성공] SpawnManager의 StartGame() 함수가 정상적으로 호출되었습니다!");

        gameTimer = 0f;
        timer = 0f;

        isGameStart = true;

        // MyUIManager를 통해 MainUI의 리셋 함수를 호출
        if (MyUIManager.Inst != null && MyUIManager.Inst.GetMainUI() != null)
        {
            MyUIManager.Inst.GetMainUI().ResetInGameUI();
        }
    }

    public void SetSpawnContainer(Transform container)
    {
        spawnContainer = container;
    }

    private void Update()
    {
        // 스타트버튼 누르기 전엔 시간 누적과 UI 갱신 차단
        if (isGameStart == false) return;

        gameTimer += Time.deltaTime;

        if (MyUIManager.Inst != null && MyUIManager.Inst.GetMainUI() != null)
        {
            MyUIManager.Inst.GetMainUI().UpdateTimerText(gameTimer);
        }

        // 컨테이너가 없을때(로비거나 플레이어가 없을때) 작동 안하게 (아래 스폰 로직만 막음)
        if (spawnContainer == null) return;

        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            // 소환
            SpawnEnemy();
            // 타이머 다시 초기화
            timer = 0;
        }
    }

    private void SpawnEnemy()
    {
        if (spawnContainer == null || spawnContainer.childCount == 0) return;

        // SpawnContainer의 자식들 중 랜덤하게 하나 고름 (나중에 SpawnPoint가 추가되거나 삭제되어도 알아서 작동)
        int randomIndex = Random.Range(0, spawnContainer.childCount);
        Transform selectedPoint = spawnContainer.GetChild(randomIndex);

        // PoolManager에게 풀에 들어있는 몹 요청
        GameObject enemy = PoolManager.Inst.GetEnemy();

        if (enemy != null)
        {
            Vector3 spawnPosition = selectedPoint.position;
            spawnPosition.z = 0;

            enemy.transform.position = spawnPosition;

            // Pool에서 나온 Enemy에 스크립터블 오브젝트 적용
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null && enemyDataGroup.Length > 0)
            {
                // 데이터 목록중 랜덤으로 하나 선택
                int randomDataIndex = Random.Range(0, enemyDataGroup.Length);
                EnemyData selectedData = enemyDataGroup[randomDataIndex];

                // 시간에 따른 등장 몬스터 로직
                if (gameTimer < 30f)
                {
                    // 30초 전에는 무조건 0번째 데이터(고블린)만 스폰
                    selectedData = enemyDataGroup[0];
                }
                else if (gameTimer >= 30f && gameTimer < 60f && enemyDataGroup.Length > 1)
                {
                    // 30초~60초 사이에는 0번과 1번(다크엘프) 중 랜덤 스폰
                    selectedData = enemyDataGroup[Random.Range(0, 2)];
                }
                else if (gameTimer >= 60f)
                {
                    // 60초 이후에는 배열의 가장 마지막 데이터만 확정 스폰
                    selectedData = enemyDataGroup[enemyDataGroup.Length - 1];
                }

                // Enemy가 가진 InitEnemy함수를 호출해서 선택된 데이터를 전달
                enemyScript.InitEnemy(selectedData);
            }
            // 스크립터블 오브젝트로 데이터를 다 적용한 뒤에 활성화
            enemy.SetActive(true);
        }
    }
}