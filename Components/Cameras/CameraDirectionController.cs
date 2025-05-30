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
        camera?.YawDegree -= delta.X * Sensitivity;
        camera?.PitchDegree -= delta.Y * Sensitivity;
        camera?.PitchDegree = MathHelper.Clamp(camera.PitchDegree, -89, 89);
    }
}

