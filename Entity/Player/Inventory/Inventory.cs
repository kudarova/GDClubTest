using UnityEngine;

[System.Serializable]
public class Inventory
{
    public int Size { get; private set; }
    public int SizeInRaw { get; private set; }
    public int Coloumns { get { return Mathf.FloorToInt(Size / SizeInRaw); } }
    public int Fullness { get; private set; }

    public bool isEmpty { get { if (Fullness != 0) return false; else return true; } }
    public bool isFull { get { if (Fullness == Size) return true; else return false; } }

    private InventoryCell[] cells;
    public InventoryCell GetCell(int id) => cells[id];
    public CellData GetData(int id) => cells[id].CellData;
    [field: System.NonSerialized] public static byte LastCellId { get; private set; }

    [System.NonSerialized] private InventoryDisplay display;
    public void SetDisplay(InventoryDisplay invDisplay) => display = invDisplay;

    public Inventory()
    {
        Size = 20;
        SizeInRaw = 10;

        BuildCellData();
        UpdateFullness();
    }

    private void BuildCellData()
    {
        cells = new InventoryCell[Size];
        for (int i = 0; i < Size; i++)
        {
            LastCellId = (byte)i;
            cells[i] = new InventoryCell();
        }
    }
    public void UpdateFullness()
    {
        Fullness = 0;
        for (int i = 0; i < cells.Length; i++)
        {
            if (!cells[i].CellData.IsEmpty)
            {
                Fullness++;
            }
        }
    }

    public bool TryAdd(Item item)
    {
        if (!isEmpty)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (!cells[i].CellData.IsEmpty && cells[i].CellData.ItemId == item.Id)
                {
                    cells[i].AddTo(item);
                    display.UpdateAmount(i);
                    if (item.Amount == 0) return true;
                }
            }
        }

        if (!isFull)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i].CellData.IsEmpty)
                {
                    cells[i].AddNew(item);
                    display.AddNew(cells[i].Id, cells[i].CellData.ItemId);
                    item.Amount = 0;
                    Fullness++;

                    return true;
                }
            }
        }

        return false;
    }
    public void Remove(byte id) 
    {
        Level.ItemManager.Spawn(cells[id].CellData.ItemId, cells[id].CellData.Amount, Level.Player.MainTransform.position);
        cells[id].Remove();
        Fullness--;
    } 
}