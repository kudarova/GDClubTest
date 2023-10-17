using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

// level - сборная солянка, реализовал бы по-другому, но весь акцент был сделан на save-system, data, и inventory
public class Level : MonoBehaviour
{
    public static Level Instance;

    [field: SerializeField] public GameObject Back { get; private set; }
    [field: SerializeField] public GameObject HealthbarPrefab { get; private set; }

    // tilemaps определил бы в другой скрипт, но не было времени
    [field: SerializeField] public Tilemap GroundTiles { get; private set; }
    [field: SerializeField] public Tilemap WallsTiles { get; private set; }

    private static Player player;
    public static Player Player { get { return player; } }

    public static ItemManager ItemManager { get; private set; }
    public static EnemySpawner EnemySpawner { get; private set; }

    private void Awake()
    {
        Instance = this;
        ItemManager = GetComponent<ItemManager>();
        ItemManager.Init();

        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Entity.Load(out player, playerTransform);
    }

    private void Start()
    {
        EnemySpawner = GetComponent<EnemySpawner>();
        EnemySpawner.Spawn();
    }

    private void Update()
    {
        Updatable.Tick();
        Back.transform.position = new Vector3(Player.MainTransform.position.x * .8f, Player.MainTransform.position.y * .8f);
    }

    public static void ReloadLevel() 
    {
        Updatable.Reset();
        SceneManager.LoadScene(0);
    }
    
}