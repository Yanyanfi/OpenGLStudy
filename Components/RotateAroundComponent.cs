using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenGLStudy;
using OpenTK.Mathematics;

namespace OpenGLStudy.Components
{
    internal class RotateAroundComponent : Component
    {
        public GameObject Target { get; set; } // 旋转中心（玩家）
        public float Radius { get; set; } = 3f;
        public float Speed { get; set; } = 2f; // 旋转速度
        private float angle = 0f;

        public override void Update(float deltaTime)
        {
            if (Target == null) return;
            angle += Speed * deltaTime;
            var center = Target.Transform.Position;
            Owner.Transform.Position = new Vector3(
                center.X + Radius * MathF.Cos(angle),
                center.Y,
                center.Z + Radius * MathF.Sin(angle)
            );
        }
    }
}
