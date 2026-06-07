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

    private void Awake()
    {
        Inst = this;
    }

    public void SetSpawnContainer(Transform container)
    {
        spawnContainer = container;
    }

    private void Update()
    {
        // 컨테이너가 없을때(로비거나 플레이어가 없을때) 작동 안하게
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
        if(spawnContainer == null || spawnContainer.childCount == 0) return;

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

                // Enemy가 가진 InitEnemy함수를 호출해서 선택된 데이터를 전달
                enemyScript.InitEnemy(selectedData);
            }
            // 스크립터블 오브젝트로 데이터를 다 적용한 뒤에 활성화
            enemy.SetActive(true);
        }
    }
}
