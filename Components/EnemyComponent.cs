using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;
internal class EnemyComponent : Component
{
    public int Health { get; set; } = 100;
    public int  droop_id= 0; // ����Ʒid
    public int Attack_id = 0; // ������ʽ
    public int damage = 50; // �����˺�

    public EnemyComponent() { }

    public EnemyComponent(int health)
    {
        Health = health;
    }
    public EnemyComponent(int health,int droop_id)
    {
        Health = health;
        this.droop_id = droop_id; // ������Ʒid
    }
    public EnemyComponent(int health = 100, int droop_id = 0, int attack_id = 0, int damage = 50)
    {
        Health = health;
        this.droop_id = droop_id;
        Attack_id = attack_id;
        this.damage = damage;
    }

    public override void Start()
    {
        // �ɳ�ʼ������ֵ��
    }

    public override void Update(float deltaTime)
    {
        if (Attack_id == 0) return; // ���������ʽΪ0��ִ���ƶ��͹����߼�
        // 1. ������Ҷ���
        if (Owner.Scene == null) return;
        if (!Owner.Scene.TryGetGameObject("Player", out var player)) return;

        // 2. ��ȡ��Һ͵��˵�λ��
        var playerPos = player.Transform.Position;
        var enemyPos = Owner.Transform.Position;

        // 3. ���㷽��;���
        var direction = playerPos - enemyPos;
        float distance = direction.Length;
        if (distance < 0.1f) return; // ����ܽ�ʱ�����ƶ�

        direction = direction.Normalized();

        // 4. �����ƶ��ٶȣ���λ/�룩
        float moveSpeed = 1.5f;
        if (distance < 10f)
            moveSpeed *= 4; // ����С��10ʱ�ٶȷ�4��

        // 5. �ƶ�����
        if (distance > 1f)
        {
            var move = direction * moveSpeed * deltaTime;
            Owner.Transform.Position += move;
        }
        else
        {
            // 6. �������������
            // ���������˾���Ϊ5f���ɸ�����Ҫ����
            float knockbackDistance = 5f;
            var knockback = direction * knockbackDistance;
            player.Transform.Position += knockback;

            // ���ڴ˴���Ӷ��������˺����߼�
            if (player.TryGetComponent<PlayerAttributeComponent>(out var attr))
            {
                attr.Health -= damage; // ����˺�
            }
        }
    }

    public void TakeDamage(int amount,GameObject gameObject)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            switch (droop_id) // ���ݵ���id���ɲ�ͬ��ս��Ʒ
            {
                case 1:
                    DropLoot_01(gameObject);
                    break;
                // ������Ӹ���case������ͬ�ĵ�����Ʒ
                default:
                    break;
            }
            // ����ֵ���㣬���ٵ���
            Owner.Scene.RemoveGameObjects(Owner);
        }
    }


    public void DropLoot_01(GameObject gameObject)// ����һ����ת�ŵ���
    {
        // ���������
        var ball = new GameObject("LootBall", Owner.Transform.Position)
        { Model = ModelManager.Instance!.GetModel(ModelType.Ball) };

        // ��ӹ������
        ball.AddComponent(new AttackComponent());
        // �����ת������˴�Target����ʰȡʱ����Ϊ��ң�
        ball.AddComponent(new RotateAroundComponent { Radius = 5f, Speed = 2f, Target = gameObject });
        // ��ӵ�����
        Owner.Scene.AddGameObject(ball);
    }

}