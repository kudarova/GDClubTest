using UnityEngine;
using UnityEngine.UI;

public class Statusbar : MonoBehaviour
{
    private Image fillArea;
    private Color minColor;
    private Color maxColor;
    private int range;

    private Vector2 Position { set { transform.position = value; } }
    private Vector2 Scale { get { return transform.localScale; } set { transform.localScale = value; } }
    public float Value { private get { return fillArea.fillAmount; } set { fillArea.fillAmount = Mathf.Clamp(value, 0, range) / range; NowColor = Value; } }
    private float NowColor { set { fillArea.color = Color.Lerp(minColor, maxColor, value); } }

    public void Switch() => fillArea.gameObject.SetActive(!fillArea.gameObject.activeSelf);

    public static Statusbar New(Color _minColor, Color _maxColor, int _range, float value, Transform transform)
    {
        Statusbar bar = Instantiate(Level.Instance.HealthbarPrefab, transform).GetComponent<Statusbar>();
        bar.fillArea = bar.GetComponentInChildren<Image>();
        bar.Scale = new Vector2(bar.Scale.x, bar.Scale.y + _range / 1000);

        bar.minColor = _minColor;
        bar.maxColor = _maxColor;
        bar.range = _range;
        bar.Value = value;
        bar.Position = new Vector2(transform.position.x, transform.position.y + .1f);

        return bar;
    }
}