using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawn Setting")]
    // 스폰 주기
    [SerializeField] private float spawnDelay = 1.0f;
    // 드래그해서 넣어줄 스폰 배열
    [SerializeField] private Transform[] spawnPoints;

    private float timer;

    private void Update()
    {
        // (ai) 스폰 포인트가 인스펙터에 아예 등록 안되었거나 개수가 0개면 작동하지 않음
        // 이 코드를 아직 이해 못해서 주석처리해둠. 이 코드가 없어도 잘 작동함
        // if (spawnPoints == null || spawnPoints.Length == 0) return;

        // 매 프레임 타이머에 흐른 시간 누적
        timer = timer + Time.deltaTime;

        if (timer >= spawnDelay)
        {
            // 스폰
            SpawnEnemy();
            // 스폰했으니 타이머 리셋
            timer = 0;
        }
    }

    private void SpawnEnemy()
    {
        // 배치해둔 SpawnPoint중 한곳을 랜덤하게 선정
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedPoint = spawnPoints[randomIndex];

        // PoolManager한테 대기중인 몹 요청
        GameObject enemy = PoolManager.Inst.GetEnemy();

        if (enemy != null)
        {
            // 랜덤으로 골라진 스폰 포인트의 현재 위치로 몹을 위치시킴
            enemy.transform.position = selectedPoint.position;
        }
    }
}
