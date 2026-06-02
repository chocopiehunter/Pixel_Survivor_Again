using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance {  get; private set; }

    [Header("Pool Setting")]
    // 몬스터 프리팹을 넣을곳
    [SerializeField] private GameObject enemyPrefab;
    // 미리 만들어둘 eneymy 갯수
    [SerializeField] private int poolSize = 50;

    // 생성된 옵젝들을 담아둘 리스트
    private List<GameObject> pool = new List<GameObject>();

    private void Awake()
    {
        // 싱글턴 초기화
        Instance = this;

        InitPool();
    }

    private void InitPool()
    {
        int i = 0;
        while (i < poolSize)
        {
            // 몹 생성
            GameObject obj = Instantiate(enemyPrefab, transform);

            // 비활성화
            obj.SetActive(false);

            // 리스트에 넣어두기
            pool.Add(obj);

            i = i + 1;
        }
    }

    // 몹 소환할때 호출할 함수
    public GameObject GetEnemy()
    {
        // 리스트에 비활성화된 일 안하고있는 Enemy가 있는지 찾기
        foreach (GameObject obj in pool)
        {
            if(obj.activeSelf == false)
            {
                // 찾으면 활성화 시키고 리턴
                obj.SetActive(true);

                return obj;
            }
        }

        // 리스트의 50마리가 모두 씬에 불려져서 일을 하고있다면 새로 Enemy 추가
        GameObject newObj = Instantiate(enemyPrefab, transform);
        newObj.SetActive(true);
        pool.Add(newObj);

        return newObj;
    }
}
