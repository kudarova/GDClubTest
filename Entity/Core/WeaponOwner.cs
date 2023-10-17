using UnityEngine;

[System.Serializable]
public abstract class WeaponOwner : Entity // or EntityShooter
{
    protected int? weaponId { get; private set; } = null;
    [field: System.NonSerialized] protected Weapon weapon { get; private set; }

    [field: System.NonSerialized] protected Transform WeaponRig { get; private set; }
    [field: System.NonSerialized] protected ShootColliderManager ShootManager { get; private set; }

    public void Shoot()
    {
        if (weapon is null) return;
        if (!ShootManager.CanShoot) return;
        ShootTerm();
    }
    protected virtual void ShootTerm()
    {
        weapon.Shoot(ShootManager.Target);
    }

    protected bool CollectWeapon(int? _id)
    {
        if (_id is null) return false;

        // drop weapon
        if (weapon != null)
        {
            Level.ItemManager.Spawn((byte)weaponId, 1, Position);
            RemoveSprite(weapon.GetComponent<SpriteRenderer>());
            Object.Destroy(weapon.gameObject);
            ShootManager = null;
        }

        GameObject weaponPrefab = ItemManager.GetItem((int)_id).Prefab;
        InitWeaponRig();

        float radius = weaponPrefab.GetComponent<Weapon>().ShootRadius;
        WeaponRig.localScale = new Vector3(radius, radius, 0);
        ShootManager ??= ShootColliderManager.New(WeaponRig, EnemyTag, 1);

        // set new weapon
        Transform newWeapon = Object.Instantiate(weaponPrefab, WeaponRig.position, Quaternion.identity).transform;
        SpriteRenderer sr = newWeapon.gameObject.GetComponent<SpriteRenderer>();
        sr.flipY = facingLeft;
        int diff = 0 - GetSortingPos();
        sr.sortingOrder += diff;
        newWeapon.SetParent(WeaponRig.parent);
        newWeapon.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
        weapon = newWeapon.gameObject.GetComponent<Weapon>();
        weaponId = _id;

        AddSprite(sr);

        return true;
    }

    private void InitWeaponRig()
    {
        WeaponRig ??= FindChild(MainTransform, "Weapon Rig");
        if (WeaponRig is null)
        {
            WeaponRig = new GameObject("Weapon Rig").transform;
            Transform transform = FindChild(MainTransform, "Weapon Hand").transform;
            if (transform is null)
            {
                GameObject.Destroy(WeaponRig);
                Debug.LogError("Entity doesn`t have Weapon Hand object, it can`t use weapon.");
                return;
            }

            WeaponRig.SetParent(transform);
        }
    }

    // ей здесь не место, но пусть. ее, кстати, написал чат бот
    // Рекурсивная функция для поиска дочернего объекта по имени
    private Transform FindChild(Transform parent, string name)
    {
        Transform result = null;

        foreach (Transform child in parent)
        {
            if (child.name == name)
            {
                result = child;
                break;
            }
            else
            {
                result = FindChild(child, name);
                if (result != null) break;
            }
        }

        return result;
    }
}