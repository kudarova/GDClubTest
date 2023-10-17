using System.Collections;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [Range(1, 1000)]
    [SerializeField] private int damage;
    [field: Range(.01f, 100)]
    [field: SerializeField] protected float Speed { get; private set; }

    protected float lifeTime { get; private set; } = 5;
    protected float currentLifeTime { get; private set; }

    protected float dynamicSpeed;

    private Transform target;
    protected Vector3 targetPos { get { return target.position; } }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void Set(Vector3 shootPos, Transform _target)
    {
        transform.position = shootPos;
        target = _target;

        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        currentLifeTime = 0;
        StartCoroutine(Flight());
    }

    private void OnDisable()
    {
        StopCoroutine(Flight());
    }

    private IEnumerator Flight()
    {
        Vector3 targetPos = target.position;
        float targetLength = target.localScale.x; 

        while (target != null && Vector3.Distance(transform.position, targetPos) > targetLength)
        {
            // индивидуальное поведение
            BulletBehaviour();
             
            // Увеличиваем текущее время жизни пули
            currentLifeTime += Time.deltaTime;

            // Если пуля прожила свое время, выключаем еe
            if (currentLifeTime >= lifeTime) gameObject.SetActive(false);

            yield return new WaitForEndOfFrame();
        }

        if (target != null) target.gameObject.GetComponent<DataManager>().Entity.TakeDamage(damage);
        gameObject.SetActive(false);
    }

    // поведение, которое можно перезаписать для различных видов пуль: пускать пулю по синусоиде к примеру... 
    protected abstract void BulletBehaviour();
}