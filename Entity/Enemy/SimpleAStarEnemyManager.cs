using UnityEngine;

class SimpleAStarEnemyManager : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            GetComponent<Pathfinding.AIPath>().canSearch = true;
            Destroy(GetComponent<CircleCollider2D>());
            Destroy(this);
        }
    }
}