using UnityEngine;

public class ItemDummy : MonoBehaviour
{
    private float radius = 1.5f;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        circleCollider = gameObject.AddComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;

        radius /= (transform.localScale.x + transform.localScale.y) / 2;
        circleCollider.radius = radius;

        if (Vector3.Distance(Level.Player.Position, transform.position) > radius)
            End();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Level.Player.Tag))
        {
            circleCollider.radius /= 2;
            End();
        }
    }

    private void End()
    {
        tag = Level.ItemManager.ItemTag;
        Destroy(this);
    }
}