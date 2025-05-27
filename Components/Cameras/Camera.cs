using OpenGLStudy.Enums;
using OpenGLStudy.Shaders;
using OpenTK.Mathematics;

namespace OpenGLStudy.Components.Cameras;

/// <summary>
/// 支持第一人称视角和第三人称视角的摄像机
/// </summary>
internal class Camera : Component
{
    public float FOV { get; set; } = 60f;
    public bool EnableThirdPerson { get; set; } = false;
    public float ThirdPersonDistance { get; set; } = 3f;
    public float NearPlane { get; set; } = 0.1f;
    public float FarPlane { get; set; } = 100f;
    public float AspectRatio { get; set; } = 16f / 9f;
    public Vector3 PositionOffset { get; set; }
    public Vector3 Position => Owner.Transform.Position + PositionOffset;
    public Vector3 ThirdPersonPosition => Position - Front * ThirdPersonDistance;
    public float Yaw { get; set; } = 90;
    public float Pitch { get; set; } = 0;
    public Vector3 Front { get; private set; } = -Vector3.UnitZ;
    public Vector3 Up { get; private set; } = Vector3.UnitY;
    public Vector3 Right => Vector3.Normalize(Vector3.Cross(Front, Up));
    private Matrix4 ProjectionMatrix =>
        Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(FOV),
            AspectRatio,
            NearPlane,
            FarPlane
        );
    private Matrix4 ViewMatrix => Matrix4.LookAt(Position, Position + Front, Up);
    private Matrix4 TPViewMatrix => Matrix4.LookAt(ThirdPersonPosition, ThirdPersonPosition + Front, Up);

    public Camera(Vector3 positionOffset, float aspectRatio, bool thirdPerson = false)
    {
        PositionOffset = positionOffset;
        AspectRatio = aspectRatio;
        EnableThirdPerson = thirdPerson;
        FrameTimeLimit = 1 / 165f;
    }

    protected override void CustomUpdate(float deltaTime)
    {
        Shader.Instance?.SetMvpMatrix(MVPMatrixTarget.View, EnableThirdPerson ? TPViewMatrix : ViewMatrix);
        Shader.Instance?.SetMvpMatrix(MVPMatrixTarget.Projection, ProjectionMatrix);
        UpdateFront();
    }
    private void UpdateFront()
    {
        Front = new Vector3(
            MathF.Cos(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(Pitch)),
            MathF.Sin(MathHelper.DegreesToRadians(Yaw)) * MathF.Cos(MathHelper.DegreesToRadians(Pitch))
        );
    }
}
