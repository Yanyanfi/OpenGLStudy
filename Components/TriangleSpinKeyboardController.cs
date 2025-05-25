using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components;

internal class TriangleSpinKeyboardController : Component
{
    public float Speed { get; set; } = 30f;
    protected override void CustomUpdate(float deltaTime) => Spin(deltaTime);
    private void Spin(float time)
    {
        float deltaAngle = Speed* time;
        Quaternion deltaRotation = Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(deltaAngle));
        Owner.Transform.Rotation *= deltaRotation;
    }
}
