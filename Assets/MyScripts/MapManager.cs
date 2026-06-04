using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Inst {  get; private set; }

    [Header("Map Setting")]
    // 맵 넣는곳
    [SerializeField] private GameObject mapPrefab;
    // 맵의 크기(타일 수 20x20으로 만듬)
    [SerializeField] private float mapSize = 20.0f;

    // 플레이어의 위치
    private Transform playerPosition;
    // 생성된 3x3의 맵 타일을 저장해둘 배열
    private GameObject[] loadedTiles = new GameObject[9];

    private void Awake()
    {
        Inst = this;
    }

    private void Start()
    {
        // Player태그로 플레이어 위치 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerPosition = player.transform;
        }

        // 게임 시작할때 플레이어 주변에 3x3 맵 생성
        InitMap();
    }

    private void InitMap()
    {
        int index = 0;

        for (int x = -1; x <= 1; x = x + 1)
        {
            for (int y = -1; y <= 1; y = y + 1)
            {
                Vector3 spawnPosition = new Vector3(x * mapSize, y * mapSize, 0);

                GameObject tile = Instantiate(mapPrefab, spawnPosition, Quaternion.identity, transform);
                tile.name = "Tile_" + x + "_" + y;

                loadedTiles[index] = tile;
                index = index + 1;
            }
        }
    }
}
