using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;
internal class EnemyComponent : Component
{
    public int Health { get; set; } = 100;
    public int  droop_id= 0; // ����Ʒid
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

    public override void Start()
    {
        // �ɳ�ʼ������ֵ��
    }

    public override void Update(float deltaTime)
    {
        // ����AI�ȣ���������ʵ��
    }

    public void TakeDamage(int amount,GameObject gameObject)
    {
        Console.WriteLine("-" + amount);
        Health -= amount;
        if (Health <= 0)
        {
            // ����ֵ���㣬���ٵ���
            Owner.Scene.RemoveGameObjects(Owner);
            switch (droop_id) // ���ݵ���id���ɲ�ͬ��ս��Ʒ
            {
                case 1:
                    DropLoot_01(gameObject);
                    break;
                // ������Ӹ���case������ͬ�ĵ�����Ʒ
                default:
                    break;
            }

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