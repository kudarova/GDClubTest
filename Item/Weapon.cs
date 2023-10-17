using System;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Range(.1f, 10)]
    [SerializeField] private float shootDelay;
    [field: Range(5, 15)]
    [field: SerializeField] public float ShootRadius { get; private set; }
    [SerializeField] private GameObject bulletPrefab;

    private Transform shootPos;
    private List<Bullet> bullets = new List<Bullet>();

    private void Start()
    {
        shootDelay /= 10;
        shootPos = transform.GetChild(0);
    }

    private float lastShoot = 0;
    public bool Shoot(Transform target)
    {
        if (lastShoot + shootDelay <= Time.time)
        {
            GetBullet().Set(shootPos.position, target);
            lastShoot = Time.time;
            return true;
        }

        return false;
    }

    private Bullet GetBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].gameObject.activeSelf)
                return bullets[i];
        }

        return NewBullet();
    }

    private Bullet NewBullet()
    {
        Bullet newBullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullets.Add(newBullet);
        return newBullet;
    }
}