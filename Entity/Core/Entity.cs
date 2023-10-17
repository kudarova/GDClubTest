using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// под конец пон€л, что был бы неплохо реализовать в ентити инстанцию объекта, экземпл€р,
// чтобы можно было легко манипулировать информацией пр€мо отсюда

[System.Serializable]
public abstract class Entity : Data
{
    public abstract string Name { get; }

    private int maxHealth;
    private int health;
    private float speed;
    private int level;

    #region Properies

    public int MaxHealth { get { return maxHealth; } protected set { maxHealth = Mathf.Clamp(value, 0, 100000); } }
    public int Health { get { return health; } protected set { health = Mathf.Clamp(value, 0, MaxHealth); } }
    public float Speed { get { return speed; } protected set { speed = Mathf.Clamp(value, 0, 25); } }
    public int ExpLevel { get { return level; } private set { level = Mathf.Clamp(value, 0, 100); } }

    [field: System.NonSerialized] public Transform MainTransform { get; protected set; }
    [field: System.NonSerialized] public Transform ObjectTransform { get; protected set; }
    public Vector3 Position { get { return MainTransform.position; } protected set { MainTransform.position = value; } }
    [field: System.NonSerialized] protected bool facingLeft { get; private set; } = false;

    [field: System.NonSerialized] protected Statusbar Healthbar { private get; set; }
    [System.NonSerialized] private Animator animator;


    public string Tag { get { return MainTransform.tag; } }
    public string EnemyTag { get; protected set; }

    #endregion

    #region New-Load

    /// <summary>
    /// LocalLoad Entity 
    /// </summary>
    public static void Load<T>(out T dat, Transform transform) where T : Entity, new()
    {
        if (!TryLoad(out dat)) Save(out dat);
        dat.Set(transform);
    }
    public static T New<T>(Transform transform) where T : Entity, new()
    {
        T entity = new();
        entity.Set(transform);
        return entity;
    }
    protected void Set(Transform transform)
    {
        if (MainTransform is not null) return;
        MainTransform = transform;

        ObjectTransform = MainTransform.GetChild(0); // or transform.Find();
        ObjectTransform.gameObject.AddComponent<DataManager>().Entity = this;
        //ObjectTransform.TryGetComponent(out animator);
        UpdateSprites();

        LateLoad();
        LinkToUpdate(this);
    }

    #endregion

    #region Main

    public virtual void TakeDamage(int damage) 
    {
        Health -= damage;
        Healthbar.Value = Health;
        if (Health == 0) Death();
    } 
    
    protected virtual void Death()
    {
        UnlinkToUpdate(this);
        DataRemove();

        DeathBehavior();
        OnDeath();
    }
    protected virtual void DeathBehavior() // проиграть какую-то особенную анимацию, выкинуть какой-то предмет и тд
    {
        Debug.Log(Name + " dying");
    }
    protected virtual void OnDeath() // or "AfterDeath", у player - respawn (load) к примеру, у значимого персонажа - recover, у единичного enemy - destroy
    {
        Debug.Log(Name + " died");
        GameObject.Destroy(MainTransform.gameObject);
    }

    protected void UpdateExperienceLevel(int exp)
    {
        ExpLevel = exp / 100;
    }
    protected virtual void UpdateStatsByLevel() { /* ... */ } // у player может быть например еще один статусбар дл€ опыта

    // Flip, mb i should pass it to another script
    [System.NonSerialized] private float lastXPos = 0;
    protected void CheckFlip()
    {
        if (!facingLeft && Position.x + .01f < lastXPos)
        {
            Flip();
        }
        else if (facingLeft && Position.x - .01f > lastXPos)
        {
            Flip();
        }

        lastXPos = Position.x;
    }
    protected void CheckFlip(float input)
    {
        if (facingLeft && input < 0)
        {
            Flip();
        }
        else if (!facingLeft && input > 0)
        {
            Flip();
        }
    }
    private void Flip()
    {
        facingLeft = !facingLeft;
        Vector3 Scaler = ObjectTransform.localScale;
        Scaler.x *= -1;
        ObjectTransform.localScale = Scaler;
    }

    // SortingOrder, mb i should pass it to another script
    [System.NonSerialized] private int lastYPos = 0;
    [System.NonSerialized] private List<SpriteRenderer> entitySprites;
    protected int GetSortingPos() => Mathf.FloorToInt(ObjectTransform.transform.position.y - ObjectTransform.transform.localScale.y / 3);
    protected void UpdateSprites()
    {
        entitySprites = ObjectTransform.GetComponentsInChildren<SpriteRenderer>().ToList();
    }
    protected void RemoveSprite(SpriteRenderer sr)
    {
        if (sr is null) return;
        entitySprites.Remove(sr);
    }
    protected void AddSprite(SpriteRenderer sr)
    {
        if (sr is null) return;
        entitySprites.Add(sr);
    }
    protected void UpdateSortingOrder()
    {
        int newPos = GetSortingPos();
        if (newPos != lastYPos)
        {
            int diff = lastYPos - newPos;
            for (int i = 0; i < entitySprites.Count; i++)
            {
                entitySprites[i].sortingOrder += diff;
            }

            lastYPos = newPos;
        }
    }

    #endregion

}