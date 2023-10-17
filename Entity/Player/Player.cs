using System.Reflection;
using UnityEngine;

[System.Serializable]
public sealed class Player : WeaponOwner
{
    #region Updatable

    protected override void Update()
    {
        //moveInput = joystick.Direction;
        //moveVelocity = moveInput.normalized * ExpLevel.Player.Speed;

        UpdateSortingOrder();
        CheckFlip();
    }
    protected override void FixedUpdate()
    {
        //rigid.MovePosition(rigid.targetPos + moveVelocity * Time.fixedDeltaTime);
    }

    #endregion

    #region Data

    public Player() 
    {
        MaxHealth = 100;
        Health = MaxHealth;
        Speed = 5;
        bullets = Random.Range(15, 60);
        Experience = 0;
        Inventory = new Inventory();
        EnemyTag = "Enemy";
    }
    protected override void LateLoad()
    {
        Healthbar = Statusbar.New(Color.red, Color.green, MaxHealth, Health, MainTransform);

        UI.Instance.UpdateBulletAmount();
        UpdateExperienceLevel(Experience);

        ObjectTransform.tag = "Player";

        CollectWeapon(weaponId);

        props = typeof(Player).GetProperties();
    }

    #endregion

    #region Entity

    public override string Name { get; } = "Player";

    protected override void Death()
    {
        DeathBehavior();
        OnDeath();
    }
    protected override void DeathBehavior()
    {
        Level.ItemManager.DropInventory(Inventory);
    }
    protected override void OnDeath()
    {
        Level.ReloadLevel();
    }

    #endregion

    #region Player

    private int bullets = 0;

    public int Experience { get; private set; }
    public int Bullets { get { return bullets; } private set { bullets = value; UI.Instance.UpdateBulletAmount(); } }

    public Inventory Inventory { get; private set; }
    [System.NonSerialized] private PropertyInfo[] props;

    public void CollectItem(Item item)
    {
        ItemData itemData = ItemManager.GetItem(item.Id);
        switch (itemData.Type)
        {
            case ItemType.Item:
                if (!Inventory.TryAdd(item)) return;
                break;
            case ItemType.Resource:
                CountResource(item);
                break;
            case ItemType.Weapon:
                if (!CollectWeapon(item.Id)) return;
                break;
            case ItemType.Inventory:
                Debug.Log("Inventory found");
                Debug.Log("Collect inventory items");
                break;
        }

        GameObject.Destroy(item.gameObject);
    }
    public void CountResource(Item itemInfo)
    {
        // or through switch-case
        for (int i = 0; i < props.Length; i++)
        {
            if (props[i].Name != ItemManager.GetItem(itemInfo.Id).Icon.name) continue;

            object propValue = Mathf.Clamp((int)props[i].GetValue(this) + itemInfo.Amount, 0, int.MaxValue);
            props[i].SetValue(this, propValue);
            return;
        }
    }

    #endregion

    #region WeaponOwner

    protected override void ShootTerm()
    {
        if (Bullets == 0) return;
        if (weapon.Shoot(ShootManager.Target))
            Bullets--;
    }

    #endregion

}