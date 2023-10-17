
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public static UI Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI bulletsAmount;
    [SerializeField] private Button shootBtn;
    [SerializeField] private Button debugBtn;
    [SerializeField] private Button exitBtn;
    private bool isButtonHeld = false;

    private void Awake()
    {
        if (Instance != null) return;
        Instance = this;
    }

    private void Start()
    {
        shootBtn.onClick.AddListener(delegate { Shoot(); });
        debugBtn.onClick.AddListener(delegate { Debug(); });
        exitBtn.onClick.AddListener(delegate { Exit(); });

        // добавьте этот скрипт как слушатель событий для вашей кнопки Shoot - бот написал
        EventTrigger trigger = shootBtn.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(pointerUpEntry);
    }


    private void OnPointerDown(PointerEventData eventData)
    {
        isButtonHeld = true;
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        isButtonHeld = false;
    }

    private void Shoot()
    {
        Level.Player.Shoot();
    }
    private void Debug()
    {
        Level.Player.TakeDamage(1);
        Level.ItemManager.Drop(Level.Player.MainTransform.position);
    }
    private void Exit()
    {
        Data.SaveAll();
        Application.Quit();
    }

    private void Update()
    {
        if (isButtonHeld) Shoot();
    }

    public void UpdateBulletAmount() => bulletsAmount.text = "x" + Level.Player.Bullets; 
}