using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    // я бы сделал какой-нибудь статический класс наподобие UI, где держал бы эти и не только кнопки
    [SerializeField] private GameObject cellBtnPrefab;
    [SerializeField] private GameObject cellText;
    [SerializeField] private GameObject deleteBtn;
    [SerializeField] private GameObject selectMarker;

    [SerializeField] private float cellOffset;

    private Transform cellsParent;
    private GameObject[] cells;
    private TextMeshProUGUI[] texts;

    private void Start()
    {
        Level.Player.Inventory.SetDisplay(this);
        texts = new TextMeshProUGUI[Level.Player.Inventory.Size];
        cellsParent = transform.GetChild(3);

        DrawCells();
    }
    private Vector3 GetCellPosition(byte id, float width, float heigh)
    {
        int coloumn = id / Level.Player.Inventory.SizeInRaw;
        int raw = id - coloumn * Level.Player.Inventory.SizeInRaw;

        float x = coloumn * width + cellOffset * coloumn;
        float y = raw * heigh + cellOffset * raw;

        return new Vector3(-x, y, 0);
    }
    private void DrawCells()
    {
        cells = new GameObject[Level.Player.Inventory.Size];

        for (int id = 0; id < Level.Player.Inventory.Size; id++)
        {
            GameObject newCell = Instantiate(cellBtnPrefab);
            newCell.name = "Cell " + id;
            newCell.transform.SetParent(cellsParent.transform);
            RectTransform rt = newCell.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
            newCell.transform.localPosition = GetCellPosition((byte)id, rt.rect.width, rt.rect.height);
            cells[id] = newCell;

            byte selectId = (byte)id;
            cells[id].GetComponent<Button>().onClick.AddListener(delegate { Select(selectId); });
        }

        UpdateInventory();
    }
    private void UpdateInventory()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            CellData data = Level.Player.Inventory.GetData(i);
            if (!data.IsEmpty)
            {
                AddNew(i, data.ItemId);
            }
        }
    }

    public void AddNew(int cellId, int itemId)
    {
        Sprite icon = ItemManager.GetItem(itemId).Icon;
        GameObject invItem = new GameObject(icon.name);

        Image img = invItem.AddComponent<Image>();
        img.sprite = icon;

        invItem.transform.position = cells[cellId].transform.position;
        invItem.transform.SetParent(cells[cellId].transform);
        float size = icon.pixelsPerUnit / icon.rect.size.x / 3f;
        img.GetComponent<RectTransform>().localScale = Vector3.one * size;

        UpdateAmount(cellId);
    }
    public void UpdateAmount(int cellId)
    {
        int amount = Level.Player.Inventory.GetData(cellId).Amount;
        if (amount > 1)
        {
            texts[cellId] ??= Instantiate(cellText, cells[cellId].transform.GetChild(0).transform).GetComponent<TextMeshProUGUI>();
            texts[cellId].text = amount.ToString();

            texts[cellId].rectTransform.localScale = Vector3.one * 3;
        }
    }

    private int selectedId = -1;

    public void Select(byte cellId)
    {
        if (cellId == selectedId)
        {
            selectedId = -1;
            Unselect();
            HideDelete();
            return;
        }

        selectedId = cellId;
        selectMarker.transform.position = cells[selectedId].transform.position;
        selectMarker.SetActive(true);
        ShowDelete();
    }
    public void Switch()
    {
        cellsParent.gameObject.SetActive(!cellsParent.gameObject.activeSelf);
        Unselect();
        HideDelete();
    }
    public void Remove()
    {
        if (!Level.Player.Inventory.GetData(selectedId).IsEmpty)
        {
            Level.Player.Inventory.Remove((byte)selectedId);
            texts[selectedId] = null;
            Destroy(cells[selectedId].transform.GetChild(0).gameObject);
            Unselect();
            HideDelete();
            selectedId = -1;
        }
    }
    public void Unselect() => selectMarker.SetActive(false);
    public void ShowDelete() => deleteBtn.SetActive(true);
    public void HideDelete() => deleteBtn.SetActive(false);
}