using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] Enemies;
    [SerializeField] private int enemiesAmount;
    [SerializeField] private int maxSpawnAttempts;
    [SerializeField] private int spawnOffset;

    private Transform enemiesParent;
    private Transform enemy;
    private Map map;

    private void Start()
    {
        enemiesParent = new GameObject("Enemies").transform;
        map = new Map(spawnOffset);
    }

    public void Spawn()
    {
        for (int i = 0; i < enemiesAmount; i++)
        {
            enemy = Enemies[Random.Range(0, Enemies.Length)].transform;
            enemy = Instantiate(enemy, FindPosition(), Quaternion.identity, enemiesParent);
            Entity.New<Enemy>(enemy);
        }
    }

    private Vector3 FindPosition()
    {
        Vector3 newSpawn = Vector3.zero;
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            Vector3Int position = map.Random();
            if (IsPosResponds(position))
            {
                newSpawn = position;
                break;
            }
        }

        return newSpawn;
    }

    private bool IsPosResponds(Vector3Int pos)
    {
        if (Level.Instance.GroundTiles.HasTile(pos) &&
            !Level.Instance.WallsTiles.HasTile(pos) &&
            Vector3.Distance(Level.Player.Position, pos) > enemy.GetComponent<CircleCollider2D>().radius + 1)
        {
            return true;
        }

        return false;
    }
}

class Map
{
    public Vector3 Max { get; private set; }
    public Vector3 Min { get; private set; }
   
    public Map(int offset)
    {
        Max = Level.Instance.GroundTiles.size - Vector3.one * offset;
        Min = Vector3.zero - Max / 2;
        Max /= 2;
    }

    public Vector3Int Random()
    {
        int x = Mathf.FloorToInt(UnityEngine.Random.Range(Min.x, Max.x));
        int y = Mathf.FloorToInt(UnityEngine.Random.Range(Min.y, Max.x));
        return new Vector3Int(x, y, 0);
    }
}