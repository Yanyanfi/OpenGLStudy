using OpenGLStudy.Inputs;
using OpenTK.Mathematics;

namespace OpenGLStudy.Components.Cameras;

internal class CameraDirectionController : Component
{
    public float Sensitivity { get; set; } = 0.03f;
    private Camera camera = null!;
    public CameraDirectionController() { }
    public CameraDirectionController(float sensitivity) => Sensitivity = sensitivity;
    public override void Start() => camera = Owner.GetComponent<Camera>();
    public override void Update(float deltaTime) => HandleMouseMove();
    private void HandleMouseMove()
    {
        Vector2 delta = InputState.Mouse.Delta;
        camera?.Yaw += delta.X * Sensitivity;
        camera?.Pitch -= delta.Y * Sensitivity;
        camera?.Pitch = MathHelper.Clamp(camera.Pitch, -89, 89);
    }
}

