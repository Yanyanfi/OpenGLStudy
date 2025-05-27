using OpenGLStudy.Components.Cameras;
using OpenGLStudy.Inputs;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace OpenGLStudy.Components;

internal class PlayerDirectionController : Component
{
    private Camera camera = null!;
    private float tempYaw;
    private bool isFreeView;
    public override void Start()
    {
        camera = Owner.GetComponent<Camera>();
        FrameTimeLimit = camera.FrameTimeLimit;
    }
    protected override void CustomUpdate(float deltaTime)
    {
        if (!InputState.Keyboard.IsKeyDown(Keys.LeftAlt))
        {
            if (isFreeView)
            {
                camera.Yaw = tempYaw;
                isFreeView = false;
            }
            Owner.Transform.Rotation = Quaternion.FromEulerAngles(0f, MathHelper.DegreesToRadians(-camera.Yaw + 90), 0f);
        }
        else if (!isFreeView)
        {
            tempYaw = camera.Yaw;
            isFreeView = true;
        }
    }
}
