using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;
internal class EnemyComponent : Component
{
    public int Health { get; set; } = 100;
    public int  droop_id= 0; // 掉落品id
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

    public override void Start()
    {
        // 可初始化生命值等
    }

    public override void Update(float deltaTime)
    {
        // 可做AI等，这里无需实现
    }

    public void TakeDamage(int amount,GameObject gameObject)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            // 生命值归零，销毁敌人
            Owner.Scene.RemoveGameObjects(Owner);
            switch (droop_id) // 根据掉落id生成不同的战利品
            {
                case 1:
                    DropLoot_01(gameObject);
                    break;
                // 可以添加更多case来处理不同的掉落物品
                default:
                    break;
            }

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