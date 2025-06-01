using OpenGLStudy;
internal class EnemyComponent : Component
{
    public int Health { get; private set; } = 100;

    public EnemyComponent() { }

    public EnemyComponent(int health)
    {
        Health = health;
    }

    public override void Start()
    {
        // 可初始化生命值等
    }

    public override void Update(float deltaTime)
    {
        // 可做AI等，这里无需实现
    }

    public void TakeDamage(int amount)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            // 生命值归零，销毁敌人
            Scene.RemoveGameObjects(Owner);
        }
    }
}