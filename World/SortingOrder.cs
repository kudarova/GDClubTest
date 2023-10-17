using UnityEngine;
using UnityEngine.Tilemaps;

public class SortingOrder : MonoBehaviour
{
    [SerializeField] private GameObject environmentObject;
    [SerializeField] private GameObject groundEnvironmentObject;
    [SerializeField] private GameObject tilemapObject;

    public static int Min { get; private set; } = 0;
    public static int Max { get; private set; } = 0;

    private void Start()
    {
        SpriteRenderer[] environment = environmentObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var env in environment)
        {
            env.sortingOrder = Mathf.FloorToInt(-env.transform.position.y - 1);
            if (env.sortingOrder < Min) Min = env.sortingOrder;
            else if (env.sortingOrder > Max) Max = env.sortingOrder;
        }

        TilemapRenderer[] tilemaps = tilemapObject.transform.GetComponentsInChildren<TilemapRenderer>();
        foreach (var tm in tilemaps)
        {
            if (tm.sortingOrder < 1) tm.sortingOrder = 1;
            tm.sortingOrder = -tm.sortingOrder + Min;
            Min = tm.sortingOrder;
        }

        Level.Instance.Back.GetComponent<SpriteRenderer>().sortingOrder = Min - 1;
        SpriteRenderer[] groundEnvironment = groundEnvironmentObject.transform.GetComponentsInChildren<SpriteRenderer>();
        foreach (var grEnv in groundEnvironment)
        {
            grEnv.sortingOrder = Min + 1;
        }
    }
}