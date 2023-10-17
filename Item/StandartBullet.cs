using UnityEngine;

public class StandartBullet : Bullet
{
    protected override void BulletBehaviour()
    {
        dynamicSpeed = Mathf.Lerp(Speed, Speed / 2, currentLifeTime / lifeTime);

        // Перемещаем пулю в направлении цели
        transform.position += (targetPos - transform.position) * dynamicSpeed * Time.deltaTime;
    }
}