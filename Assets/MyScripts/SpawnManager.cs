using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

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
                if (gameTimer < 20f)
                {
                    // 20초 전에는 무조건 0번째 데이터(고블린)만 스폰
                    selectedData = enemyDataGroup[0];
                }
                else if (gameTimer >= 20f && gameTimer < 40f && enemyDataGroup.Length > 1)
                {
                    // 20초~40초 사이에는 0번과 1번 중 랜덤 스폰
                    selectedData = enemyDataGroup[Random.Range(0, 2)];
                }
                else if (gameTimer >= 40f && gameTimer < 60f && enemyDataGroup.Length > 1)
                {
                    // 40초~60초 사이에는 1번과 3번 중 랜덤 스폰
                    selectedData = enemyDataGroup[Random.Range(1, 3)];
                }
                else if (gameTimer >= 60f && gameTimer < 80f && enemyDataGroup.Length > 1)
                {
                    // 60초~80초 사이에는 2번과 4번 중 랜덤 스폰
                    selectedData = enemyDataGroup[Random.Range(2, 4)];
                }
                else if (gameTimer >= 80f && gameTimer < 100f && enemyDataGroup.Length > 1)
                {
                    // 80초~100초 사이에는 전체 데이터 중 랜덤 스폰
                    selectedData = enemyDataGroup[Random.Range(0, enemyDataGroup.Length)];
                }
                else if (gameTimer >= 100f)
                {
                    // 100초 이후에는 배열의 가장 마지막 데이터만 확정 스폰
                    selectedData = enemyDataGroup[enemyDataGroup.Length - 1];
                    spawnDelay = 0.3f;
                }

                // Enemy가 가진 InitEnemy함수를 호출해서 선택된 데이터를 전달
                enemyScript.InitEnemy(selectedData);
            }
            // 스크립터블 오브젝트로 데이터를 다 적용한 뒤에 활성화
            enemy.SetActive(true);

            StartCoroutine(GhostModeRoutine(enemy, 5f)); // Enemy 활성화 직후 5초간 벽 통과 코루틴 실행
        }
    }

    // 소환된 에너미가 5초 동안 특정 레이어만 통과하게 만드는 코루틴
    private IEnumerator GhostModeRoutine(GameObject enemy, float duration)
    {
        Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
        if (enemyCollider == null) yield break;

        // Village_Object와 Dungeon_Wall 레이어를 동시에 가져와서 에너미의 충돌 제외 레이어에 추가
        int ignoreLayerMask = LayerMask.GetMask("Village_Object", "Dungeon_Wall");
        enemyCollider.excludeLayers |= ignoreLayerMask;

        // 5초 동안 대기
        yield return new WaitForSeconds(duration);

        // 5초 뒤 에너미가 살아있다면 해당 레이어들과의 충돌을 다시 복구
        if (enemy != null && enemyCollider != null && enemy.activeSelf)
        {
            enemyCollider.excludeLayers &= ~ignoreLayerMask;
        }
    }
}