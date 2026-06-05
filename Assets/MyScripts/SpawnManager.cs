using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Inst { get; private set; }

    [Header("Spawn Setting")]
    // 스폰 주기
    [SerializeField] private float spawnDelay = 1.0f;

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
            enemy.SetActive(true);
        }
    }
}
