using OpenTK.Mathematics;
using OpenGLStudy;
using OpenGLStudy.Components;
using OpenGLStudy.Enums;
using OpenGLStudy.Models;

namespace OpenGLStudy.Components
{
    /// <summary>
    /// 敌人生成组件：定时在挂载单位周围生成敌人
    /// </summary>
    internal class EnemySpawnerComponent : Component
    {
        public float SpawnInterval { get; set; } = 10f; // 生成间隔（秒）
        public int SpawnCount { get; set; } = 3;       // 每次生成数量
        public int EnemyHealth { get; set; } = 100; // 敌人生命值
        public float SpawnRadius { get; set; } = 5f;   // 生成半径

        public EnemySpawnerComponent(
        float spawnInterval = 10f,
        int spawnCount = 3,
        int enemyHealth = 100,
        float spawnRadius = 5f)
        {
            SpawnInterval = spawnInterval;
            SpawnCount = spawnCount;
            EnemyHealth = enemyHealth;
            SpawnRadius = spawnRadius;
        }

        public EnemySpawnerComponent(  int enemyHealth = 100 )
        { 
            EnemyHealth = enemyHealth;
        }

        private float timer = 0f;

        public override void Update(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= SpawnInterval)
            {
                timer = 0f;
                SpawnEnemies();
            }
        }

        private void SpawnEnemies()
        {
            if (Owner == null || Owner.Scene == null) return;

            var center = Owner.Transform.Position;         

            for (int i = 0; i < SpawnCount; i++)
            {
                float angle = MathF.PI * 2 * i / SpawnCount + Random.Shared.NextSingle() * 0.5f;
                float radius = SpawnRadius * (0.8f + Random.Shared.NextSingle() * 0.4f);
                Vector3 pos = new Vector3(
                    center.X + radius * MathF.Cos(angle),
                    center.Y,
                    center.Z + radius * MathF.Sin(angle)
                );

                // 创建敌人对象
                var enemy = new GameObject($"Enemy_{Guid.NewGuid()}", pos) 
                { Model = ModelManager.Instance!.GetModel(ModelType.Girl) };//替换敌人模型

             
                var enemyComponent = new EnemyComponent { Health = EnemyHealth };
                enemy.AddComponent(enemyComponent);

                Scene.AddGameObject(enemy);
            }
        }
    }
}