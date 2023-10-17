using UnityEngine;

[System.Serializable]
public class InventoryCell
{
    public byte Id { get; private set; }
    public CellData CellData { get; private set; }

    public InventoryCell()
    {
        Id = Inventory.LastCellId;
        CellData = new CellData();
    }

    public void AddTo(Item item)
    {
        if (CellData.Remains != 0)
        {
            int newItemAmount = Mathf.Clamp(item.Amount - CellData.Remains, 0, item.Amount);
            int toAddAmount = CellData.Amount + (item.Amount - newItemAmount);
            CellData.AddAmount(toAddAmount);
            item.Amount = newItemAmount;
        }
    }
    public void AddNew(Item item) => CellData.AddNew(item.Id, item.Amount);
    public void Remove() => CellData.Remove();
}

[System.Serializable]
public class CellData
{
    public byte Id { get; private set; }
    public bool IsEmpty { get; private set; }
    public byte ItemId { get; private set; }
    public int Amount { get; private set; }
    public int Remains { get; private set; }

    public CellData()
    {
        Id = Inventory.LastCellId;

        IsEmpty = true;
        ItemId = 0;
        Amount = 0;
        Remains = 0;
    }

    public int CountRemains() => Remains = ItemManager.GetItem(ItemId).AmountInStack - Amount;
    public void AddAmount(int amount)
    {
        Amount = amount;
        CountRemains();
    }
    public void AddNew(byte itemId, int amount)
    {
        IsEmpty = false;
        ItemId = itemId;
        Amount = amount;
        CountRemains();
    }
    public void Remove() => IsEmpty = true;
}