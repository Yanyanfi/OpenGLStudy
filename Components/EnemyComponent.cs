using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;
internal class EnemyComponent : Component
{
    public int Health { get; set; } = 100;
    public int  droop_id= 0; // 掉落品id
    public int Attack_id = 0; // 攻击方式
    public int damage = 50; // 攻击伤害

    public EnemyComponent() { }

    public EnemyComponent(int health)
    {
        Health = health;
    }
    public EnemyComponent(int health,int droop_id)
    {
        Health = health;
        this.droop_id = droop_id; // 掉落物品id
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
        // 可初始化生命值等
    }

    public override void Update(float deltaTime)
    {
        if (Attack_id == 0) return; // 如果攻击方式为0则不执行移动和攻击逻辑
        // 1. 查找玩家对象
        if (Owner.Scene == null) return;
        if (!Owner.Scene.TryGetGameObject("Player", out var player)) return;

        // 2. 获取玩家和敌人的位置
        var playerPos = player.Transform.Position;
        var enemyPos = Owner.Transform.Position;

        // 3. 计算方向和距离
        var direction = playerPos - enemyPos;
        float distance = direction.Length;
        if (distance < 0.1f) return; // 距离很近时不再移动

        direction = direction.Normalized();

        // 4. 设置移动速度（单位/秒）
        float moveSpeed = 1.5f;
        if (distance < 10f)
            moveSpeed *= 4; // 距离小于10时速度翻4倍

        // 5. 移动敌人
        if (distance > 1f)
        {
            var move = direction * moveSpeed * deltaTime;
            Owner.Transform.Position += move;
        }
        else
        {
            // 6. 攻击并击退玩家
            // 这里假设击退距离为5f，可根据需要调整
            float knockbackDistance = 5f;
            var knockback = direction * knockbackDistance;
            player.Transform.Position += knockback;

            // 可在此处添加对玩家造成伤害的逻辑
            if (player.TryGetComponent<PlayerAttributeComponent>(out var attr))
            {
                attr.Health -= damage; // 造成伤害
            }
        }
    }

    public void TakeDamage(int amount,GameObject gameObject)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            switch (droop_id) // 根据掉落id生成不同的战利品
            {
                case 1:
                    DropLoot_01(gameObject);
                    break;
                // 可以添加更多case来处理不同的掉落物品
                default:
                    break;
            }
            // 生命值归零，销毁敌人
            Owner.Scene.RemoveGameObjects(Owner);
        }
    }


    public void DropLoot_01(GameObject gameObject)// 掉落一个旋转着的球
    {
        // 生成球对象
        var ball = new GameObject("LootBall", Owner.Transform.Position)
        { Model = ModelManager.Instance!.GetModel(ModelType.Ball) };

        // 添加攻击组件
        ball.AddComponent(new AttackComponent());
        // 添加旋转组件（此处Target需在拾取时设置为玩家）
        ball.AddComponent(new RotateAroundComponent { Radius = 5f, Speed = 2f, Target = gameObject });
        // 添加到场景
        Owner.Scene.AddGameObject(ball);
    }

}