using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private Transform itemsParent;
    [field: SerializeField] public string ItemTag { get; private set; } = "Item";
    [SerializeField] private ItemData[] items;
    public static ItemData GetItem(int id) => Level.ItemManager.items[id];

    private byte[] lootSpawn;
    private byte[] dropSpawn;

    public void Init()
    {
        itemsParent ??= new GameObject("Drop").transform;
        itemsParent.SetParent(transform);

        List<byte> toLoot = new List<byte>();
        List<byte> toDrop = new List<byte>();

        for (int i = 0; i < items.Length; i++)
        {
            ItemData item = items[i];
            if (item.SpawnAsLoot)
                toLoot.Add((byte)i);
            if (item.SpawnAsDrop)
                toDrop.Add((byte)i);
        }

        lootSpawn = toLoot.ToArray();
        dropSpawn = toDrop.ToArray();
    }

    public void Spawn(byte id, int amount, Vector2 pos)
    {
        ItemData item = items[id];

        GameObject spawnedItem;
        if (item.Prefab != null)
        {
            spawnedItem = Instantiate(item.Prefab, pos, Quaternion.identity, itemsParent);
            spawnedItem.GetComponent<SpriteRenderer>().sortingOrder = SortingOrder.Min + 2;
        }
        else
        {
            spawnedItem = new GameObject(item.Icon.name);
            spawnedItem.transform.SetParent(itemsParent);
            spawnedItem.transform.position = pos;

            SpriteRenderer sr = spawnedItem.AddComponent<SpriteRenderer>();
            sr.sprite = item.Icon;
            sr.sortingOrder = SortingOrder.Min + 2;
        }

        spawnedItem.AddComponent<ItemDummy>();
        if (amount == 0) amount = Random.Range(1, item.AmountInStack / 100 * 80);
        Item.New(spawnedItem, id, amount);
    }

    public void Drop(Vector2 pos)
    {
        byte id = dropSpawn[Random.Range(0, dropSpawn.Length)];
        Spawn(id, items[id].Amount, pos);
    }

    // идея 
    public void DropInventory(Inventory inventory)
    {
        Debug.Log("Inventory spawned");
    }
}

public class Item : MonoBehaviour
{
    private int amount = 0;

    public byte Id { get; private set; }
    public int Amount { get { return amount; } set { amount = Mathf.Clamp(value, 0, ItemManager.GetItem(Id).AmountInStack); } }

    public static Item New(GameObject obj, byte id, int _amount)
    {
        Item item = obj.AddComponent<Item>();
        item.Id = id;
        item.Amount = _amount;

        return item;
    }
}

[System.Serializable]
public struct ItemData
{
    public Sprite Icon;
    public int Amount;
    public int AmountInStack;

    public bool SpawnAsLoot;
    public bool SpawnAsDrop;

    public ItemType Type;

    public GameObject Prefab; // сделал для оружия, ничего лучше не придумал
}

public enum ItemType
{
    Item,
    Resource,
    Weapon,
    Inventory
}