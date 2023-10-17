using UnityEngine;

public class MainColliderManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag(Level.ItemManager.ItemTag))
        {
            Level.Player.CollectItem(collider.GetComponent<Item>());
        }
    }
}