using UnityEngine;

public class DataManager : MonoBehaviour
{
    private Entity entity;
    public Entity Entity { get { return entity; } set { entity ??= value; } }
}