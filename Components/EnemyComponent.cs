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
        // �ɳ�ʼ������ֵ��
    }

    public override void Update(float deltaTime)
    {
        // ����AI�ȣ���������ʵ��
    }

    public void TakeDamage(int amount)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            // ����ֵ���㣬���ٵ���
            Scene.RemoveGameObjects(Owner);
        }
    }
}