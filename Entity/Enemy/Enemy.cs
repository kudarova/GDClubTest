using UnityEngine;

// enemy - ������������ ���� - ��������� �� ������ � ���� ���������, ����� ����������� ���������� � ����
[System.Serializable]
public class Enemy : Entity
{
    public override string Name { get; } = "Enemy";

    public int Damage { get; private set; }
    public float AttackDistance { get; private set; }
    public float AttackDelay { get; private set; }
    public int ExperienceGained { get; private set; }

    // ����� � � ����� ������ ������� �� ��������,
    // � �������: ���� ������� ��������� ������������� ����� monobehavior, ������� ����� ������� �����-���� ����������
    // � ����� ��������� �� ������� ������������� ���� �����, ����� �� ������� ������� ����� ��������� ������� enemy,
    // ��� ������� �� ����� ��� ����� � �������, ����� ������� ��, �� ��� ������� ��� �� ������ ���������� ������ � ������ � �����
    public Enemy()
    {
        MaxHealth = 50;
        Health = MaxHealth;
        Speed = 1.8f;
        Damage = 10;
        AttackDistance = 1;
        AttackDelay = 3f;
        ExperienceGained = 5;
        EnemyTag = "Player";
        UpdateExperienceLevel(Level.Player.Experience);
    }
    protected override void LateLoad()
    {
        Healthbar = Statusbar.New(Color.black, Color.red, MaxHealth, Health, MainTransform);
        ObjectTransform.tag = "Enemy";

        MainTransform.gameObject.AddComponent<Pathfinding.AIDestinationSetter>().target = Level.Player.MainTransform;
        MainTransform.gameObject.GetComponent<Pathfinding.AIPath>().maxSpeed = Speed;
    }

    protected override void Update()
    {
        if (Vector3.Distance(Position, Level.Player.Position) < AttackDistance)
            Attack();

        UpdateSortingOrder();
        CheckFlip();
    }

    [System.NonSerialized] float lastAttack = 0;
    private void Attack()
    {
        if (lastAttack + AttackDelay > Time.time) return;
        Level.Player.TakeDamage(Damage);
        lastAttack = Time.time;
    }
    protected override void DeathBehavior() // drop random item if dead
    {
        Level.ItemManager.Drop(MainTransform.position);
    }
}