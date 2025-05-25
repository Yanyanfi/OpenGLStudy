using OpenGLStudy.Inputs;
using OpenGLStudy.Shaders;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGLStudy.Components;

internal class ThirdPersonCameraDistanceController : Component
{
    private Camera? camera;
    public float MinDistance { get; set; } = 1.0f;
    public float MaxDistance { get; set; } = 5.0f;
    public float Speed { get; set; } = 1.0f;
    public override void Start()
    {
        Owner.TryGetComponent<Camera>(out camera);
    }
    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);
        ChangeDistance();
    }
    private void ChangeDistance()
    {
        if (camera is null)
            return;
        bool allowZoomIn = camera.ThirdPersonDistance > MinDistance;
        bool allowZoomOut= camera.ThirdPersonDistance < MaxDistance;
        if (allowZoomIn && InputState.Mouse.ScrollDelta.Y > 0 || allowZoomOut && InputState.Mouse.ScrollDelta.Y < 0)
        {
            camera.ThirdPersonDistance -= InputState.Mouse.ScrollDelta.Y * Speed;
        }
    }
}
