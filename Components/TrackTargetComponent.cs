using OpenGLStudy;
using OpenGLStudy.Components;
using OpenTK.Mathematics;

namespace OpenGLStudy.Components
{
    /// <summary>
    /// 追踪目标组件：让物体自动追踪目标GameObject，击中目标或超时后自动销毁
    /// </summary>
    internal class TrackTargetComponent : Component
    {
        public GameObject? Target { get; set; }
        public float Speed { get; set; } = 8f;
        public float LifeTime { get; set; } = 5f; // 最大存活时间（秒）
        private float timer = 0f;
        public float HitDistance { get; set; } = 1f; // 判定为击中的距离

        public override void Update(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= LifeTime)
            {
                Owner.Scene.RemoveGameObjects(Owner);
                return;
            }

            if (Target == null) return;

            var direction = Target.Transform.Position - Owner.Transform.Position;
            float distance = direction.Length;
            if (distance < HitDistance)
            {
                // 造成伤害
                if (Target.TryGetComponent<EnemyComponent>(out var enemy))
                {
                    // 你可以根据需要调整伤害数值
                    enemy.TakeDamage(200, Owner);
                }
                // 销毁自己
                Owner.Scene.RemoveGameObjects(Owner);
                return;
            }

            direction = direction.Normalized();
            Owner.Transform.Position += direction * Speed * deltaTime;
        }
    }
}