using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Models;
using OpenGLStudy.Enums;
using OpenTK.Mathematics;
using OpenGLStudy.Inputs;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Linq;

namespace OpenGLStudy.Components
{
    /// <summary>
    /// 玩家技能组件：按下Z键在玩家周围生成N个球，自动锁定范围内敌人进行攻击，技能有冷却
    /// </summary>
    internal class PlayerSkillComponent : Component
    {
        public float Cooldown = 5f; // 技能冷却时间（秒）
        private float cooldownTimer = 0f;
        public int BallCount = 3; // 可成长的球数量
        public float Range = 10f; // 技能作用范围

        public override void Update(float deltaTime)
        {
            if (cooldownTimer > 0)
                cooldownTimer -= deltaTime;

            if (cooldownTimer <= 0 && InputState.Keyboard.IsKeyDown(Keys.Z))
            {
                ReleaseSkill();
                cooldownTimer = Cooldown;
            }
        }

        private void ReleaseSkill()
        {
            // 1. 查找范围内的敌人
            var enemies = Owner.Scene.GetGameObjects<EnemyComponent>()
                .Where(obj => (obj.Transform.Position - Owner.Transform.Position).Length <= Range)
                .Take(BallCount)
                .ToList();

            // 2. 生成球体并锁定敌人
            for (int i = 0; i < BallCount; i++)
            {
                float angle = i * MathF.PI * 2 / BallCount;
                Vector3 offset = new Vector3(MathF.Cos(angle), 0, MathF.Sin(angle)) * 2f;
                Vector3 spawnPos = Owner.Transform.Position + offset;

                var ball = new GameObject("SkillBall", spawnPos)
                { Model = ModelManager.Instance!.GetModel(ModelType.Ball) };

                // 添加攻击组件（如有特殊逻辑可自定义）
                ball.AddComponent(new AttackComponent());

                // 添加追踪组件，锁定敌人
                if (i < enemies.Count)
                {
                    ball.AddComponent(new TrackTargetComponent
                    {
                        Target = enemies[i],
                        Speed = 8f,
                        LifeTime = 10f,
                        HitDistance = 1f
                    });
                }
                else
                {
                    // 没有目标时，球体10秒后自动消失
                    ball.AddComponent(new TrackTargetComponent
                    {
                        Target = null,
                        Speed = 0f,
                        LifeTime = 10f,
                        HitDistance = 1f
                    });
                }

                Owner.Scene.AddGameObject(ball);
            }
        }
    }
}