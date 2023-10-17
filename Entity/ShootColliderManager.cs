using UnityEngine;

public class ShootColliderManager : MonoBehaviour
{
    public string EnemyTag { get; private set; }
    public bool CanShoot { get; private set; }
    public Transform Target { get; private set; }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(EnemyTag))
        {
            Target = collider.transform;
            CanShoot = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag(EnemyTag))
        {
            Target = null;
            CanShoot = false;
        }
    }

    public static ShootColliderManager New(Transform obj, string enemyTag, float radius)
    {
        ShootColliderManager scm = obj.gameObject.AddComponent<ShootColliderManager>();
        scm.EnemyTag = enemyTag;

        CircleCollider2D circleCol = obj.gameObject.AddComponent<CircleCollider2D>();
        circleCol.radius = radius;
        circleCol.isTrigger = true;

        return scm;
    }
}