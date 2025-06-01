using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;
internal class AttackComponent : Component
{
    public int Damage { get; set; } = 50;
    public float Cooldown { get; set; } = 1.0f; // 冷却时间，单位秒
    public float AttackRange { get; set; } = 3.0f; // 攻击范围
    private float cooldownTimer = 0f;

    public override void Update(float deltaTime)
    {
        if (cooldownTimer > 0)
        {
            cooldownTimer -= deltaTime;
            return;
        }
        //查找玩家对象
        if (Owner.Scene == null) return;
        if (!Owner.Scene.TryGetGameObject("Player", out var player)) return;

        var attr = player.GetComponent<PlayerAttributeComponent>();

        var gameObjectsCopy = new List<GameObject>(Owner.Scene.GetAllGameObjects());
        foreach (var obj in gameObjectsCopy)
        {
            if (obj == Owner) continue;
            if (obj.TryGetComponent<EnemyComponent>(out var enemy))
            {
                if (IsColliding(Owner, obj))
                {
                    enemy!.TakeDamage(Damage+attr.Attack,Owner);
                    cooldownTimer = Cooldown; // 重置冷却
                    break; // 只攻击一个目标
                }
            }
        }
    }

    private bool IsColliding(GameObject a, GameObject b)
    {
        var apos = a.Transform.Position;
        var bpos = b.Transform.Position;
        float dist = (apos - bpos).Length;
        return dist < AttackRange;
    }
}